#include "define.h"

// IN
#if multisample
	layout(binding = 0) uniform sampler2DMS	  tex_depth;
#else
	layout(binding = 0) uniform sampler2DRect tex_depth;
#endif

void main(void)
{
	vec2 depth;
#if multisample
	depth = texelFetch(tex_depth, ivec2(gl_FragCoord.xy), gl_SampleID).rg;
#else
	depth = texture(tex_depth, gl_FragCoord.xy).rg;
#endif
	float depth_near = -depth.x;
	float depth_far	 =  depth.y;

	if (gl_FragCoord.z < depth_near || gl_FragCoord.z > depth_far)
		discard;
}