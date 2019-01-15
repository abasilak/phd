#include "define.h"

// IN
#if multisample
	layout(binding = 0) uniform sampler2DMS   tex_color;
	layout(binding = 1) uniform sampler2DMS   tex_overlap_rule;
	layout(binding = 2) uniform sampler2DMS   tex_final_color;
#else
	layout(binding = 0) uniform sampler2DRect tex_color;
	layout(binding = 1) uniform sampler2DRect tex_overlap_rule;
	layout(binding = 2) uniform sampler2DRect tex_final_color;
#endif

// OUT
layout(location = 0, index = 0) out vec4 out_frag_color;

void main(void)
{
	vec4  blender_color = texture(tex_final_color, gl_FragCoord.xy);
	bvec2 rule		    = bvec2(texture(tex_overlap_rule , gl_FragCoord.xy).ga);

	if(rule.x != rule.y && blender_color.a == 0.0f)
	{
		vec4  c, color =  texture(tex_color , gl_FragCoord.xy);
		c.rgb = (rule.x) ? vec3(0.0,0.0,1.0) : vec3(1.0,1.0,0.0);
		blender_color = vec4(c.rgb * color.rgb, color.a);
	}
	out_frag_color = blender_color;
}
