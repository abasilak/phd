#include "define.h"
#include "s-buffer.h"



	layout(binding = 0, r32ui ) coherent uniform  uimage2DArray  image_count_countT_next_sem;
	layout(binding = 5, std430)	coherent  buffer  final_address  { uint final[]; };

	uint getPixelFragCounterTotal	(				) { return	imageLoad   (image_count_countT_next_sem, ivec3(gl_FragCoord.xy,1)).x ;}

	void setPixelNextAddress		(	 	uint val) {			imageStore	(image_count_countT_next_sem, ivec3(gl_FragCoord.xy,2), uvec4(val, 0U, 0U, 0U) );}
	uint addSharedNextAddress		(int j,	uint val) { return	atomicAdd	(final[j], val);}

	void main(void)
	{
		uint counter = getPixelFragCounterTotal();
		if(counter == 0U)
			discard;

		int  hash_id = hashFunction(ivec2(gl_FragCoord.xy));
		uint address = addSharedNextAddress (hash_id, counter);
		setPixelNextAddress (address);
	}