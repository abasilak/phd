#include "define.h"

#if multisample
	layout(binding = 1, r32ui) writeonly uniform uimage2DMS   image_count_front;
	layout(binding = 4, r32ui) writeonly uniform uimage2DMS   image_count_back;
#else
	layout(binding = 1, r32ui) writeonly uniform uimage2DRect image_count_front;
	layout(binding = 4, r32ui) writeonly uniform uimage2DRect image_count_back;
#endif 

void main(void)
{
#if multisample
	imageStore(image_count_front, ivec2(gl_FragCoord.xy), gl_SampleID, uvec4(0));
	imageStore(image_count_back , ivec2(gl_FragCoord.xy), gl_SampleID, uvec4(0));
#else
	imageStore(image_count_front, ivec2(gl_FragCoord.xy), uvec4(0));
	imageStore(image_count_back , ivec2(gl_FragCoord.xy), uvec4(0));
#endif 
}
