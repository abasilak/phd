#include "define.h"

	layout(binding = 1, r32ui) writeonly uniform uimage2DArray image_histogram;

	void  resetPixelFragHistogramDepth(const int coord_z) { imageStore (image_histogram, ivec3(gl_FragCoord.xy, coord_z), uvec4(0U));}

	void main(void)
	{
		for(int i=0; i<BUCKET_SIZE_32d; i++)
			resetPixelFragHistogramDepth(i);
	}