#include "define.h"
#extension GL_EXT_gpu_shader4 : enable

// INCLUDE
vec4 computePixelColor();
// IN
#if multisample
#if draw_buffers_blend
	layout(binding = 0) uniform sampler2DMS	tex_id;
#else
	layout(binding = 0) uniform sampler2DMS tex_blend_front;
	layout(binding = 1) uniform sampler2DMS tex_blend_back;
#endif
#else
#if draw_buffers_blend
	layout(binding = 0) uniform sampler2DRect tex_id;
#else
	layout(binding = 0) uniform sampler2DRect tex_blend_front;
	layout(binding = 1) uniform sampler2DRect tex_blend_back;
#endif
#endif

// OUT
layout(location = 0, index = 0) out vec4 out_frag_color_front;
layout(location = 1, index = 0) out vec4 out_frag_color_back;

void main(void)
{
	ivec2 id;
#if multisample
	ivec2 coords  = ivec2(gl_FragCoord.xy);
#if draw_buffers_blend		
	id   = ivec2(texelFetch(tex_id, coords, gl_SampleID).xy);
#else
	id.x = int(texelFetch(tex_blend_front, coords, gl_SampleID).r);
	id.y = int(texelFetch(tex_blend_back , coords, gl_SampleID).r);
#endif
#else
#if draw_buffers_blend	
	id   = ivec2(texture(tex_id, gl_FragCoord.xy).xy);
#else
	id.x = int(texture(tex_blend_front, gl_FragCoord.xy).r);
	id.y = int(texture(tex_blend_back , gl_FragCoord.xy).r);
#endif
#endif

	out_frag_color_front = (				gl_PrimitiveID == id.x) ? computePixelColor() : vec4(0.0f);
	// (id.x != id.y) : Only Front exports final (middle) layer !!
	out_frag_color_back  = (id.x != id.y && gl_PrimitiveID == id.y) ? computePixelColor() : vec4(0.0f);
}