#include "define.h"

// IN
#if multisample
	layout(binding = 0) uniform sampler2DMS   tex_depth;
#else
	layout(binding = 0) uniform sampler2DRect tex_depth;
#endif
// OUT
layout(location = 0, index = 0) out vec4 out_frag_thickness;

void main(void)
{
	vec2  thickness;
	// r: final, g: prev
#if multisample
	thickness = texelFetch(tex_depth, ivec2(gl_FragCoord.xy), gl_SampleID).zw;
#else
	thickness = texture(tex_depth, gl_FragCoord.xy).zw;
#endif
	if(thickness.y == 1.0f)	  thickness.y =  0.0f;
	if(int(thickness.x) == 0)  thickness.y = -thickness.y;
	out_frag_thickness.r = thickness.y;
}