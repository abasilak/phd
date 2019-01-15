#include "define.h"
#extension GL_EXT_gpu_shader4 : enable

// INCLUDE
vec4 computePixelColor();
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
// OUT
layout(location = 0, index = 0) out  vec4 out_frag_depth;
layout(location = 1, index = 0) out  vec4 out_frag_color;

void main(void)
{
	float depth;
#if multisample
	depth =	-texelFetch(tex_depth, ivec2(gl_FragCoord.xy), gl_SampleID).r;
#else
	depth =	-texture(tex_depth, gl_FragCoord.xy).r;
#endif
	if (gl_FragCoord.z < depth)
		discard;

	ivec2 countID;
#if multisample

#if draw_buffers_blend	
	countID = ivec2(texelFetch(tex_id   , ivec2(gl_FragCoord.xy), gl_SampleID).r,
					texelFetch(tex_count, ivec2(gl_FragCoord.xy), gl_SampleID).r);
#else
	countID = ivec2(texelFetch(tex_blend, ivec2(gl_FragCoord.xy), gl_SampleID).ra);
#endif

#else

#if draw_buffers_blend	
	countID = ivec2(texture(tex_id   , gl_FragCoord.xy).r, texture(tex_count, gl_FragCoord.xy).r);
#else
	countID = ivec2(texture(tex_blend, gl_FragCoord.xy).ra);
#endif

#endif
	out_frag_depth.r = (countID.y > 1 || gl_FragCoord.z != depth) ? -gl_FragCoord.z : -depth_max;
	out_frag_color   = (countID.x == gl_PrimitiveID) ? computePixelColor() : vec4(0.0f);
}