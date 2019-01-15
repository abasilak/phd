#include "version.h"

// IN
layout(binding = 0) uniform usampler2DRect tex_copy;
// OUT
layout(location = 0, index = 0) out uvec4 out_frag;

void main(void)
{
	out_frag = texture(tex_copy, gl_FragCoord.xy);
}