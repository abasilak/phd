#include "define.h"

// IN
uniform  bool first;
uniform  int  frames;

layout(binding = 0) uniform  sampler2DRect tex_overlap;
layout(binding = 1) uniform  sampler2DRect tex_depth;
layout(binding = 2) uniform  sampler2DRect tex_depth_old;
layout(binding = 3) uniform  sampler2DRect tex_layers_old;
layout(binding = 4) uniform usampler2DRect tex_layers;
layout(binding = 5) uniform usampler2DRect tex_facing_class;
layout(binding = 6) uniform usampler2DRect tex_rule;
layout(binding = 7) uniform usampler2DRect tex_rule_old;

// OUT
layout(location = 0, index = 0) out  vec4 out_frag_overlap;
layout(location = 1, index = 0) out uvec4 out_frag_rule;
layout(location = 2, index = 0) out uvec4 out_frag_layers;

float checkRule(const float value)
{
	return (int(abs(ceil(value*0.5f)))%2 == 1) ? 1.0f : 0.0f;
}

float dynamic(const float O, const float O_prev, const float n1)
{
	float n;
	if		(O > O_prev )	n = 1.0f;
	else if	(O < O_prev )	n = 0.0f;
	else					n = n1;
	return n;
}

void main(void)
{
	float R1,R2;
	uint  old_rule,old_layers,new_layers,R;

	vec4 O = texture(tex_overlap, gl_FragCoord.xy);
	if(frames > 0)
		old_rule = texture(tex_rule_old, gl_FragCoord.xy).a;

	if(first)
	{		
		new_layers = 1U;
		if(frames == 0)
		{
			R1 = checkRule(O.r);
			R2 = checkRule(O.g);
			out_frag_overlap.rg = O.rg;
		}
		else
		{
			vec2  D,old_R;
			float depth		= texture(tex_depth	   , gl_FragCoord.xy).r;
			float depth_old = texture(tex_depth_old, gl_FragCoord.xy).r;

			old_layers	= uint(texture(tex_layers_old, gl_FragCoord.xy).r);			
			old_R.x		= float((old_rule >> (old_layers-1U)) & 1U);
			old_R.y		= float((old_rule >>  old_layers    ) & 1U);

			if		(depth == depth_old) { D = O.ba; }
			else if (depth  < depth_old) { D = O.bb; old_R.y = old_R.x; old_layers--;}
			else						 { D = O.aa; old_R.x = old_R.y;}

			R1 = dynamic(O.r,D.x,old_R.x);
			R2 = dynamic(O.g,D.y,old_R.y);
			out_frag_overlap.rg = vec2(D.y, O.g);
		}
		R = (uint(R2) << new_layers) + uint(R1);
	}
	else
	{
		float old_R;
		R		   = texture(tex_rule  , gl_FragCoord.xy).a;
		uvec2 L	   = texture(tex_layers, gl_FragCoord.xy).rg;
		old_layers = L.x;
		new_layers = L.y;
		uvec2 f_c  = texture(tex_facing_class, gl_FragCoord.xy).rg;
		float f    = (f_c.x == 1U) ? 1.0f : -1.0f;	
		
		if(frames==0)
		{
			new_layers++;
			f += O.g;
			R1 = checkRule(f);
			out_frag_overlap.rg = vec2(O.r,f);
		}
		else
		{
			if(f_c.y == 2U)
			{
				new_layers++;
				f += O.g;
				old_R = float((old_rule >> old_layers) & 1U);
				R1 = dynamic(f,O.r,old_R);	
				out_frag_overlap.rg = vec2(O.r,f);
			}
			else if(f_c.y == 0U)
			{
				old_layers++;
				out_frag_overlap.rg = vec2(O.r+f,O.g);
			}
			else 
			{
				old_layers++;
				new_layers++;
				float f1 = f + O.r;
				float f2 = f + O.g;
				old_R = float((old_rule >> old_layers) & 1U);
				R1 = dynamic(f2,f1,old_R);
				out_frag_overlap.rg = vec2(f1,f2);
			}	
		}
		R  = (uint(R1) << new_layers) + R;
	}
	out_frag_layers.rg = uvec2(old_layers,new_layers);
	out_frag_rule.a	   = R;
}