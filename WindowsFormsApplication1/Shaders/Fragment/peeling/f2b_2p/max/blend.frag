#include "define.h"
#extension GL_EXT_gpu_shader4 : enable

// IN
#if multisample
	layout(binding = 0) uniform sampler2DMS	  tex_depth;
#if draw_buffers_blend
	layout(binding = 1) uniform sampler2DMS	  tex_count;
	layout(binding = 4) uniform sampler2DMS	  tex_id;
#else
	layout(binding = 1) uniform sampler2DMS	  tex_blend;
#endif

#else
	layout(binding = 0) uniform sampler2DRect tex_depth;
#if draw_buffers_blend	
	layout(binding = 1) uniform sampler2DRect tex_count;
	layout(binding = 4) uniform sampler2DRect tex_id;
#else
	layout(binding = 1) uniform sampler2DRect tex_blend;
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

void main(void)
{
	float depth;
#if multisample
	depth = -texelFetch(tex_depth, ivec2(gl_FragCoord.xy), gl_SampleID).r;
#else
	depth = -texture   (tex_depth, gl_FragCoord.xy).r;
#endif

	if(gl_FragCoord.z == depth)
	{
		ivec2 countId;
#if multisample

#if draw_buffers_blend
		countId = ivec2(texelFetch(tex_id   , ivec2(gl_FragCoord.xy), gl_SampleID).r,
						texelFetch(tex_count, ivec2(gl_FragCoord.xy), gl_SampleID).r);
#else
		countId = ivec2(texelFetch(tex_blend, ivec2(gl_FragCoord.xy), gl_SampleID).ra);
#endif

#else

#if draw_buffers_blend
		countId = ivec2(texture(tex_id, gl_FragCoord.xy).r, texture(tex_count, gl_FragCoord.xy).r);
#else
		countId = ivec2(texture(tex_blend, gl_FragCoord.xy).ra);
#endif

#endif
		if( countId.y <= 1 || countId.x > gl_PrimitiveID)
		{
#if draw_buffers_blend
			out_frag_id.r    = gl_PrimitiveID;
			out_frag_count.r = 1.0f;
#else			
			out_frag_blend.ra = vec2(gl_PrimitiveID, 1.0f);
#endif
			return;
		}
	}
	discard;
}