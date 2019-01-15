#include "define.h"

// IN
#if multisample
	layout(binding = 0) uniform sampler2DMS tex_depth;
#if draw_buffers_blend
	layout(binding = 1) uniform sampler2DMS	tex_count;
#else
	layout(binding = 1) uniform sampler2DMS tex_blend_front;
	layout(binding = 2) uniform sampler2DMS tex_blend_back;
#endif
#else
	layout(binding = 0) uniform sampler2DRect tex_depth;
#if draw_buffers_blend
	layout(binding = 1) uniform sampler2DRect tex_count;
#else
	layout(binding = 1) uniform sampler2DRect tex_blend_front;
	layout(binding = 2) uniform sampler2DRect tex_blend_back;
#endif
#endif
// OUT
layout(location = 0, index = 0) out vec4 out_frag_depth;

void main(void)
{
	vec2 depth;
#if multisample
	ivec2 coords = ivec2(gl_FragCoord.xy);
	depth =	texelFetch(tex_depth, coords, gl_SampleID).rg;
#else
	depth = texture   (tex_depth, gl_FragCoord.xy).rg;
#endif
	float depth_near = -depth.x;
	float depth_far	 =  depth.y;

	if (gl_FragCoord.z < depth_near || gl_FragCoord.z > depth_far)
		discard;
	out_frag_depth.xy = vec2(-gl_FragCoord.z, gl_FragCoord.z);

	ivec2 count;
#if multisample
#if draw_buffers_blend	
	count   = ivec2(texelFetch(tex_count, coords, gl_SampleID).xy);
#else
	count.x = int(texelFetch(tex_blend_front, coords, gl_SampleID).a);
	count.y = int(texelFetch(tex_blend_back , coords, gl_SampleID).a);
#endif
#else
#if draw_buffers_blend	
	count   = ivec2(texture(tex_count, gl_FragCoord.xy).xy);
#else
	count.x = int(texture(tex_blend_front, gl_FragCoord.xy).a);
	count.y = int(texture(tex_blend_back , gl_FragCoord.xy).a);
#endif
#endif
	// Front
	if	   (count.x <= 1 && (gl_FragCoord.z == depth_near || gl_FragCoord.z ==  depth_far))
		out_frag_depth.x = -depth_max;
	else if(count.x >  1 && gl_FragCoord.z != depth_near)
		out_frag_depth.x = -depth_max;
	// Back
	if	   (count.y  <= 1 && (gl_FragCoord.z == depth_near || gl_FragCoord.z == depth_far))
		out_frag_depth.y = -depth_max;
	else if(count.y  >  1 && gl_FragCoord.z != depth_far)
		out_frag_depth.y = -depth_max;
}