#include "define.h"
#include "noise.h"

// INCLUDE
vec4 computePixelColor();

// IN
uniform int   width;
uniform float discardThreshold;

uniform float cappingPlane;
uniform float cappingAngle;

flat in int instanceID;

// OUT
layout(location = 0) out vec4 out_frag_color;

#if early_Z
	layout (depth_greater) out float gl_FragDepth;
#endif

void main(void)
{
	float k = 1.0f - float(gl_FragCoord.x)/cappingAngle;
	float capping = cappingPlane*k;

	if(gl_FragCoord.z <= capping)
		discard;

#if random_discard
	if(distribution(int(gl_FragCoord.x) + width*(int(gl_FragCoord.y) + instanceID), discardThreshold))
		discard;
#endif

	vec4 c = computePixelColor();
	if(cappingPlane > 0.0f)
		c *= vec4(0,0,1,1); // Blue Color

	out_frag_color   = c;
}