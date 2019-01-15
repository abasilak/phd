#include "define.h"

	layout(binding = 0, r32ui) writeonly uniform uimage2DArray image_min_max_sem;
	layout(binding = 4, rg32f) writeonly uniform  image2DArray image_peel;

	void  resetPixelFragMinDepth  (					) { imageStore (image_min_max_sem, ivec3(gl_FragCoord.xy, 0), uvec4(Packed_1f));}
	void  resetPixelFragMaxnDepth (					) { imageStore (image_min_max_sem, ivec3(gl_FragCoord.xy, 1), uvec4(0U));}
#if !(INTEL_ordering | NV_interlock)
	void  resetPixelFragSemaphore (					) { imageStore (image_min_max_sem, ivec3(gl_FragCoord.xy, 2), uvec4(0U));}
#endif
	void  resetPixelFragDepthValue(const int coord_z) { imageStore (image_peel		 , ivec3(gl_FragCoord.xy, coord_z),  vec4(1.0f));}

	void main(void)
	{
		resetPixelFragMinDepth ();
		resetPixelFragMaxnDepth();
#if !(INTEL_ordering | NV_interlock)
		resetPixelFragSemaphore();
#endif
		for(int i=0; i<HEAP_SIZE; i++)
			resetPixelFragDepthValue(i);
	}