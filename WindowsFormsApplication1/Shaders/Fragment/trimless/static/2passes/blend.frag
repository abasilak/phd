#include "define.h"

// IN
uniform float cappingPlane;
uniform float cappingAngle;
// OUT
layout(location = 0, index = 0) out vec4 out_frag_lock_overlap;

void main(void)
{
	float k = 1.0f - float(gl_FragCoord.x)/cappingAngle;
	float capping = cappingPlane*k;

	out_frag_lock_overlap.r = 0.0f;
	if(gl_FragCoord.z <= capping)
		out_frag_lock_overlap.r = gl_FrontFacing ? 1.0f : -1.0f;
	else
		discard;
}
