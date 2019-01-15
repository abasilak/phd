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
//
// OUT
layout(location = 0, index = 0) out  vec4 out_frag_color_front;
layout(location = 1, index = 0) out  vec4 out_frag_color_back;

void main(void)
{
	ivec2 id;
#if multisample
	id = ivec2(texelFetch(tex_id, ivec2(gl_FragCoord.xy), gl_SampleID).xy);
#else
	id = ivec2(texture	 (tex_id, gl_FragCoord.xy).xy);
#endif
	out_frag_color_front = (				gl_PrimitiveID == id.x) ? computePixelColor() : vec4(0.0f);
	// (id.x != id.y) : Only Front exports final (middle) layer !!
	out_frag_color_back  = (id.x != id.y && gl_PrimitiveID == id.y) ? computePixelColor() : vec4(0.0f);
}