#include "define.h"

// IN
uniform int	   layer;
uniform float  transparency;
uniform bool   useTransparency;

#if sorted_by_id
	layout(binding = 4, rg32f) coherent uniform  imageBuffer image_peel;
#else
	layout(binding = 4, rgba8) readonly uniform  imageBuffer image_peel;
#endif 
	layout(binding = 5, r32ui) readonly uniform uimageBuffer image_pointers;

#if multisample
	layout(binding = 6, r32ui) readonly uniform uimage2DMS   image_page_id;
#if sorted_by_id
	layout(binding = 7, r32ui) readonly uniform uimage2DMS   image_counter;
#endif
#else
	layout(binding = 6, r32ui) readonly uniform uimage2DRect image_page_id;
#if sorted_by_id
	layout(binding = 7, r32ui) readonly uniform uimage2DRect image_counter;
#endif
#endif 

// OUT
	layout(location = 0, index = 0) out  vec4 out_frag_color;

#if multisample
	uint getPixelCurrentPageID(const ivec2 coords){return imageLoad(image_page_id, coords, gl_SampleID).x;}
#if sorted_by_id	
	uint getPixelFragCounter  (const ivec2 coords){return imageLoad(image_counter, coords, gl_SampleID).x;}
#endif
#else
	uint getPixelCurrentPageID(const ivec2 coords){return imageLoad(image_page_id, coords).x;}
#if sorted_by_id	
	uint getPixelFragCounter  (const ivec2 coords){return imageLoad(image_counter, coords).x;}
#endif
#endif
	vec4 sharedPoolGetValue	  (const uint index  ){return imageLoad(image_peel	  , int(index));}
	uint sharedPoolGetLink    (const uint index  ){return imageLoad(image_pointers, int(index)).x;}
#if sorted_by_id
	void sharedPoolSetValue	  (const uint index, const vec4 val){imageStore(image_peel, int(index), val);}
#endif	

	vec4 resolveAlphaBlend(const uint page_id);
#if sorted_by_id
	vec4 resolveClosest	  (const uint page_id);
	void bubbleSort		  (const uint page_id, const int num);
#endif

void main(void)
{
	ivec2 coords = ivec2(gl_FragCoord.xy);

	uint  page_id = getPixelCurrentPageID(coords);
	if(page_id > 0U)
	{
#if sorted_by_id
		int counter = int(getPixelFragCounter(coords));
		if(counter > 0)
#endif
		{
			vec4 c = vec4(0.0f);
#if sorted_by_id
			bubbleSort(page_id, counter);
#endif
			if(useTransparency)
				c = resolveAlphaBlend(page_id);
			else
			{
//#if sorted_by_id
	//			c = resolveClosest	 (page_id);
//#else			
#if sorted_by_id
				if(counter <= layer)
					discard;
#endif
				int i=0;
				while(page_id != 0U)
				{
					if (i++ == layer)
					{
						c = sharedPoolGetValue(page_id);
#if sorted_by_id
						c = unpackUnorm4x8(uint(c.r)).abgr;
#endif
						break;
					}
					page_id = sharedPoolGetLink(page_id);
				}
//#endif
			}		
			out_frag_color = c;
		}
	}
	else
		discard;
}

vec4 resolveAlphaBlend(const uint page_id)
{
	vec4  finalColor = vec4(0.0f), Ci;
	uint  cur_page_id = page_id;

	while(cur_page_id != 0)
	{
		Ci = sharedPoolGetValue(cur_page_id);
#if sorted_by_id
		Ci = vec4(unpackUnorm4x8(uint(Ci.r)).abg, transparency);
#endif
		finalColor += Ci*(1.0f-finalColor.a);
		cur_page_id = sharedPoolGetLink(cur_page_id);
	}
	return finalColor;
}

#if sorted_by_id
vec4 resolveClosest(const uint page_id)
{
	uint cur_page_id = page_id;
	vec2 value, minFrag = vec2(0.0f, 0.0f);

	while(cur_page_id != 0U)
	{
		value = sharedPoolGetValue(cur_page_id).rg;
		if(value.g > minFrag.g)
			minFrag = value;

		cur_page_id = sharedPoolGetLink(cur_page_id);
	}
	return unpackUnorm4x8(uint(minFrag.r)).abgr;
}

void bubbleSort(const uint page_id, const int num)
{
	vec4 c0, c1;
	uint p0, p1;
	
	for (int i = (num - 2); i >= 0; --i)
	{
		p0 = page_id;
		for (int j = 0; j <= i; ++j)
		{
			p1 = sharedPoolGetLink (p0);
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