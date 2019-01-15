#include "define.h"

// IN
	uniform float cappingPlane;
	uniform float cappingAngle;

#if multisample

#if packing
	layout(binding = 0) uniform usampler2DMS   tex_color;
#else
	layout(binding = 0) uniform  sampler2DMS   tex_color;
#endif
	layout(binding = 1) uniform  sampler2DMS   tex_depth;
	layout(binding = 2) uniform usampler2DMS   tex_lock_overlap;
#else

#if packing
	layout(binding = 0) uniform usampler2DRect tex_color;
#else
	layout(binding = 0) uniform  sampler2DRect tex_color;
#endif
	layout(binding = 1) uniform  sampler2DRect tex_depth;
	layout(binding = 2) uniform usampler2DRect tex_lock_overlap;
#endif

// OUT
#if packing
	layout(location = 0, index = 0) out uvec4 out_frag_color;
#else
	layout(location = 0, index = 0) out  vec4 out_frag_color;
#endif
	layout(location = 1, index = 0) out  vec4 out_frag_depth;
	layout(location = 2, index = 0) out uvec4 out_frag_lock_overlap;

void main(void)
{

	uvec2 L_O;
#if multisample
	L_O = texelFetch(tex_lock_overlap, ivec2(gl_FragCoord.xy), gl_SampleID).rg;
#else
	L_O = texture(tex_lock_overlap, gl_FragCoord.xy).rg;
#endif
	if(L_O.x == 1U) discard;

	float depth;
#if multisample
	depth = texelFetch(tex_depth, ivec2(gl_FragCoord.xy), gl_SampleID).r;
#else
	depth = texture(tex_depth, gl_FragCoord.xy).r;
#endif

	float k = 1.0f - float(gl_FragCoord.x)/cappingAngle;
	float capping = cappingPlane*k;

	if(depth < capping)
	{
#if packing
		out_frag_color.r		 = 0U;
#else
		out_frag_color			 = vec4(0.0f);
#endif
		out_frag_depth.r		 = 0.0f;
		out_frag_lock_overlap.rg = uvec2(0U, L_O.y + 1U);
	}
	else
	{
#if packing
		uvec4 color;
#else
		 vec4 color;
#endif

#if multisample
		color = texelFetch(tex_color, ivec2(gl_FragCoord.xy), gl_SampleID);
#else
		color = texture(tex_color, gl_FragCoord.xy);
#endif

#if packing
		out_frag_color.r		 = color.r;
#else
		out_frag_color			 = color;
#endif
		out_frag_depth.r		 = depth;
		out_frag_lock_overlap.rg = uvec2(1U, L_O.y);
	}
}
