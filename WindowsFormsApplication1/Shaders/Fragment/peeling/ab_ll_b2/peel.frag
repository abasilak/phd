#include "define.h"

// INCLUDE
	vec4 computePixelColor();
// IN
	uniform uint total_fragments;

	layout(binding = 0, r32ui) coherent uniform uimage2DRect  image_next;
	layout(binding = 1, r32ui) coherent uniform uimage2DRect  image_counter;

	layout(binding = 4, r32f ) writeonly uniform  imageBuffer image_peel_depth;
	layout(binding = 5, r32ui) writeonly uniform uimageBuffer image_peel_color;
	layout(binding = 6, r32ui) writeonly uniform uimageBuffer image_pointers;
	layout(binding = 7, offset = 0)		 uniform atomic_uint  next_address;

	void addPixelFragCounter		()				 {		  imageAtomicAdd	 (image_counter, ivec2(gl_FragCoord.xy), 1U);}
	uint exchangePixelCurrentPageID	(const uint val) { return imageAtomicExchange(image_next   , ivec2(gl_FragCoord.xy), val);}
	void sharedPoolSetDepthValue	(const uint index, const float val)	{		 imageStore(image_peel_depth, int(index),  vec4(val, 0.0f, 0.0f, 0.0f));}
	void sharedPoolSetColorValue	(const uint index, const uint  val)	{		 imageStore(image_peel_color, int(index), uvec4(val, 0U  , 0U  , 0U  ));}
	void sharedPoolSetLink			(const uint index, const uint  val)	{		 imageStore(image_pointers	, int(index), uvec4(val, 0U  , 0U  , 0U  ));}

void main(void)
{
	uint page_id = atomicCounterIncrement(next_address);

	if(page_id < total_fragments)
	{
#if trimless || trimming || collision
		float Z = (gl_FrontFacing) ? gl_FragCoord.z : -gl_FragCoord.z;
#else
		float Z = gl_FragCoord.z;
#endif
		uint  C	= packUnorm4x8(computePixelColor());

		addPixelFragCounter		();
		sharedPoolSetLink		(page_id, exchangePixelCurrentPageID(page_id));
		
		sharedPoolSetDepthValue (page_id, Z);
		sharedPoolSetColorValue (page_id, C);

		discard;
	}
}