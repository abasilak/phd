#include "define.h"

// IN
#if multisample
	layout(binding = 0) uniform sampler2DMS   tex_color_front;
	layout(binding = 1) uniform sampler2DMS   tex_color_back;
#else
	layout(binding = 0) uniform sampler2DRect tex_color_front;
	layout(binding = 1) uniform sampler2DRect tex_color_back;
#endif
// OUT
	layout(location = 0, index = 0) out vec4  out_frag_color;

void main(void)
{
	vec4 frontColor,backColor;

	ivec2 coords = ivec2(gl_FragCoord.xy);
#if multisample
	frontColor = texelFetch(tex_color_front, coords, gl_SampleID);
	backColor  = texelFetch(tex_color_back , coords, gl_SampleID);
#else
	frontColor = texture(tex_color_front, gl_FragCoord.xy);
	backColor  = texture(tex_color_back , gl_FragCoord.xy);
#endif
	vec4 c = frontColor + backColor * frontColor.a;

	out_frag_color   = c;
}
