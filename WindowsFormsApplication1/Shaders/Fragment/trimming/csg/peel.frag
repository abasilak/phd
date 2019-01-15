#include "define.h"
#extension GL_EXT_gpu_shader4 : enable

// Include
vec4 computePixelColor();
// IN
uniform bool  first;
uniform bool  all_in;
uniform int	  primitives_1;

uniform float cappingPlane;
uniform float cappingAngle;

layout(binding = 0) uniform sampler2DRect tex_depth;
layout(binding = 1) uniform sampler2DRect tex_lock;

// OUT
layout(location = 0, index = 0) out  vec4 out_frag_color;
layout(location = 1, index = 0) out uvec4 out_frag_class;

void main(void)
{
	float depth;
	if(first)
	{
		float k = 1.0f - float(gl_FragCoord.x)/cappingAngle;
		depth   = cappingPlane*k;
	}
	else
	{
		if(all_in && texture(tex_lock, gl_FragCoord.xy).a > 0.0f)
			discard;
		depth = texture(tex_depth, gl_FragCoord.xy).r;
	}
	if(gl_FragCoord.z <= depth)
		discard;

	out_frag_color   = computePixelColor();	
	out_frag_class.r = (gl_PrimitiveID <= primitives_1) ? 1U : 2U;
}