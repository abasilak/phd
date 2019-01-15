#include "define.h"

#if multisample
	uniform sampler2DMS	  tex_depth;
#else
	uniform sampler2DRect tex_depth;
#endif

void main(void)
{
	float depth;
#if multisample
	depth = texelFetch(tex_depth, ivec2(gl_FragCoord.xy), gl_SampleID).r;
#else
	depth = texture	  (tex_depth, gl_FragCoord.xy).r;
#endif
	if(gl_FragCoord.z <= depth)
		discard;
}