#include "define.h"

// Include
vec4 computePixelColor();

// IN
#if multisample
	layout(binding = 0) uniform sampler2DMS	  tex_depth;
#else
	layout(binding = 0) uniform sampler2DRect tex_depth;
#endif

// OUT
	layout(location = 0, index = 0) out  vec4 out_frag_depth;
#if packing
	layout(location = 1, index = 0) out uvec4 out_frag_color_front;
	layout(location = 2, index = 0) out uvec4 out_frag_color_back;
#else
	layout(location = 1, index = 0) out  vec4 out_frag_color_front;
	layout(location = 2, index = 0) out  vec4 out_frag_color_back;
#endif

void main(void)
{
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
	out_frag_color_front = vec4(0.0f);
	out_frag_color_back  = vec4(0.0f);
#endif
	out_frag_depth.rg	 = vec2(-depth_max);
	
	if(gl_FrontFacing == true)
	{
		if(gl_FragCoord.z == depth.r)
		{
			vec4 c = computePixelColor();
#if packing
			out_frag_color_front.r = packUnorm4x8(c);
#else
			out_frag_color_front   = c;
#endif
		}
		else if(gl_FragCoord.z > depth.r)
			out_frag_depth.r = -gl_FragCoord.z;
	}
	else
	{
		if(gl_FragCoord.z == depth.g)
		{
			vec4 c = computePixelColor();
#if packing
			out_frag_color_back.r = packUnorm4x8(c);
#else
			out_frag_color_back   = c;
#endif
		}
		else if(gl_FragCoord.z > depth.g)
			out_frag_depth.g = -gl_FragCoord.z;
	}
}