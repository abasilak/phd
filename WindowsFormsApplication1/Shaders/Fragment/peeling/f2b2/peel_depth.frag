#include "define.h"

// IN
#if multisample
	layout(binding = 0) uniform sampler2DMS	  tex_depth;
#else
	layout(binding = 0) uniform sampler2DRect tex_depth;
#endif

#if early_Z
	layout (depth_greater) out float gl_FragDepth;
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