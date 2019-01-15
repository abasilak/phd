#include "version.h"

	uniform int width;
	uniform int height;

	layout(binding = 0) uniform sampler2D tex_color;
	
	layout(location = 0, index = 0) out vec4 out_frag_color;

	void main(void)
	{
		out_frag_color = texture(tex_color, gl_FragCoord.xy/vec2(width,height));
	}