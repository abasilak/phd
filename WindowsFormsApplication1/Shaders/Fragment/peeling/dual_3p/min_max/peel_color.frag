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
layout(location = 0, index = 0) out vec4 out_frag_color[4];

void main(void)
{
	ivec4 id;
#if multisample
	ivec2 coords  = ivec2(gl_FragCoord.xy);
#if draw_buffers_blend		
	id    = ivec4(texelFetch(tex_id			, coords, gl_SampleID));
#else
	id.xy = ivec2(texelFetch(tex_blend_front, coords, gl_SampleID).xy);
	id.zw = ivec2(texelFetch(tex_blend_back , coords, gl_SampleID).xy);
#endif
#else
#if draw_buffers_blend	
	id    = ivec4(texture(tex_id		 , gl_FragCoord.xy));
#else
	id.xy = ivec2(texture(tex_blend_front, gl_FragCoord.xy).xy);
	id.zw = ivec2(texture(tex_blend_back , gl_FragCoord.xy).xy);
#endif
#endif
	id.xz = -id.xz;

	for(int i=0; i<4; i++)
		if(gl_PrimitiveID == id[i])
		{
			out_frag_color[i] = computePixelColor();
			return;
		}
	discard;
}