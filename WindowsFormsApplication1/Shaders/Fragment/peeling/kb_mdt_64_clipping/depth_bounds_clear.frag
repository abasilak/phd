#include "define.h"

	uniform int width;

	layout(binding = 0, r32ui ) writeonly uniform uimage2DArray image_min_max;
	layout(binding = 4, std430)	writeonly buffer  myKbuffer   { uint64_t nodes[]; };

	void  resetPixelFragMinDepth  (					) { imageStore (image_min_max, ivec3(gl_FragCoord.xy, 0), uvec4(Packed_1f));}
	void  resetPixelFragMaxnDepth (					) { imageStore (image_min_max, ivec3(gl_FragCoord.xy, 1), uvec4(0U));}

	void main(void)
	{
		resetPixelFragMinDepth ();
		resetPixelFragMaxnDepth();

		int index = (int(gl_FragCoord.x) + width*int(gl_FragCoord.y))*HEAP_SIZE;

		uint64_t zero64 = uint64_t(0xFFFFFFFF) << 32;
		for(int i=0; i<HEAP_SIZE; i++)
			nodes[index + i] = zero64;
	}