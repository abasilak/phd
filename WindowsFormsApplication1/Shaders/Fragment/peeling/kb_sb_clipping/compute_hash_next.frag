#include "define.h"
#include "s-buffer.h"

	coherent uniform uint *next_address[COUNTERS];

	layout(binding = 4, r32ui) coherent uniform uimage2DArray image_count_next_sem;

	uint addSharedNextAddress	(int j,	uint val) { return	atomicAdd	(next_address[j], val);}
	void setPixelNextAddress	(	 	uint val) {			imageStore	(image_count_next_sem, ivec3(gl_FragCoord.xy,1), uvec4(val, 0U, 0U, 0U) );}
	uint getPixelFragCounter	(				) { return	imageLoad   (image_count_next_sem, ivec3(gl_FragCoord.xy,0)).x ;}

	void main(void)
	{
		uint counter = getPixelFragCounter();
		if(counter == 0U)
			discard;

		int  hash_id = hashFunction(ivec2(gl_FragCoord.y));
		uint address = addSharedNextAddress (hash_id, counter);
		setPixelNextAddress (address);
	}