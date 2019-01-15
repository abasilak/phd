#include "define.h"

// IN
#if multisample
	uniform int samples;
	layout(binding = 0) uniform sampler2DMS   tex_color;
#else
	layout(binding = 0) uniform sampler2DRect tex_color;
#endif
// OUT
	layout(location = 0, index = 0) out vec4 out_frag_color;

void main(void)
{
#if multisample
	vec4 color = vec4(0.0f);
	for(int i=0; i<samples; i++)
		color += texelFetch(tex_color, ivec2(gl_FragCoord.xy), i);
	out_frag_color = color/float(samples);
#else
	vec4 color = texture(tex_color, gl_FragCoord.xy);
//  Peeling Samples Count - Background = Black
#if peeling_error
	if(color.r == 0.0f)
		discard;
#endif
	out_frag_color = color;
#endif
}