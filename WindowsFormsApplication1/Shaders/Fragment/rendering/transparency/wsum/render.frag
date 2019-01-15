#include "define.h"

	uniform int  width;
	uniform int  height;
	uniform bool useTexture;
	uniform vec4 color_background;

#if multisample
	layout(binding = 0) uniform sampler2DMS   tex_blend;
#else
	layout(binding = 0) uniform sampler2DRect tex_blend;
#endif
	layout(binding = 1) uniform sampler2D	  tex_background;

	layout(location = 0, index = 0) out vec4  out_frag_color;

	void main(void)
	{
		vec4 sumColor;
		vec4 background = (useTexture) ? texture(tex_background, gl_FragCoord.xy/vec2(width,height)) : color_background;
#if multisample
		sumColor   = texelFetch(tex_blend, ivec2(gl_FragCoord.xy), gl_SampleID);
#else
		sumColor   = texture(tex_blend , gl_FragCoord.xy);
#endif
		out_frag_color = vec4(sumColor.rgb + background.rgb * (1.0 - sumColor.a), 1.0f);
	}
