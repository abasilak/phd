#include "define.h"

#if multisample
	layout(binding = 0) uniform usampler2DMS   tex_lock_overlap;
#else
	layout(binding = 0) uniform usampler2DRect tex_lock_overlap;
#endif

void main(void)
{
	uint lock;
#if multisample
	lock = texelFetch(tex_lock_overlap, ivec2(gl_FragCoord.xy), gl_SampleID).r;
#else
	lock = texture	 (tex_lock_overlap, gl_FragCoord.xy).r;
#endif
	if(lock == 1U) 
		discard;
}
