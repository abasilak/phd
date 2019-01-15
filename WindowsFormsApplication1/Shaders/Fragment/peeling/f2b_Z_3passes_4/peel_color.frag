#include "define.h"
#extension GL_EXT_gpu_shader4 : enable

// INCLUDE
vec4 computePixelColor();
// IN
#if multisample
	uniform sampler2DMS	  tex_id;
#else
	uniform sampler2DRect tex_id;
#endif
// OUT
#if packing
	layout(location = 0, index = 0) out uvec4 out_frag_color;
#else
	layout(location = 0, index = 0) out  vec4 out_frag_color;
#endif

void main(void)
{
	int id;

#if multisample
	id = int(texelFetch	(tex_id, ivec2(gl_FragCoord.xy), gl_SampleID).r);
#else
	id = int(texture	(tex_id, gl_FragCoord.xy).r);
#endif
	if (id == gl_PrimitiveID)
	{
		vec4 c = computePixelColor();
#if packing
		out_frag_color.r = packUnorm4x8(c);
#else
		out_frag_color   = c;
#endif
	}
	else
		discard;
}