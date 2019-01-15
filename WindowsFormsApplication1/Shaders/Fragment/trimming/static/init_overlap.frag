#include "define.h"

// IN
#if multisample
	layout(binding = 0) uniform sampler2DMS   tex_depth;
#else
	layout(binding = 0) uniform sampler2DRect tex_depth;
#endif

// OUT
layout(location = 0, index = 0) out vec4 out_frag_overlap;

void main(void)
{
	float facing = gl_FrontFacing ? 1.0f : -1.0f;
	float depth;
#if multisample
	depth = texelFetch(tex_depth, ivec2(gl_FragCoord.xy), gl_SampleID).r;
#else
	depth = texture(tex_depth, gl_FragCoord.xy).r;
#endif

	if(gl_FragCoord.z <  depth) out_frag_overlap.r = facing;
	if(gl_FragCoord.z <= depth) out_frag_overlap.b = facing;
}