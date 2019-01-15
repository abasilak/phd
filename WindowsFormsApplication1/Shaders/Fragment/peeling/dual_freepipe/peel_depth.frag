#include "define.h"
// IN
#if multisample
	layout(binding = 0) uniform sampler2DMS	  tex_depth;
#else
	layout(binding = 0) uniform sampler2DRect tex_depth;
#endif
// OUT
	layout(location = 0, index = 0) out vec4 out_frag_depth;

void main(void)
{
	vec2 depth;
#if multisample
	depth = texelFetch(tex_depth, ivec2(gl_FragCoord.xy), gl_SampleID).xy;
#else
	depth = texture(tex_depth, gl_FragCoord.xy).xy;
#endif
	float depth_near = -depth.x;
	float depth_far	 =  depth.y;

	if (gl_FragCoord.z <= depth_near || gl_FragCoord.z >= depth_far)
		discard;
	out_frag_depth.xy	= vec2(-gl_FragCoord.z, gl_FragCoord.z);
}