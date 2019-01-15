#include "define.h"

	vec4 computePixelColor();

	layout(binding = 0, r32ui)		coherent	uniform uimage2DRect  image_next;
	layout(binding = 1, r32ui)		coherent	uniform uimage2DRect  image_counter;
	layout(binding = 4, rgba32f)	writeonly	uniform  imageBuffer  image_peel_pointers;
	layout(binding = 5, offset = 0)				uniform atomic_uint   next_address;

	void addPixelFragCounter		(				)					{		 imageAtomicAdd		(image_counter, ivec2(gl_FragCoord.xy), 1U);}
	uint exchangePixelCurrentPageID	(const uint val	)					{ return imageAtomicExchange(image_next   , ivec2(gl_FragCoord.xy), val);}

	void sharedPoolSetValue			(const uint index, const vec4 val)	{		 imageStore			(image_peel_pointers, int(index), val);}

	void main(void)
	{
		uint  page_id = atomicCounterIncrement(next_address) + 1U;
		if(page_id < imageSize(image_peel_pointers))
		{
			vec4  value = vec4(uintBitsToFloat(packUnorm4x8(computePixelColor())), gl_FragCoord.z, uintBitsToFloat(exchangePixelCurrentPageID(page_id)), 0.0f);

			addPixelFragCounter();
			sharedPoolSetValue (page_id, value);

			discard;
		}
	}