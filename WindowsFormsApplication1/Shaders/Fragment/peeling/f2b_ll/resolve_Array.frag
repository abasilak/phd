#version 410 core
#extension GL_NV_gpu_shader5 : enable
#extension GL_NV_shader_buffer_load : enable
#extension GL_EXT_shader_image_load_store : enable

#define ABUFFER_SIZE 16

precision highp float;
// IN
uniform bool   correctAlpha;
uniform bool   useTransparency;
uniform bool   useTranslucency;
uniform float  transparency;
uniform vec4   color_background;
		 
		 uniform uint							page_size;
coherent uniform layout(size1x32) uimage2DRect	image_page_id;
coherent uniform layout(size1x32) uimage2DRect	image_counter;
coherent uniform layout(size4x32) imageBuffer	shared_page_list;
coherent uniform layout(size1x32) uimageBuffer  shared_link_list;
// OUT
layout(location = 0) out vec4 out_frag_color;

vec4 fragmentList[ABUFFER_SIZE];

void bubbleSort		  (int num);
vec4 resolveAlphaBlend(int num);
void fillFragmentArray(int pageIdx, int abNumFrag);

vec4 resolveClosest	  (int pageIdx, int abNumFrag);

uint getPixelCurrentPageID(ivec2 coords){return imageLoad(image_page_id	  , coords).x;}
uint getPixelFragCounter  (ivec2 coords){return imageLoad(image_counter	  , coords).x;}
vec4 sharedPoolGetValue	  (uint index)  {return imageLoad(shared_page_list, (int)index);}
uint sharedPoolGetLink	  (uint pageNum){return imageLoad(shared_link_list, (int)(pageNum) ).x;}

void main(void)
{
	ivec2 coords = ivec2(gl_FragCoord.xy);

	int cur_counter;
	int cur_page_id = (int) getPixelCurrentPageID(coords);

	if(cur_page_id > 0)
	{
		cur_counter = (int)getPixelFragCounter(coords);
		if(cur_counter > 0)
		{
			if(useTransparency || useTranslucency)
			{
				fillFragmentArray(cur_page_id, cur_counter);
				cur_counter = min(cur_counter, ABUFFER_SIZE); // ?? why ABUFFER_SIZE ?
				bubbleSort(cur_counter);

				out_frag_color = resolveAlphaBlend(cur_counter);
			}
			else
				out_frag_color = resolveClosest	  (cur_page_id, cur_counter);

	//		out_frag_color = vec4(cur_counter/float(ABUFFER_SIZE)); // ?? why ABUFFER_SIZE ?
		}
	}
	else
	{
			discard;
	//		out_frag_color = vec4(0.0f);
	}
}

//Load fragments into a local memory array for sorting
void fillFragmentArray(int pageIdx, int abNumFrag)
{
	int ip=0;
	int fi=0;
	int numElem;
	int curPage=pageIdx;

	while(curPage!=0 && ip<20){ // 20 ???
		if( ip == 0)
		{
			numElem=abNumFrag % int(page_size);
			if(numElem == 0)
				numElem = int(page_size);
		}
		else
			numElem = int(page_size);
	
		for(int i=0; i<numElem; i++)
		{
			if(fi < ABUFFER_SIZE)
				fragmentList[fi]=sharedPoolGetValue(curPage+i);
			fi++; // ??? remove
		}

		curPage = (int)sharedPoolGetLink(curPage/int(page_size));
		ip++;
	}

}

vec4 resolveAlphaBlend(int num){
	
	float thickness, prevZ = 0.0f;;
	const float sigma = 20.0f;
	
	thickness = (useTranslucency) ? 0.0 : fragmentList[0].w/2.0f;
	
	vec4 color;
	vec4 finalColor = vec4(0.0f);

	for(int i=0; i<num; i++){		
		if(useTranslucency)
		{
			if(i%2==1) thickness += fragmentList[i].w - prevZ;
			prevZ = fragmentList[i].w;
		}
		if(useTransparency)
		{
			color.rgb= fragmentList[i].rgb;
			color.w  = transparency;
			if(correctAlpha){
				if(i%2 == num%2)
					thickness=(fragmentList[i+1].w-fragmentList[i].w)*0.5f;
				color.w=1.0f-pow(1.0f-color.w, thickness* sigma );
			}
			color.rgb *= color.w;
			finalColor = finalColor + color*(1.0f-finalColor.a);		
		}
	}
	
	if	   (useTranslucency &&  useTransparency) finalColor *= exp(-sigma*thickness);
	else if(useTranslucency && !useTransparency) finalColor  = exp(-sigma*thickness)*fragmentList[0];

	if(useTransparency) finalColor += color_background*(1.0f-finalColor.a);

	return finalColor;
}

vec4 resolveClosest(int pageIdx, int abNumFrag){

	int  ip=0;
	int  numElem;
	int  curPage = pageIdx;
	vec4 value;
	vec4 minFrag=vec4(0.0f, 0.0f, 0.0f, 1.0f);

	while(curPage!=0 && ip<20){
		if(ip==0)
		{
			numElem=abNumFrag % int(page_size);
			if(numElem == 0)
				numElem = int(page_size);
		}
		else
			numElem = int(page_size);

		for(int i=0; i<numElem; i++)
		{
			value = sharedPoolGetValue(curPage+i);
			if(value.w < minFrag.w)
				minFrag = value;
		}

		curPage = (int)sharedPoolGetLink(curPage/int(page_size)); //Get next page index
		ip++;
	}

	return minFrag;
}

//Bubble sort used to sort fragments
void bubbleSort(int num) {
  for (int i = (num - 2); i >= 0; --i) {
    for (int j = 0; j <= i; ++j) {
      if (fragmentList[j].w > fragmentList[j+1].w) {
		vec4 temp = fragmentList[j+1];
		fragmentList[j+1] = fragmentList[j];
		fragmentList[j] = temp;
      }
    }
  }
}