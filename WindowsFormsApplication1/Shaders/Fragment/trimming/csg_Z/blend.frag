#include "define.h"

#define UNION		 0
#define INTERSECTION 1
#define DIFFERENCE   2

// IN
uniform  bool first;
uniform  int  operation;
uniform uint  new_layers;

layout(binding = 0) uniform usampler2DRect tex_rule_old_1;
layout(binding = 1) uniform usampler2DRect tex_rule_old_2;
layout(binding = 2) uniform  sampler2DRect tex_class;
layout(binding = 3) uniform usampler2DRect tex_layers;
layout(binding = 4) uniform usampler2DRect tex_rule;
layout(binding = 5) uniform  sampler2DRect tex_blend;

// OUT
layout(location = 0, index = 0) out uvec4 out_frag_rule;
layout(location = 1, index = 0) out uvec4 out_frag_layers;


uint boolean(const uint r1, const uint r2)
{
	uint r;
	if	   (operation == UNION		 )	r = r1 |   r2 ;
	else if(operation == INTERSECTION)	r = r1 &   r2 ;
	else								r = r1 & (~r2);
	return r;
}

void main(void)
{
	uint  R11, R12, R21, r1 , r2;
	uint  classification = uint(texture(tex_class, gl_FragCoord.xy).r);
	uvec4 old_rule_1	 = texture(tex_rule_old_1, gl_FragCoord.xy);
	uvec4 old_rule_2	 = texture(tex_rule_old_2, gl_FragCoord.xy);
	uvec2 old_layers	 = texture(tex_layers	 , gl_FragCoord.xy).rg;

	if(classification == 1U)
	{
		old_layers.r++;
		R12 = (old_rule_1.a >> old_layers.r) & 1U;
		R21 = (old_rule_2.a >> old_layers.g) & 1U;
		
		if(first)
		{	
			R11 = old_rule_1.a & 1U;
			r1  = boolean(R11,R21);
		}
		else
			r1  = texture(tex_rule, gl_FragCoord.xy).a;
		r2  = boolean(R12,R21);
	}
	else
	{
		old_layers.g++;
		R12 = (old_rule_2.a >> old_layers.g) & 1U;
		R21 = (old_rule_1.a >> old_layers.r) & 1U;
		if(first)
		{	
			R11 = old_rule_2.a & 1U;
			r1  = boolean(R21,R11);
		}
		else
			r1 = texture(tex_rule, gl_FragCoord.xy).a;

		// Z-fighting
		uint k1 = uint(texture(tex_blend, gl_FragCoord.xy).r);
		if(k1 > 1U)
			R21 = (old_rule_1.a >> (old_layers.r+1U)) & 1U;
		////

		r2  = boolean(R21,R12);		
	}
	out_frag_rule.a	   = (r2 << new_layers) + r1;
	out_frag_layers.rg = old_layers;
}