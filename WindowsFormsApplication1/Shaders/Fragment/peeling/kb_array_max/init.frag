#include "define.h"

	layout(binding = 4, rg32f) writeonly uniform  image2DArray image_peel;
	//layout(binding = 6, r32ui) writeonly uniform uimage2DRect  image_skata;

	//void  resetPixelFragSkata	 (){ imageStore (image_skata  , ivec2(gl_FragCoord.xy), uvec4(0U));}
	void  resetPixelFragDepth	 (){ imageStore (image_peel, ivec3(gl_FragCoord.xy, 0), vec4(1.0f));}

	void main(void)
	{
		resetPixelFragDepth	   ();
		//resetPixelFragSkata  ();
	}