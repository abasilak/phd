#include "define.h"

// IN
uniform int   layer;
uniform bool  useTransparency;

#if multisample
	layout(binding = 4, r32ui ) readonly uniform uimage2DMS	     image_counter;
#if sorted_by_id
	layout(binding = 3, rg32ui) coherent uniform uimage2DMSArray image_peel;
#else
	layout(binding = 3, rgba8 ) readonly uniform image2DMSArray  image_peel;
#endif
#else
	layout(binding = 4, r32ui ) readonly uniform uimage2DRect	 image_counter;
#if sorted_by_id
	layout(binding = 3, rg32ui) coherent uniform uimage2DArray	 image_peel;
#else
	layout(binding = 3, rgba8 ) readonly uniform image2DArray	 image_peel;
#endif 

#endif 

// OUT
	layout(location = 0, index = 0) out  vec4 out_frag_color;

////////
#if sorted_by_id
	void bubbleSort		  (const ivec2 coords, const int num);
#endif
	vec4 resolveAlphaBlend(const ivec2 coords, const int num);

void main(void)
{
	int   count;
	ivec2 coords  = ivec2(gl_FragCoord.xy);

#if multisample
	count = int(imageLoad(image_counter, coords, gl_SampleID).r);
#else
	count = int(imageLoad(image_counter, coords).r);
#endif 

	if(count > 0)
	{
		vec4 c;
#if sorted_by_id
		bubbleSort(coords, count);
#endif
		if(useTransparency)  
			c = resolveAlphaBlend(coords, count);
		else
		{
			if(layer > count)
				discard;
#if multisample

#if sorted_by_id
			c  = unpackUnorm4x8(imageLoad(image_peel, ivec3(coords, layer), gl_SampleID).r).abgr;
#else
			c  = imageLoad(image_peel, ivec3(coords, layer), gl_SampleID);
#endif

#else

#if sorted_by_id
			c  = unpackUnorm4x8(imageLoad(image_peel, ivec3(coords, layer)).r).abgr;
#else
			c  = imageLoad(image_peel, ivec3(coords, layer));
#endif

#endif
		}
		out_frag_color   = c;
	}
	else
		discard;
}

vec4 resolveAlphaBlend(const ivec2 coords, const int num)
{
	vec4 Ci, finalColor = vec4(0.0f);
	
	for(int i=0; i<num; i++)
	{
#if multisample

#if sorted_by_id
			Ci  = unpackUnorm4x8(imageLoad(image_peel, ivec3(coords, i), gl_SampleID).r).abgr;
#else
			Ci  = imageLoad(image_peel, ivec3(coords, i), gl_SampleID);
#endif

#else

#if sorted_by_id
			Ci  = unpackUnorm4x8(imageLoad(image_peel, ivec3(coords, i)).r).abgr;
#else
			Ci  = imageLoad(image_peel, ivec3(coords, i));
#endif

#endif
			finalColor += Ci*(1.0f-finalColor.a);
	}
	return finalColor;
}

#if sorted_by_id

void bubbleSort(const ivec2 coords, const int num)
{
	uvec4 c0,c1;
	for (int i = (num - 2); i >= 0; --i)
	{
		for (int j = 0; j <= i; ++j)
		{
#if multisample
			c0 = imageLoad(image_peel, ivec3(coords, j  ), gl_SampleID);
			c1 = imageLoad(image_peel, ivec3(coords, j+1), gl_SampleID);
#else
			c0 = imageLoad(image_peel, ivec3(coords, j));
			c1 = imageLoad(image_peel, ivec3(coords, j+1));
#endif 	
			if (c0.g < c1.g)
			{
#if multisample
				imageStore(image_peel, ivec3(coords, j  ), gl_SampleID, c1);
				imageStore(image_peel, ivec3(coords, j+1), gl_SampleID, c0);
#else
				imageStore(image_peel, ivec3(coords, j  ), c1);
				imageStore(image_peel, ivec3(coords, j+1), c0);
#endif 	
			}
		}
	}
}

#endif