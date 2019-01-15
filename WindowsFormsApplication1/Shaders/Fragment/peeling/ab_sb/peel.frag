#include "define.h"
#include "s-buffer.h"
#include "data_structs.h"

	vec4 computePixelColor();

	layout(binding = 1, r32ui)  coherent  uniform  uimage2DRect		image_next;
	layout(binding = 4, std430) coherent  buffer   SBuffer		  { NodeTypeSB nodes[]; };
	layout(binding = 5, std430)	readonly  buffer   final_address  { uint final[]; };
	
	uint addPixelNextAddress  (									 )	{ return imageAtomicAdd	(image_next, ivec2(gl_FragCoord.xy), 1U);}

	void main(void)
	{
		uint C = packUnorm4x8(computePixelColor());
		//uint C = packUnorm4x8(vec4(1.0f));

		uint  page_id = addPixelNextAddress();
		int   hash_id = hashFunction(ivec2(gl_FragCoord.xy))+COUNTERS;
		uint  sum     = final[hash_id] + page_id;

#if inverse
		uint  index = hash_id < COUNTERS_2d ? sum : nodes.length() + 1U - sum;
#else
		uint  index = sum;
#endif
		nodes[index].color = C;
		nodes[index].depth = gl_FragCoord.z;
	}