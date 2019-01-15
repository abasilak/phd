#include "define.h"

// IN
	uniform int	layers;
#if multisample
	layout(binding = 0) uniform sampler2DMS tex_depth;
	layout(binding = 1) uniform sampler2DMS tex_thickness;
	layout(binding = 2) uniform sampler2DMS tex_color;
#else
	layout(binding = 0) uniform sampler2DRect tex_depth;
	layout(binding = 1) uniform sampler2DRect tex_thickness;
	layout(binding = 2) uniform sampler2DRect tex_color;
#endif
// OUT
	layout(location = 0, index = 0) out vec4 out_frag_color;

void main(void)
{
	float  front_fac;
	float depth, thickness;
#if multisample
	ivec2 coords = ivec2(gl_FragCoord.xy);
	thickness = texelFetch(tex_thickness , coords, gl_SampleID).r;
	depth     = texelFetch(tex_depth	 , coords, gl_SampleID).r;
	front_fac = texelFetch(tex_color	 , coords, gl_SampleID).a;
#else
	thickness = texture(tex_thickness , gl_FragCoord.xy).r;
	depth	  = texture(tex_depth     , gl_FragCoord.xy).r;
	front_fac = texture(tex_color	  , gl_FragCoord.xy).a;
#endif
	// alternate assumpsion : front-back-front-back
	if(layers == 1) depth = -depth;
	//if(front_fac == 0.0f) depth = -depth;
	out_frag_color.r = thickness + depth;
}