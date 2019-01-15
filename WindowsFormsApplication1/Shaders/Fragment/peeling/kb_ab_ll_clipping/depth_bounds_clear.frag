#include "define.h"

	layout(binding = 0, r32ui) writeonly uniform uimage2DArray image_min_max;

	void  resetPixelFragMinDepth  (					) { imageStore (image_min_max, ivec3(gl_FragCoord.xy, 0), uvec4(Packed_1f));}
	void  resetPixelFragMaxnDepth (					) { imageStore (image_min_max, ivec3(gl_FragCoord.xy, 1), uvec4(0U));}

	void main(void)
	{
		resetPixelFragMinDepth ();
		resetPixelFragMaxnDepth();
	}