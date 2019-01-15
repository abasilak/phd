#include "define.h"

#if multisample
	layout(binding = 0) uniform usampler2DMS   tex_lock_overlap;
#else
	layout(binding = 0) uniform usampler2DRect tex_lock_overlap;
#endif

void main(void)
{
	uvec4 L_O;
#if multisample
	L_O = texelFetch(tex_lock_overlap, ivec2(gl_FragCoord.xy), gl_SampleID);
#else
	L_O = texture	(tex_lock_overlap, gl_FragCoord.xy);
#endif
	
	if(L_O.r == 1U && L_O.b == 1U)
		discard;
}
