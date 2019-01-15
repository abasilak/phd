#include "define.h"
#extension GL_EXT_gpu_shader4 : enable

// INCLUDE
vec4 computePixelColor();
// IN
#if multisample
	layout(binding = 4, r32ui ) coherent uniform uimage2DMS		 image_counter;
#if sorted_by_id
	layout(binding = 3, rg32ui) coherent uniform uimage2DMSArray image_peel;
#else
	layout(binding = 3, rgba8 ) coherent uniform image2DMSArray  image_peel;
#endif 
		
#else
	layout(binding = 4, r32ui ) coherent uniform uimage2DRect  image_counter;
#if sorted_by_id
	layout(binding = 3, rg32ui) coherent uniform uimage2DArray image_peel;
#else
	layout(binding = 3, rgba8 ) coherent uniform image2DArray  image_peel;
#endif 

#endif 

#if early_Z
	layout (early_fragment_tests) in;
#endif

void main(void)
{
	int   id;
	ivec2 coords = ivec2(gl_FragCoord.xy);
	vec4  value = computePixelColor();

#if multisample
	id = (int) imageAtomicIncWrap (image_counter, coords, gl_SampleID, FREEPIPE_SIZE); // atomicCounterIncrement
#if sorted_by_id
	imageStore(image_peel, ivec3(coords, id), gl_SampleID, uvec4(packUnorm4x8(value.abgr), gl_PrimitiveID, 0U, 0U));
#else
	imageStore(image_peel, ivec3(coords, id), gl_SampleID, value);
#endif 	

#else
	id = (int) imageAtomicIncWrap (image_counter, coords, FREEPIPE_SIZE); // atomicCounterIncrement
#if sorted_by_id
	imageStore(image_peel, ivec3(coords, id), uvec4(packUnorm4x8(value.abgr), gl_PrimitiveID, 0U, 0U));
#else
	imageStore(image_peel, ivec3(coords, id), value);
#endif 	

#endif 
}