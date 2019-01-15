#include "define.h"

// IN
uniform float cappingPlane;
uniform float cappingAngle;
#if multisample

#if packing
	layout(binding = 0) uniform usampler2DMS   tex_color_front;
	layout(binding = 1) uniform usampler2DMS   tex_color_back;
#else
	layout(binding = 0) uniform  sampler2DMS   tex_color_front;
	layout(binding = 1) uniform  sampler2DMS   tex_color_back;
#endif
	layout(binding = 2) uniform  sampler2DMS   tex_depth;
	layout(binding = 3) uniform usampler2DMS   tex_lock_overlap;
#else

#if packing
	layout(binding = 0) uniform usampler2DRect tex_color_front;
	layout(binding = 1) uniform usampler2DRect tex_color_back;
#else
	layout(binding = 0) uniform  sampler2DRect tex_color_front;
	layout(binding = 1) uniform  sampler2DRect tex_color_back;
#endif
	layout(binding = 2) uniform  sampler2DRect tex_depth;
	layout(binding = 3) uniform usampler2DRect tex_lock_overlap;
#endif

// OUT
#if packing
	layout(location = 0, index = 0) out uvec4 out_frag_color_front;
	layout(location = 1, index = 0) out uvec4 out_frag_color_back;
#else
	layout(location = 0, index = 0) out  vec4 out_frag_color_front;
	layout(location = 1, index = 0) out  vec4 out_frag_color_back;
#endif
	layout(location = 2, index = 0) out  vec4 out_frag_depth;
	layout(location = 3, index = 0) out uvec4 out_frag_lock_overlap;

void main(void)
{
	uvec4 L_O;
#if multisample
	L_O = texelFetch(tex_lock_overlap, ivec2(gl_FragCoord.xy), gl_SampleID);
#else
	L_O = texture(tex_lock_overlap, gl_FragCoord.xy);
#endif
	vec2 depth;
#if multisample
	depth = -texelFetch(tex_depth, ivec2(gl_FragCoord.xy), gl_SampleID).rg;
#else
	depth = -texture(tex_depth, gl_FragCoord.xy).rg;
#endif

#if packing
	out_frag_color_front.r = 0U;
	out_frag_color_back.r  = 0U;
#else
	out_frag_color_front  = vec4(0.0f);
	out_frag_color_back   = vec4(0.0f);
#endif
	out_frag_depth.rg     = vec2(0.0f);
	out_frag_lock_overlap = L_O;

	float k = 1.0f - float(gl_FragCoord.x)/cappingAngle;
	float capping = cappingPlane*k;

	if(L_O.r == 0U)
	{
		if(depth.r < capping)
			out_frag_lock_overlap.rg = uvec2(0U, L_O.g + 1U);
		else
		{
			vec4 color;
#if multisample
			color = texelFetch(tex_color_front, ivec2(gl_FragCoord.xy), gl_SampleID);
#else
			color = texture(tex_color_front, gl_FragCoord.xy);
#endif

#if packing
			out_frag_color_front.r  = color.r;
#else
			out_frag_color_front	= color;
#endif
			out_frag_depth.r		= depth.r;
			out_frag_lock_overlap.rg = uvec2(1U, L_O.g); 
		}
	}
	if(L_O.b == 0U)
	{
		if(depth.g < capping)
			out_frag_lock_overlap.ba = uvec2(0U, L_O.a + 1U);
		else
		{	
			vec4 color;
#if multisample
			color = texelFetch(tex_color_back, ivec2(gl_FragCoord.xy), gl_SampleID);
#else
			color = texture(tex_color_back, gl_FragCoord.xy);
#endif

#if packing
			out_frag_color_back.r	= color.r;
#else
			out_frag_color_back		= color;
#endif
			out_frag_depth.g		= depth.g;
			out_frag_lock_overlap.ba= uvec2(1U, L_O.a);
		}
	}
}
