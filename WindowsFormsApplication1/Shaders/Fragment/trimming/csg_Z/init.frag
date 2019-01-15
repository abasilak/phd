#include "define.h"

// IN
uniform float cappingPlane;
uniform float cappingAngle;
// OUT
layout(location = 0, index = 0) out vec4 out_frag_depth;

void main(void)
{
	float k = 1.0f - float(gl_FragCoord.x)/cappingAngle;
	float depth = cappingPlane*k;
	if(gl_FragCoord.z <= depth)
		discard;
	out_frag_depth.r = -gl_FragCoord.z;
}