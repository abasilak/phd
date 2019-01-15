#include "version.h"

	vec4 computePixelColor();

	uniform float cappingPlane;
	uniform float cappingAngle;

	layout(location = 0, index = 0) out vec4 out_frag_color;

	void main(void)
	{
		float k = 1.0f - float(gl_FragCoord.x)/cappingAngle;
		float capping  = cappingPlane*k;

		if(gl_FragCoord.z > capping)
			discard;
		out_frag_color = computePixelColor();
	}