#include "define.h"

	layout(binding = 0, r32f) writeonly uniform  image2DArray image_peel_depth;

	void  resetPixelFragDepth	 (const int coord_z	){ imageStore (image_peel_depth, ivec3(gl_FragCoord.xy, coord_z), vec4(1.0f));}

	void main(void)
	{
		for(int i=0; i<HEAP_SIZE; i++)
			resetPixelFragDepth(i);
	}