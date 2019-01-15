#include "define.h"
#extension GL_EXT_gpu_shader4 : enable

// INCLUDE
vec4 computePixelColor();
// IN
//uniform int bufferSize;
// OUT
#if multisample
	layout(binding = 0)					 uniform  sampler2DMS	tex_depth;
	layout(binding = 1, r32ui ) coherent uniform uimage2DMS		image_count_front;
	layout(binding = 4, r32ui ) coherent uniform uimage2DMS		image_count_back;
#if sorted_by_id
	layout(binding = 5, rg32ui) coherent uniform uimage2DMSArray image_peel_front;
	layout(binding = 6, rg32ui) coherent uniform uimage2DMSArray image_peel_back;
#else
	layout(binding = 5, rgba8 ) coherent uniform image2DMSArray  image_peel_front;
	layout(binding = 6, rgba8 ) coherent uniform image2DMSArray  image_peel_back;
#endif 
		
#else
	layout(binding = 0)					 uniform  sampler2DRect tex_depth;
	layout(binding = 1, r32ui ) coherent uniform uimage2DRect   image_count_front;
	layout(binding = 4, r32ui ) coherent uniform uimage2DRect   image_count_back;
#if sorted_by_id
	layout(binding = 5, rg32ui) coherent uniform uimage2DArray  image_peel_front;
	layout(binding = 6, rg32ui) coherent uniform uimage2DArray  image_peel_back;
#else
	layout(binding = 5, rgba8 ) coherent uniform image2DArray   image_peel_front;
	layout(binding = 6, rgba8 ) coherent uniform image2DArray   image_peel_back;
#endif 

#endif 

void main(void)
{
	ivec2 coords = ivec2(gl_FragCoord.xy);
	vec2 depth;
	
#if multisample
	depth = texelFetch(tex_depth, coords, gl_SampleID).xy;
#else
	depth = texture	  (tex_depth, gl_FragCoord.xy).xy;
#endif
	float depth_near = -depth.x;
	float depth_far	 =  depth.y;

	int   id;
	vec4  value;

	if		(gl_FragCoord.z == depth_near)
	{
		value = computePixelColor();
#if multisample
		id = (int) imageAtomicIncWrap (image_count_front, coords, gl_SampleID, FREEPIPE_SIZE); // atomicCounterIncrement
#if sorted_by_id
		imageStore(image_peel_front, ivec3(coords, id), gl_SampleID, uvec4(packUnorm4x8(value.abgr), gl_PrimitiveID, 0U, 0U));
#else
		imageStore(image_peel_front, ivec3(coords, id), gl_SampleID, value);
#endif 	
#else
		id = (int) imageAtomicIncWrap (image_count_front, coords, FREEPIPE_SIZE); // atomicCounterIncrement
#if sorted_by_id
		imageStore(image_peel_front, ivec3(coords, id), uvec4(packUnorm4x8(value.abgr), gl_PrimitiveID, 0U, 0U));
#else
		imageStore(image_peel_front, ivec3(coords, id), value);
#endif 	
#endif 
	}
	else if (gl_FragCoord.z == depth_far)
	{
		value = computePixelColor();
#if multisample
		id = (int) imageAtomicIncWrap (image_count_back, coords, gl_SampleID, FREEPIPE_SIZE); // atomicCounterIncrement
#if sorted_by_id
		imageStore(image_peel_back, ivec3(coords, id), gl_SampleID, uvec4(packUnorm4x8(value.abgr), gl_PrimitiveID, 0U, 0U));
#else
		imageStore(image_peel_back, ivec3(coords, id), gl_SampleID, value);
#endif 	
#else
		id = (int) imageAtomicIncWrap (image_count_back, coords, FREEPIPE_SIZE); // atomicCounterIncrement
#if sorted_by_id
		imageStore(image_peel_back, ivec3(coords, id), uvec4(packUnorm4x8(value.abgr), gl_PrimitiveID, 0U, 0U));
#else
		imageStore(image_peel_back, ivec3(coords, id), value);
#endif 	
#endif 
	}
	else
		discard;
}