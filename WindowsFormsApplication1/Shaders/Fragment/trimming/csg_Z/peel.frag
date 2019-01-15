#include "define.h"
#extension GL_EXT_gpu_shader4 : enable

// Include
vec4 computePixelColor();
// IN
uniform int	  primitives_1;
uniform bool  all_in;

layout(binding = 0) uniform sampler2DRect tex_depth;
layout(binding = 1) uniform sampler2DRect tex_blend;
layout(binding = 4) uniform sampler2DRect tex_lock;

// OUT
layout(location = 0, index = 0) out  vec4 out_frag_depth;
layout(location = 1, index = 0) out  vec4 out_frag_color;
layout(location = 2, index = 0) out  vec4 out_frag_class;

void main(void)
{
	if(all_in && texture(tex_lock, gl_FragCoord.xy).a > 0.0f)
		discard;

	float depth   = -texture(tex_depth, gl_FragCoord.xy).r;
	ivec2 CountID = ivec2(texture(tex_blend, gl_FragCoord.xy).ra);
	
	if (gl_FragCoord.z < depth)
		discard;

	if(gl_PrimitiveID == CountID.y)
	{
		out_frag_color   = computePixelColor();
		out_frag_class.r = (gl_PrimitiveID <= primitives_1) ? 1.0f : 2.0f;
	}
	else
	{
		out_frag_color   = vec4(0.0f);
		out_frag_class.r = 0.0f;
	}

	if	   (CountID.x <= 1 && gl_FragCoord.z == depth)  out_frag_depth.r = -depth_max;
	else if(CountID.x >  1 && gl_FragCoord.z >  depth)  out_frag_depth.r = -depth_max;
	else												out_frag_depth.r = -gl_FragCoord.z;
	
}


