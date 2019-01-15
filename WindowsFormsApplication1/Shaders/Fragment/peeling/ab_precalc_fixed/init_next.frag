#include "define.h"

	uniform int width;

	layout(binding = 1, r32ui) writeonly uniform uimage2DRect image_next;

	void setPixelNextAddress  (uint val){ imageStore(image_next, ivec2(gl_FragCoord.xy), uvec4(val, 0U, 0U, 0U) );}

	void main(void)
	{
		setPixelNextAddress	 (uint((gl_FragCoord.x + width*gl_FragCoord.y)*ARRAY_SIZE));
	}