#include "version.h"

// OUT
layout(location = 0, index = 0) out vec4 out_frag_color;

void main(void)
{
	out_frag_color.r =  (gl_FrontFacing) ? gl_FragCoord.z : -gl_FragCoord.z;
}