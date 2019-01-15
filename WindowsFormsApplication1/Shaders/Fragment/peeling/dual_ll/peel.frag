#include "define.h"
#extension GL_EXT_gpu_shader4 : enable

// INCLUDE
	vec4 computePixelColor();
// IN
	uniform uint  total_fragments;

#if multisample
	layout(binding = 10)				uniform sampler2DMS	 tex_depth;
	layout(binding = 2, r32ui) coherent uniform uimage2DMS   image_page_id_front;
	layout(binding = 3, r32ui) coherent uniform uimage2DMS   image_page_id_back;
#if sorted_by_id	
	layout(binding = 4, r32ui) coherent uniform uimage2DMS   image_counter_front;
	layout(binding = 5, r32ui) coherent uniform uimage2DMS   image_counter_back;
#endif
#else
	layout(binding = 10)				uniform sampler2DRect tex_depth;
	layout(binding = 2, r32ui) coherent uniform uimage2DRect  image_page_id_front;
	layout(binding = 3, r32ui) coherent uniform uimage2DRect  image_page_id_back;
#if sorted_by_id	
	layout(binding = 4, r32ui) coherent uniform uimage2DRect  image_counter_front;
	layout(binding = 5, r32ui) coherent uniform uimage2DRect  image_counter_back;
#endif
#endif
 
#if sorted_by_id
	layout(binding = 6, rg32f) writeonly uniform  imageBuffer image_peel;
#else
	layout(binding = 6, rgba8) writeonly uniform  imageBuffer image_peel;
#endif
	layout(binding = 7, r32ui) writeonly uniform uimageBuffer image_pointers;
	layout(binding = 0, offset = 0)		 uniform atomic_uint  next_address_front;
	layout(binding = 1, offset = 0)		 uniform atomic_uint  next_address_back;

#if multisample
	uint exchangePixelCurrentPageID_front(ivec2 coords, uint val)	{ return imageAtomicExchange(image_page_id_front	, coords, gl_SampleID, val);}
	uint exchangePixelCurrentPageID_back(ivec2 coords, uint val)	{ return imageAtomicExchange(image_page_id_back		, coords, gl_SampleID, val);}
#if sorted_by_id
	void addPixelFragCounter_front	(ivec2 coords, uint val)		{		 imageAtomicAdd		(image_counter_front	, coords, gl_SampleID, val);}
	void addPixelFragCounter_back(ivec2 coords, uint val)			{		 imageAtomicAdd		(image_counter_back		, coords, gl_SampleID, val);}
#endif
#else
	uint exchangePixelCurrentPageID_front(ivec2 coords, uint val)	{ return imageAtomicExchange(image_page_id_front	, coords, val);}
	uint exchangePixelCurrentPageID_back(ivec2 coords, uint val)	{ return imageAtomicExchange(image_page_id_back		, coords, val);}
#if sorted_by_id
	void addPixelFragCounter_front(ivec2 coords, uint val)			{		 imageAtomicAdd		(image_counter_front	, coords, val);}
	void addPixelFragCounter_back(ivec2 coords, uint val)			{		 imageAtomicAdd		(image_counter_back		, coords, val);}
#endif
#endif 
	void sharedPoolSetValue	(uint index, vec4 val)					{ imageStore(image_peel	   , int(index), val);}
	void sharedPoolSetLink	(uint id   , uint val)					{ imageStore(image_pointers, int(id)   , uvec4(val, 0U, 0U, 0U) );}

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

	if (gl_FragCoord.z != depth_near && gl_FragCoord.z != depth_far)
		discard;

	vec4 value;
#if sorted_by_id
	value = vec4(packUnorm4x8(computePixelColor().abgr), gl_PrimitiveID, 0.0f, 0.0f);

	if (gl_FragCoord.z == depth_near) addPixelFragCounter_front(coords, 1U);
	else						      addPixelFragCounter_back (coords, 1U);
#else
	value = computePixelColor();
#endif
	uint page_id, next_id;
	if (gl_FragCoord.z == depth_near)
	{
		page_id = atomicCounterIncrement(next_address_front);
		next_id = exchangePixelCurrentPageID_front(coords, page_id);
	}
	else
	{
		page_id = atomicCounterIncrement(next_address_back);
		next_id = exchangePixelCurrentPageID_back(coords, page_id);

		page_id = total_fragments - page_id;
		next_id = total_fragments - next_id;
	}

	sharedPoolSetLink (page_id, next_id);
	sharedPoolSetValue(page_id, value  ); 
}