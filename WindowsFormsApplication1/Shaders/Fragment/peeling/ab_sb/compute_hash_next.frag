#include "define.h"
#include "s-buffer.h"

	layout(binding = 0, r32ui)  readonly  uniform uimage2DRect  image_counter;
	layout(binding = 1, r32ui)  writeonly uniform uimage2DRect  image_next;
	layout(binding = 5, std430)	coherent  buffer  final_address  { uint final[]; };

	void setPixelNextAddress  (			uint val) {			imageStore	(image_next	  , ivec2(gl_FragCoord.xy), uvec4(val, 0U, 0U, 0U) );}
	uint getPixelFragCounter  (					) { return	imageLoad   (image_counter, ivec2(gl_FragCoord.xy)).x ;}
	
	uint addSharedNextAddress (int j,	uint val) { return	atomicAdd	(final[j], val);}
		
	void main(void)
	{
		uint counter = getPixelFragCounter();
		if(counter == 0U)
			discard;

		int  hash_id = hashFunction(ivec2(gl_FragCoord.xy));
		uint address = addSharedNextAddress (hash_id, counter);
		setPixelNextAddress (address);
	}