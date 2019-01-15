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
layout(location = 0, index = 0) out  vec4 out_frag_color_front;
layout(location = 1, index = 0) out  vec4 out_frag_color_back;

#if early_Z
	layout (early_fragment_tests) in;
#endif

void main(void)
{
	ivec2 IDs;
#if multisample
	IDs = ivec2(texelFetch	(tex_id, ivec2(gl_FragCoord.xy), gl_SampleID).rg);
#else
	IDs = ivec2(texture		(tex_id, gl_FragCoord.xy).rg);
#endif
	if		(-IDs.x == gl_PrimitiveID ) out_frag_color_front = computePixelColor();
	else if ( IDs.y == gl_PrimitiveID )	out_frag_color_back  = computePixelColor();
	else
		discard;
}