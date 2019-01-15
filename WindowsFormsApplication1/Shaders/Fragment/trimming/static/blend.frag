#include "define.h"

// IN
uniform bool first;

#if multisample
	layout(binding = 0) uniform  sampler2DMS   tex_overlap_rule;
	layout(binding = 1) uniform usampler2DMS   tex_facing;
#else
	layout(binding = 0) uniform  sampler2DRect tex_overlap_rule;
	layout(binding = 1) uniform usampler2DRect tex_facing;
#endif

// OUT
layout(location = 0, index = 0) out vec4 out_frag_overlap_rule;

float checkRule(const float value)
{
	return (int(abs(ceil(value*0.5f)))%2 == 1) ? 1.0f : 0.0f;
}

void main(void)
{
	vec4 A; 
#if multisample
	A = texelFetch(tex_overlap_rule, ivec2(gl_FragCoord.xy), gl_SampleID);
#else
	A = texture(tex_overlap_rule, gl_FragCoord.xy);
#endif
		
	if(first)
		out_frag_overlap_rule = vec4(A.r, checkRule(A.r), A.b, checkRule(A.b));
	else
	{
		uint  F;
		float f = A.b;
#if multisample
		F = texelFetch(tex_facing, ivec2(gl_FragCoord.xy), gl_SampleID).r;
#else		
		F = texture(tex_facing, gl_FragCoord.xy).r;
#endif
		f	   += (F == 1U) ? 1.0f : -1.0f;
		out_frag_overlap_rule = vec4(A.r,A.g,f,checkRule(f));
	}
}
