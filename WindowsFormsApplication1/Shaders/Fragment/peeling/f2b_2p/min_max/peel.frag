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
layout(location = 1, index = 0) out  vec4 out_frag_color_front;
layout(location = 2, index = 0) out  vec4 out_frag_color_back ;

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

	ivec3 countIDs;
#if multisample

#if draw_buffers_blend	
	countIDs = ivec3(texelFetch(tex_id   , ivec2(gl_FragCoord.xy), gl_SampleID).rg,
					 texelFetch(tex_count, ivec2(gl_FragCoord.xy), gl_SampleID).r);
#else
	countIDs = ivec3(texelFetch(tex_blend, ivec2(gl_FragCoord.xy), gl_SampleID).rga);
#endif

#else

#if draw_buffers_blend	
	countIDs = ivec3(texture(tex_id   , gl_FragCoord.xy).rg, texture(tex_count, gl_FragCoord.xy).r);
#else
	countIDs = ivec3(texture(tex_blend, gl_FragCoord.xy).rga);
#endif

#endif
	out_frag_color_front = vec4(0.0f);
	out_frag_color_back  = vec4(0.0f);

	out_frag_depth.r	 = ( countIDs.z > 2 || gl_FragCoord.z != depth) ? -gl_FragCoord.z : -depth_max;
	if		(-countIDs.x == gl_PrimitiveID) out_frag_color_front = computePixelColor();
	else if ( countIDs.y == gl_PrimitiveID) out_frag_color_back  = computePixelColor();
}