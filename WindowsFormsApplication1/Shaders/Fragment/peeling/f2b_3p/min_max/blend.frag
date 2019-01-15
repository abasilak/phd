#include "define.h"
#extension GL_EXT_gpu_shader4 : enable

// IN
#if multisample
#if draw_buffers_blend
	layout(binding = 0) uniform sampler2DMS	  tex_count;
	layout(binding = 1) uniform sampler2DMS	  tex_id;
#else
	layout(binding = 0) uniform sampler2DMS   tex_blend;
#endif
#else
#if draw_buffers_blend
	layout(binding = 0) uniform sampler2DRect tex_count;
	layout(binding = 1) uniform sampler2DRect tex_id;
#else
	layout(binding = 0) uniform sampler2DRect tex_blend;
#endif
#endif
//
// OUT
#if draw_buffers_blend
	layout(location = 0, index = 0) out vec4 out_frag_id;
	layout(location = 1, index = 0) out vec4 out_frag_count;
#else
	layout(location = 0, index = 0) out vec4 out_frag_blend;
#endif

#if early_Z
	layout (early_fragment_tests) in;
#endif

void main(void)
{
	ivec3 IDsCount;
#if draw_buffers_blend
#if multisample
	IDsCount = ivec3(texelFetch(tex_id   , ivec2(gl_FragCoord.xy), gl_SampleID).rg,
	 				 texelFetch(tex_count, ivec2(gl_FragCoord.xy), gl_SampleID).r);
#else
	IDsCount = ivec3(texture(tex_id   , gl_FragCoord.xy).rg, texture(tex_count, gl_FragCoord.xy).r);
#endif
#else
#if multisample
	IDsCount = ivec3(texelFetch(tex_blend, ivec2(gl_FragCoord.xy), gl_SampleID).rga);
#else
	IDsCount = ivec3(texture   (tex_blend, gl_FragCoord.xy).rga);
#endif

#endif
	if( IDsCount.z <= 2 || (-IDsCount.x < gl_PrimitiveID && IDsCount.y > gl_PrimitiveID) )
	{
#if draw_buffers_blend
		out_frag_count.r = 1.0f;
		out_frag_id.rg	 = vec2(-gl_PrimitiveID, gl_PrimitiveID);
#else
		out_frag_blend.rga = vec3(-gl_PrimitiveID, gl_PrimitiveID, 1.0f);
#endif
	}
	else
	{
#if draw_buffers_blend
		out_frag_count.r = 0.0f;
		out_frag_id.rg   = vec2(float_min, 0.0f);
#else
		out_frag_blend.rga = vec3(float_min, 0.0f, 0.0f);
#endif
	}
}