#include "define.h"
#extension GL_EXT_gpu_shader4 : enable

// Include
	vec4 computePixelColor();
// IN
	//uniform int bufferSize;
#if multisample
	layout(binding = 0) uniform sampler2DMS	tex_peel_0;
	layout(binding = 1) uniform sampler2DMS tex_peel_1;
	layout(binding = 4) uniform sampler2DMS tex_peel_2;
	layout(binding = 5) uniform sampler2DMS tex_peel_3;
	layout(binding = 6) uniform sampler2DMS tex_peel_4;
	layout(binding = 7) uniform sampler2DMS tex_peel_5;
	layout(binding = 8) uniform sampler2DMS tex_peel_6;
	layout(binding = 9) uniform sampler2DMS tex_peel_7;
#else
	layout(binding = 0) uniform sampler2DRect tex_peel_0;
	layout(binding = 1) uniform sampler2DRect tex_peel_1;
	layout(binding = 4) uniform sampler2DRect tex_peel_2;
	layout(binding = 5) uniform sampler2DRect tex_peel_3;
	layout(binding = 6) uniform sampler2DRect tex_peel_4;
	layout(binding = 7) uniform sampler2DRect tex_peel_5;
	layout(binding = 8) uniform sampler2DRect tex_peel_6;
	layout(binding = 9) uniform sampler2DRect tex_peel_7;
#endif
// OUT
	layout(location = 0, index = 0) out vec4 out_frag_color[8];

void main(void)
{
	ivec2 id[8];
#if multisample
	id[0] = ivec2(texelFetch(tex_peel_0, ivec2(gl_FragCoord.xy), gl_SampleID).gb);
	id[1] = ivec2(texelFetch(tex_peel_1, ivec2(gl_FragCoord.xy), gl_SampleID).gb);
	id[2] = ivec2(texelFetch(tex_peel_2, ivec2(gl_FragCoord.xy), gl_SampleID).gb);
	id[3] = ivec2(texelFetch(tex_peel_3, ivec2(gl_FragCoord.xy), gl_SampleID).gb);
	id[4] = ivec2(texelFetch(tex_peel_4, ivec2(gl_FragCoord.xy), gl_SampleID).gb);
	id[5] = ivec2(texelFetch(tex_peel_5, ivec2(gl_FragCoord.xy), gl_SampleID).gb);
	id[6] = ivec2(texelFetch(tex_peel_6, ivec2(gl_FragCoord.xy), gl_SampleID).gb);
	id[7] = ivec2(texelFetch(tex_peel_7, ivec2(gl_FragCoord.xy), gl_SampleID).gb);
#else
	id[0] = ivec2(texture(tex_peel_0, gl_FragCoord.xy).gb);
	id[1] = ivec2(texture(tex_peel_1, gl_FragCoord.xy).gb);
	id[2] = ivec2(texture(tex_peel_2, gl_FragCoord.xy).gb);
	id[3] = ivec2(texture(tex_peel_3, gl_FragCoord.xy).gb);
	id[4] = ivec2(texture(tex_peel_4, gl_FragCoord.xy).gb);
	id[5] = ivec2(texture(tex_peel_5, gl_FragCoord.xy).gb);
	id[6] = ivec2(texture(tex_peel_6, gl_FragCoord.xy).gb);
	id[7] = ivec2(texture(tex_peel_7, gl_FragCoord.xy).gb);
#endif
	for(int i=0; i<KB_SIZE; i++)
	{
		if		(gl_PrimitiveID == -id[i].r) out_frag_color[i].rg = vec2(packUnorm4x8(computePixelColor().abgr), 0.0f);
		else if	(gl_PrimitiveID ==  id[i].g) out_frag_color[i].rg = vec2(0.0f, packUnorm4x8(computePixelColor().abgr));
		else								 out_frag_color[i].rg = vec2(0.0f);
	}
}