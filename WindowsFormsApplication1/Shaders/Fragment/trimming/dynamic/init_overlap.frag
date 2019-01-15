#include "define.h"

// IN
flat in uint classification;

layout(binding = 0) uniform sampler2DRect tex_depth;
layout(binding = 1) uniform sampler2DRect tex_depth_old;
// OUT
layout(location = 0, index = 0) out vec4 out_frag_overlap;
layout(location = 1, index = 0) out vec4 out_frag_layers_old;

void main(void)
{
	float facing = gl_FrontFacing ? 1.0f : -1.0f;
	// New
	out_frag_overlap= vec4(0.0f);
	float depth		= texture(tex_depth    , gl_FragCoord.xy).r;
	if(classification > 0U)
	{
		if(gl_FragCoord.z <  depth) out_frag_overlap.r = facing;
		if(gl_FragCoord.z <= depth) out_frag_overlap.g = facing;
	}
	
	// Old
	out_frag_layers_old.r = 0.0f;
	if(classification < 2U)
	{
		float depth_old = texture(tex_depth_old, gl_FragCoord.xy).r;
		float d1_old = depth_old;
		if(depth > depth_old) depth_old = depth;

		if(gl_FragCoord.z <  depth_old) out_frag_overlap.b = facing;
		if(gl_FragCoord.z <= depth_old) out_frag_overlap.a = facing;	
		
		out_frag_layers_old.r = (gl_FragCoord.z <= depth_old && gl_FragCoord.z >= d1_old) ? 1.0f : 0.0f; // Number of old layers between d1_old and d1
	}
}