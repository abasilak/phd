#include "define.h"

	uniform int  width;
	uniform int  height;
	uniform bool useTexture;
	uniform vec4 color_background;

#if multisample
	layout(binding = 0) uniform sampler2DMS   tex_blend;
	layout(binding = 1) uniform sampler2DMS   tex_layers;
#else
	layout(binding = 0) uniform sampler2DRect tex_blend;
	layout(binding = 1) uniform sampler2DRect tex_layers;
#endif
	layout(binding = 2) uniform sampler2D	  tex_background;
	
	layout(location = 0, index = 0) out vec4  out_frag_color;

void main(void)
{
	float num;
	vec4  background, sumColor, c;
	ivec2 coords = ivec2(gl_FragCoord.xy);

	background = (useTexture) ? texture(tex_background, gl_FragCoord.xy/vec2(width,height)) : color_background;
#if multisample
	sumColor   = texelFetch(tex_blend , coords, gl_SampleID);
	num		   = texelFetch(tex_layers, coords, gl_SampleID).r;
#else
	sumColor   = texture(tex_blend , gl_FragCoord.xy);
	num		   = texture(tex_layers, gl_FragCoord.xy).r;
#endif
	if (num > 0.0f)
	{
		vec3  AvgColor = sumColor.rgb / sumColor.a;
		float AvgAlpha = sumColor.a / num;
		float T = pow(1.0f - AvgAlpha, num);
		
		c.rgb = AvgColor * (1.0f-T) + background.rgb * T;
		c.a   = 1.0f;
	}
	else 
		c = background;
	out_frag_color = c;
}
