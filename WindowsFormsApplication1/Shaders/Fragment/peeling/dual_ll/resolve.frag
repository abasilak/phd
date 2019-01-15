#include "define.h"

// IN
uniform int	   layer;
uniform float  transparency;
uniform bool   useTransparency;
uniform uint   total_fragments;

#if multisample
	layout(binding = 2, r32ui) readonly uniform uimage2DMS   image_page_id_front;
	layout(binding = 3, r32ui) readonly uniform uimage2DMS   image_page_id_back;
#if sorted_by_id
	layout(binding = 4, r32ui) readonly uniform uimage2DMS   image_counter_front;
	layout(binding = 5, r32ui) readonly uniform uimage2DMS   image_counter_back;
#endif
#else
	layout(binding = 2, r32ui) readonly uniform uimage2DRect image_page_id_front;
	layout(binding = 3, r32ui) readonly uniform uimage2DRect image_page_id_back;
#if sorted_by_id
	layout(binding = 4, r32ui) readonly uniform uimage2DRect image_counter_front;
	layout(binding = 5, r32ui) readonly uniform uimage2DRect image_counter_back;
#endif
#endif 
#if sorted_by_id
	layout(binding = 6, rg32f) coherent uniform  imageBuffer image_peel;
#else
	layout(binding = 6, rgba8) readonly uniform  imageBuffer image_peel;
#endif 
	layout(binding = 7, r32ui) readonly uniform uimageBuffer image_pointers;

// OUT
	layout(location = 0, index = 0) out  vec4 out_frag_color_front;
	layout(location = 1, index = 0) out  vec4 out_frag_color_back;

#if multisample
	uint getPixelCurrentPageID_front(ivec2 coords)	{return imageLoad(image_page_id_front  , coords, gl_SampleID).x;}
	uint getPixelCurrentPageID_back(ivec2 coords)	{return imageLoad(image_page_id_back   , coords, gl_SampleID).x;}
#if sorted_by_id
	uint getPixelFragCounter_front  (ivec2 coords)	{return imageLoad(image_counter_front  , coords, gl_SampleID).x;}
	uint getPixelFragCounter_back  (ivec2 coords)	{return imageLoad(image_counter_back   , coords, gl_SampleID).x;}
#endif
#else
	uint getPixelCurrentPageID_front(ivec2 coords)	{return imageLoad(image_page_id_front  , coords).x;}
	uint getPixelCurrentPageID_back(ivec2 coords)	{return imageLoad(image_page_id_back   , coords).x;}
#if sorted_by_id	
	uint getPixelFragCounter_front  (ivec2 coords)	{return imageLoad(image_counter_front  , coords).x;}
	uint getPixelFragCounter_back  (ivec2 coords)	{return imageLoad(image_counter_back   , coords).x;}
#endif
#endif
	vec4 sharedPoolGetValue			(uint index)	{return imageLoad(image_peel	, int(index));}
	uint sharedPoolGetLink			(uint index)	{return imageLoad(image_pointers, int(index)).x;}

#if sorted_by_id
	void sharedPoolSetValue			(uint index, vec4 val) {imageStore(image_peel, int(index), val);}
#endif	

	vec4 resolveAlphaBlend(const uint page_id, const bool front);
#if sorted_by_id
	vec4 resolveClosest	  (const uint page_id, const bool front);
	void bubbleSort		  (const uint page_id, const uint num, const bool front);
#endif

void main(void)
{
	ivec2 coords = ivec2(gl_FragCoord.xy);

	uint cur_page_id_front = getPixelCurrentPageID_front(coords);
	if(cur_page_id_front > 0U)
	{
#if sorted_by_id
		uint cur_counter_front = getPixelFragCounter_front(coords);
		if(cur_counter_front > 0U)
#endif
		{
			vec4 c = vec4(0.0f);
#if sorted_by_id
			bubbleSort(cur_page_id_front, cur_counter_front, true);
#endif
			if(useTransparency)
				c = resolveAlphaBlend(cur_page_id_front, true);
			else
			{
//#if sorted_by_id
	//			c = resolveClosest	 (cur_page_id_front, cur_counter_front);
//#else			
#if sorted_by_id
				if(int(cur_counter_front) <= layer)
					return;
#endif
				int i=0;
				while(cur_page_id_front != 0U)
				{
					if (i++ == layer)
					{
						c = sharedPoolGetValue(cur_page_id_front);
#if sorted_by_id
						c = unpackUnorm4x8(uint(c.r)).abgr;
#endif
						break;
					}
					cur_page_id_front = sharedPoolGetLink(cur_page_id_front);
				}

//#endif
			}		
			out_frag_color_front = c;
		}
	}
	
	uint cur_page_id_back  = total_fragments - getPixelCurrentPageID_back(coords);	
	if(cur_page_id_back > 0U)
	{
#if sorted_by_id
		uint cur_counter_back = getPixelFragCounter_back(coords);
		if(cur_counter_back > 0U)
#endif
		{
			vec4 c = vec4(0.0f);
#if sorted_by_id
			bubbleSort(cur_page_id_back, cur_counter_back, false);
#endif
			if(useTransparency)
				c = resolveAlphaBlend(cur_page_id_back, false);
			else
			{
//#if sorted_by_id
	//			c = resolveClosest	 (cur_page_id_back, cur_counter_back);
//#else			
#if sorted_by_id
				if(int(cur_counter_back) <= layer)
					return;
#endif
				int i=0;
				while(cur_page_id_back != 0U)
				{
					if (i++ == layer)
					{
						c = sharedPoolGetValue(cur_page_id_back);
#if sorted_by_id
						c = unpackUnorm4x8(uint(c.r)).abgr;
#endif
						break;
					}
					cur_page_id_back = total_fragments - sharedPoolGetLink(cur_page_id_back);
				}
//#endif
			}		
			out_frag_color_back = c;
		}
	}
}

vec4 resolveAlphaBlend(const uint page_id, const bool front)
{
	vec4  finalColor = vec4(0.0f), Ci;
	uint  cur_page_id = page_id;

	while(cur_page_id != 0U)
	{
		Ci = sharedPoolGetValue(cur_page_id);
#if sorted_by_id
		Ci = vec4(unpackUnorm4x8(uint(Ci.r)).abg, transparency);
#endif
		if(front)
			finalColor += Ci*(1.0f-finalColor.a);
		else
		{
			finalColor *= (1.0f-finalColor.a);
			finalColor += Ci;
		}
		cur_page_id = sharedPoolGetLink(cur_page_id);
		if(!front)
			cur_page_id = total_fragments - cur_page_id;
	}
	return finalColor;
}

#if sorted_by_id
vec4 resolveClosest(const uint page_id, const bool front)
{
	uint cur_page_id = page_id;
	vec2 value, minFrag = vec2(0.0f, 0.0f);

	while(cur_page_id != 0U)
	{
		value = sharedPoolGetValue(cur_page_id).rg;
		if(value.g > minFrag.g)
			minFrag = value;

		cur_page_id = sharedPoolGetLink(cur_page_id);
		if(!front)
			cur_page_id = total_fragments - cur_page_id;
	}
	return unpackUnorm4x8(uint(minFrag.r)).abgr;
}

///						///
//	SORTING ALGORITHMS	 //
///						///
void bubbleSort(const uint page_id, const uint num, const bool front)
{
	vec4 c0, c1;
	uint p0, p1;
	
	for (int i = (int(num) - 2); i >= 0; --i)
	{
		p0 = page_id;
		for (int j = 0; j <= i; ++j)
		{
			p1 = sharedPoolGetLink (p0);
			if(!front)
				p1 = total_fragments - p1;

			c0 = sharedPoolGetValue(p0);
			c1 = sharedPoolGetValue(p1);
			if (c0.g < c1.g)
			{
				sharedPoolSetValue (p0, c1);
				sharedPoolSetValue (p1, c0);
			}
			p0 = p1;
		}
	}
}
#endif