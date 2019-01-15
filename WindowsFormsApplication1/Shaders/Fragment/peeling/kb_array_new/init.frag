#include "define.h"

	layout(binding = 0, r32ui) writeonly uniform uimage2DRect  image_counter;
	layout(binding = 1, r32ui) writeonly uniform uimage2DRect  image_semaphore;
	layout(binding = 4, r32f ) writeonly uniform  image2DArray image_peel_depth;

	void  resetPixelFragCounter	 (){ imageStore (image_counter   , ivec2(gl_FragCoord.xy), uvec4(0U));}
	void  resetPixelFragSemaphore(){ imageStore (image_semaphore , ivec2(gl_FragCoord.xy), uvec4(0U));}
	void  resetPixelFragDepth	 (){ imageStore (image_peel_depth, ivec3(gl_FragCoord.xy, 0), vec4(1.0f));}

	void main(void)
	{
		resetPixelFragDepth	   ();
		resetPixelFragCounter  ();
		resetPixelFragSemaphore();
	}