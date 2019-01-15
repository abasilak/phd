#include "define.h"

	layout(binding = 0, r32ui) writeonly uniform uimage2DArray image_min_max;
	layout(binding = 4, r32ui) writeonly uniform uimage2DArray image_peel_depth;

	void  resetPixelFragMinDepth  (					) { imageStore (image_min_max, ivec3(gl_FragCoord.xy, 0), uvec4(Packed_1f));}
	void  resetPixelFragMaxnDepth (					) { imageStore (image_min_max, ivec3(gl_FragCoord.xy, 1), uvec4(0U));}
	void  resetPixelFragDepthValue(const int coord_z, const uvec4 val){ imageStore (image_peel_depth, ivec3(gl_FragCoord.xy, coord_z), val);}

	void main(void)
	{
		resetPixelFragMinDepth ();
		resetPixelFragMaxnDepth();
		for(int i=0; i<HEAP_SIZE; i++)
			resetPixelFragDepthValue(i, uvec4(0xFFFFFFFFU,0U,0U,0U));
	}