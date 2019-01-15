#include "define.h"

// IN
layout(binding = 0) uniform  sampler2DRect tex_color;
layout(binding = 1) uniform usampler2DRect tex_class;
layout(binding = 2) uniform usampler2DRect tex_rule;
layout(binding = 3) uniform  sampler2DRect tex_final_color;
layout(binding = 4) uniform usampler2DRect tex_layers;

// OUT
layout(location = 0, index = 0) out vec4 out_frag_color;

void main(void)
{
	vec4  blender_color = texture(tex_final_color, gl_FragCoord.xy);
	if(blender_color.a == 0.0f && texture(tex_class, gl_FragCoord.xy).g > 0U)
	{
		uint  new_layers = texture(tex_layers, gl_FragCoord.xy).g;
		uvec4 rule		 = texture(tex_rule , gl_FragCoord.xy);

		uint R1 = rule.a & 1U;
		uint R2 = (rule.a >> new_layers) & 1U;
		if(R1 != R2) 
		{
			vec4  c, color =  texture(tex_color , gl_FragCoord.xy);
			c.rgb = (R1==1U) ? vec3(0.0,0.0,1.0) : vec3(1.0,1.0,0.0); // Blue - Yellow	
			blender_color = vec4(c.rgb * color.rgb, color.a);
		}
	}
	out_frag_color = blender_color;
}
