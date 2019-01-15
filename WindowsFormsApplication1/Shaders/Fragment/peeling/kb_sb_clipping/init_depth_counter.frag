#include "define.h"
#include "s-buffer.h"

	uniform uint *final_address[COUNTERS];

	layout(binding = 4, r32ui) coherent  uniform uimage2DArray image_count_next_sem;
	layout(binding = 5, rg32f) writeonly uniform  imageBuffer  image_peel;
	
	uint getPixelFragNextAddress(				 ) { return imageLoad	(image_count_next_sem, ivec3(gl_FragCoord.xy,1)).r;}
	void resetPixelFragCounter  (				 ) {		imageStore	(image_count_next_sem, ivec3(gl_FragCoord.xy,0), uvec4(0U) );}
	void sharedPoolResetValue	(const uint index) {		imageStore	(image_peel			 , int(index), vec4(1.0f));}

	void main(void)
	{
		uint  address = getPixelFragNextAddress();
		int   hash_id = hashFunction(ivec2(gl_FragCoord.y));
		uint  start   = (*final_address[hash_id]) + address;

		sharedPoolResetValue(start);

		resetPixelFragCounter();
	}