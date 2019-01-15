#include "define.h"

// IN
uniform int   layer;
uniform bool  useTransparency;

#if multisample
	layout(binding = 1, r32ui ) readonly uniform uimage2DMS		image_count_front;
	layout(binding = 4, r32ui ) readonly uniform uimage2DMS		image_count_back;
#if sorted_by_id
	layout(binding = 5, rg32ui) coherent uniform uimage2DMSArray image_peel_front;
	layout(binding = 6, rg32ui) coherent uniform uimage2DMSArray image_peel_back;
#else
	layout(binding = 5, rgba8 ) coherent uniform image2DMSArray  image_peel_front;
	layout(binding = 6, rgba8 ) coherent uniform image2DMSArray  image_peel_back;
#endif 
		
#else
	layout(binding = 1, r32ui ) readonly uniform uimage2DRect  image_count_front;
	layout(binding = 4, r32ui ) readonly uniform uimage2DRect  image_count_back;
#if sorted_by_id
	layout(binding = 5, rg32ui) coherent uniform uimage2DArray image_peel_front;
	layout(binding = 6, rg32ui) coherent uniform uimage2DArray image_peel_back;
#else
	layout(binding = 5, rgba8 ) coherent uniform image2DArray  image_peel_front;
	layout(binding = 6, rgba8 ) coherent uniform image2DArray  image_peel_back;
#endif 

#endif

// OUT
layout(location = 0, index = 0) out  vec4 out_frag_color_front;
layout(location = 1, index = 0) out  vec4 out_frag_color_back;

////////
#if sorted_by_id
	void bubbleSort_front(const ivec2 coords, const int num);
	void bubbleSort_back (const ivec2 coords, const int num);
#endif
	vec4 resolveAlphaBlend_front(const ivec2 coords, const int num);
	vec4 resolveAlphaBlend_back (const ivec2 coords, const int num);

void main(void)
{
	int   count_front, count_back;
	ivec2 coords  = ivec2(gl_FragCoord.xy);

#if multisample
	count_front = int(imageLoad(image_count_front, coords, gl_SampleID).r);
	count_back  = int(imageLoad(image_count_back , coords, gl_SampleID).r);
#else
	count_front = int(imageLoad(image_count_front, coords).r);
	count_back  = int(imageLoad(image_count_back , coords).r);
#endif 

	if(count_front == 0 && count_back == 0)
		discard;

	if(count_front > 0)
	{
		vec4 c;
#if sorted_by_id
		bubbleSort_front(coords, count_front);
#endif
		if(useTransparency)  
			c = resolveAlphaBlend_front(coords, count_front);
		else
		{

#if multisample

#if sorted_by_id
			c  = unpackUnorm4x8(imageLoad(image_peel_front, ivec3(coords, layer), gl_SampleID).r).abgr;
#else
			c  = imageLoad(image_peel_front, ivec3(coords, layer), gl_SampleID);
#endif

#else

#if sorted_by_id
			c  = unpackUnorm4x8(imageLoad(image_peel_front, ivec3(coords, layer)).r).abgr;
#else
			c  = imageLoad(image_peel_front, ivec3(coords, layer));
#endif

#endif
		}
		out_frag_color_front = c;
	}
	
	if(count_back > 0)
	{
		vec4 c;
#if sorted_by_id
		bubbleSort_back(coords, count_back);
#endif
		if(useTransparency)  
			c = resolveAlphaBlend_back(coords, count_back);
		else
		{

#if multisample

#if sorted_by_id
			c  = unpackUnorm4x8(imageLoad(image_peel_back, ivec3(coords, layer), gl_SampleID).r).abgr;
#else
			c  = imageLoad(image_peel_back, ivec3(coords, layer), gl_SampleID);
#endif

#else

#if sorted_by_id
			c  = unpackUnorm4x8(imageLoad(image_peel_back, ivec3(coords, layer)).r).abgr;
#else
			c  = imageLoad(image_peel_back, ivec3(coords, layer));
#endif

#endif
		}
		out_frag_color_back = c;
	}
}

vec4 resolveAlphaBlend_front(const ivec2 coords, const int num)
{
	vec4 Ci, finalColor = vec4(0.0f);
	
	for(int i=0; i<num; i++)
	{
#if multisample

#if sorted_by_id
		Ci  = unpackUnorm4x8(imageLoad(image_peel_front, ivec3(coords, i), gl_SampleID).r).abgr;
#else
		Ci  = imageLoad(image_peel_front, ivec3(coords, i), gl_SampleID);
#endif

#else

#if sorted_by_id
		Ci  = unpackUnorm4x8(imageLoad(image_peel_front, ivec3(coords, i)).r).abgr;
#else
		Ci  = imageLoad(image_peel_front, ivec3(coords, i));
#endif

#endif
		finalColor += Ci*(1.0f-finalColor.a);
	}
	return finalColor;
}

vec4 resolveAlphaBlend_back(const ivec2 coords, const int num)
{
	vec4 Ci, finalColor = vec4(0.0f);
	
	for(int i=0; i<num; i++)
	{
#if multisample

#if sorted_by_id
		Ci  = unpackUnorm4x8(imageLoad(image_peel_back, ivec3(coords, i), gl_SampleID).r).abgr;
#else
		Ci  = imageLoad(image_peel_back, ivec3(coords, i), gl_SampleID);
#endif

#else

#if sorted_by_id
		Ci  = unpackUnorm4x8(imageLoad(image_peel_back, ivec3(coords, i)).r).abgr;
#else
		Ci  = imageLoad(image_peel_back, ivec3(coords, i));
#endif

#endif
		finalColor *= (1.0f-finalColor.a);
		finalColor += Ci;
	}
	return finalColor;
}

#if sorted_by_id

void bubbleSort_front(const ivec2 coords, const int num)
{
	uvec4 c0,c1;
	for (int i = (num - 2); i >= 0; --i)
	{
		for (int j = 0; j <= i; ++j)
		{
#if multisample
			c0 = imageLoad(image_peel_front, ivec3(coords, j  ), gl_SampleID);
			c1 = imageLoad(image_peel_front, ivec3(coords, j+1), gl_SampleID);
#else
			c0 = imageLoad(image_peel_front, ivec3(coords, j));
			c1 = imageLoad(image_peel_front, ivec3(coords, j+1));
#endif 	
			if (c0.g < c1.g)
			{
#if multisample
				imageStore(image_peel_front, ivec3(coords, j  ), gl_SampleID, c1);
				imageStore(image_peel_front, ivec3(coords, j+1), gl_SampleID, c0);
#else
				imageStore(image_peel_front, ivec3(coords, j  ), c1);
				imageStore(image_peel_front, ivec3(coords, j+1), c0);
#endif 	
			}
		}
	}
}

void bubbleSort_back(const ivec2 coords, const int num)
{
	uvec4 c0,c1;
	for (int i = (num - 2); i >= 0; --i)
	{
		for (int j = 0; j <= i; ++j)
		{
#if multisample
			c0 = imageLoad(image_peel_back, ivec3(coords, j  ), gl_SampleID);
			c1 = imageLoad(image_peel_back, ivec3(coords, j+1), gl_SampleID);
#else
			c0 = imageLoad(image_peel_back, ivec3(coords, j));
			c1 = imageLoad(image_peel_back, ivec3(coords, j+1));
#endif 	
			if (c0.g < c1.g)
			{
#if multisample
				imageStore(image_peel_back, ivec3(coords, j  ), gl_SampleID, c1);
				imageStore(image_peel_back, ivec3(coords, j+1), gl_SampleID, c0);
#else
				imageStore(image_peel_back, ivec3(coords, j  ), c1);
				imageStore(image_peel_back, ivec3(coords, j+1), c0);
#endif 	
			}
		}
	}
}

#endif