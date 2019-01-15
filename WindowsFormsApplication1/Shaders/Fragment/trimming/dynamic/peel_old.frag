#include "define.h"

// IN
flat in uint classification;

uniform float cappingPlane;
uniform float cappingAngle;

void main(void)
{
	if(classification == 2U)
		discard;

	float k = 1.0f - float(gl_FragCoord.x)/cappingAngle;
	if(gl_FragCoord.z <= cappingPlane*k)
		discard;
}