#include "define.h"
#include "s-buffer.h"

#if random_discard
#include "noise.h"
	uniform int	width;
	flat in int instanceID;
#endif

	layout(binding = 0, r32ui) coherent uniform uimage2DRect image_counter;
	
	void incPixelFragCounter() {imageAtomicAdd(image_counter, ivec2(gl_FragCoord.xy), 1U);}

	void main(void)
	{
#if random_discard
		if(distribution(int(gl_FragCoord.x) + width*(int(gl_FragCoord.y) + instanceID)))
			discard;
#endif
		incPixelFragCounter();
	}

