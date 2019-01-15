#include "define.h"

	layout(binding = 0, r32ui) writeonly uniform uimage2DArray  image_count_sem_min_max;

	void  resetPixelFragCounter   (){ imageStore (image_count_sem_min_max, ivec3(gl_FragCoord.xy, 0), uvec4(0U));}
	void  resetPixelFragSemaphore (){ imageStore (image_count_sem_min_max, ivec3(gl_FragCoord.xy, 1), uvec4(0U));}
	void  resetPixelFragMinDepth  (){ imageStore (image_count_sem_min_max, ivec3(gl_FragCoord.xy, 2), uvec4(Packed_1f));}
	void  resetPixelFragMaxDepth  (){ imageStore (image_count_sem_min_max, ivec3(gl_FragCoord.xy, 3), uvec4(0U));}

	void main(void)
	{
		resetPixelFragCounter  ();
		resetPixelFragSemaphore();
		resetPixelFragMinDepth ();
		resetPixelFragMaxDepth ();
	}