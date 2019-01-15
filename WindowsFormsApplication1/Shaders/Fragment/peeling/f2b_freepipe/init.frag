#include "define.h"

#if multisample
	layout(binding = 4, r32ui) writeonly uniform uimage2DMS   image_counter;
#else
	layout(binding = 4, r32ui) writeonly uniform uimage2DRect image_counter;
#endif 

void main(void)
{
#if multisample
	imageStore(image_counter, ivec2(gl_FragCoord.xy), gl_SampleID, uvec4(0));
#else
	imageStore(image_counter, ivec2(gl_FragCoord.xy), uvec4(0));
#endif 
}
