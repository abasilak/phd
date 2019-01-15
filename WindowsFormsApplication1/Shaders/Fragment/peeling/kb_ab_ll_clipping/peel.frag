#include "define.h"
#include "data_structs.h"

#if random_discard
#include "noise.h"
	uniform int	 width;
	flat in int instanceID;
#endif 

#if early_Z
	layout (early_fragment_tests) in;
#endif

	vec4 computePixelColor();

	layout(binding = 4, std430)		coherent buffer  LinkedLists   { NodeTypeLL nodes[]; };
	layout(binding = 5, r32ui)		coherent uniform uimage2DRect  image_next;
	layout(binding = 6, offset = 0)			 uniform atomic_uint   next_address;

	uint exchangePixelCurrentPageID	(const uint val	) { return imageAtomicExchange(image_next, ivec2(gl_FragCoord.xy), val);}

	void main(void)
	{
#if random_discard
		if(distribution(int(gl_FragCoord.x) + width*(int(gl_FragCoord.y) + instanceID)))
			discard;
#endif
		uint page_id = atomicCounterIncrement(next_address) + 1U;
		uint C = (packUnorm4x8(computePixelColor()));
		//uint C = (packUnorm4x8(vec4(1.0f)));

		nodes[page_id].color = C;
		nodes[page_id].depth = gl_FragCoord.z;
		nodes[page_id].next  = exchangePixelCurrentPageID(page_id);
	}