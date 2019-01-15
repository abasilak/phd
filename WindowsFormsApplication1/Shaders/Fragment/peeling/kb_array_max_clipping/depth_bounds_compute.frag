#include "version.h"

	layout(binding = 0, r32ui) coherent uniform uimage2DArray image_count_sem_min_max;

	void  setPixelFragMinDepth (uint Z) { imageAtomicMin (image_count_sem_min_max, ivec3(gl_FragCoord.xy, 2), Z); }
	void  setPixelFragMaxDepth (uint Z) { imageAtomicMax (image_count_sem_min_max, ivec3(gl_FragCoord.xy, 3), Z); }

	void main(void)
	{
		uint Z = floatBitsToUint(gl_FragCoord.z);
		setPixelFragMinDepth (Z);
		setPixelFragMaxDepth (Z);
	}