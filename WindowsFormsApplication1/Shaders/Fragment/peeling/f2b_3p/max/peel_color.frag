#include "define.h"
#extension GL_EXT_gpu_shader4 : enable

// INCLUDE
vec4 computePixelColor();
// IN
#if multisample
	layout(binding = 0) uniform sampler2DMS	  tex_id;
#else
	layout(binding = 0) uniform sampler2DRect tex_id;
#endif

// OUT
layout(location = 0, index = 0) out  vec4 out_frag_color;

#if early_Z
	layout (early_fragment_tests) in;
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
		out_frag_color = computePixelColor();
	else
		discard;
}