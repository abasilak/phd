#include "define.h"
#extension GL_EXT_gpu_shader4 : enable

// INCLUDE
	vec4 computePixelColor();
// IN
	
	layout(binding = 1, offset = 0)		 uniform  atomic_uint next_address;
#if sorted_by_id
	layout(binding = 4, rg32f) writeonly uniform  imageBuffer image_peel;
#else
	layout(binding = 4, rgba8) writeonly uniform  imageBuffer image_peel;
#endif
	layout(binding = 5, r32ui) writeonly uniform uimageBuffer image_pointers;
#if multisample
	layout(binding = 6, r32ui) coherent  uniform uimage2DMS   image_page_id;
#if sorted_by_id
	layout(binding = 7, r32ui) coherent  uniform uimage2DMS   image_counter;
#endif
#else
	layout(binding = 6, r32ui) coherent  uniform uimage2DRect image_page_id;
#if sorted_by_id
	layout(binding = 7, r32ui) coherent  uniform uimage2DRect image_counter;
#endif
#endif 

#if multisample
	uint exchangePixelCurrentPageID(ivec2 coords, uint val)	{ return imageAtomicExchange(image_page_id, coords, gl_SampleID, val);}
#if sorted_by_id
	void addPixelFragCounter(ivec2 coords, uint val)		{		 imageAtomicAdd		(image_counter, coords, gl_SampleID, val);}
#endif
#else
	uint exchangePixelCurrentPageID(ivec2 coords, uint val)	{ return imageAtomicExchange(image_page_id, coords, val);}
#if sorted_by_id	
	void addPixelFragCounter(ivec2 coords, uint val)		{		 imageAtomicAdd		(image_counter, coords, val);}
#endif
#endif 
	void sharedPoolSetValue(uint index, vec4 val)			{ imageStore(image_peel	   , int(index), val);}
	void sharedPoolSetLink (uint id   , uint val)			{ imageStore(image_pointers, int(id)   , uvec4(val, 0U, 0U, 0U) );}

#if early_Z
	layout (early_fragment_tests) in;
#endif

void main(void)
{
	ivec2 coords = ivec2(gl_FragCoord.xy);

	vec4  value;
#if sorted_by_id
	value = vec4(packUnorm4x8(computePixelColor().abgr), gl_PrimitiveID, 0.0f, 0.0f);
	addPixelFragCounter(coords, 1U);
#else
	value = computePixelColor();
#endif

	uint  page_id = atomicCounterIncrement(next_address);
	sharedPoolSetLink (page_id, exchangePixelCurrentPageID(coords, page_id));
	sharedPoolSetValue(page_id, value);
}