#include "define.h"
#extension GL_EXT_gpu_shader4 : enable
// IN
uniform int	primitives_1;

layout(binding = 0) uniform sampler2DRect tex_depth;
layout(binding = 1) uniform sampler2DRect tex_blend;
// OUT
layout(location = 0, index = 0) out vec4 out_frag_color;

void main(void)
{
	if(gl_FragCoord.z == -texture(tex_depth, gl_FragCoord.xy).r)
	{
		ivec2 CountID = ivec2(texture(tex_blend, gl_FragCoord.xy).ra);

		if(CountID.x <= 1 || CountID.y > gl_PrimitiveID)
		{
			out_frag_color.r = 1.0f;
			out_frag_color.g = (gl_PrimitiveID <= primitives_1) ? 1.0f : 0.0f;
			out_frag_color.a = float(gl_PrimitiveID);
		}
	}
	else
		discard;
}