#include "define.h"
#extension GL_EXT_gpu_shader4 : enable

// INCLUDE
vec4 computePixelColor();
// IN
#if multisample
	layout(binding = 0) uniform sampler2DMS tex_depth;
#if draw_buffers_blend
	layout(binding = 1) uniform sampler2DMS	tex_count;
	layout(binding = 4) uniform sampler2DMS	tex_id;
#else
	layout(binding = 1) uniform sampler2DMS tex_blend_front;
	layout(binding = 4) uniform sampler2DMS tex_blend_back;
#endif
#else
	layout(binding = 0) uniform sampler2DRect tex_depth;
#if draw_buffers_blend
	layout(binding = 1) uniform sampler2DRect tex_count;
	layout(binding = 4) uniform sampler2DRect tex_id;
#else
	layout(binding = 1) uniform sampler2DRect tex_blend_front;
	layout(binding = 4) uniform sampler2DRect tex_blend_back;
#endif
#endif

// OUT
	layout(location = 0, index = 0) out vec4 out_frag_depth;
	layout(location = 1, index = 0) out vec4 out_frag_color_front;
	layout(location = 2, index = 0) out vec4 out_frag_color_back;

void main(void)
{
	vec2  depth;
#if multisample
	ivec2 coords  = ivec2(gl_FragCoord.xy);
	depth =	texelFetch(tex_depth, coords, gl_SampleID).rg;
#else
	depth = texture	  (tex_depth, gl_FragCoord.xy).rg;
#endif
	float depth_near = -depth.x;
	float depth_far	 =  depth.y;

	if (gl_FragCoord.z < depth_near || gl_FragCoord.z > depth_far)
		discard;

	ivec2 count, id;
#if !draw_buffers_blend
	ivec2 CountID_Front, CountID_Back;
#endif

#if multisample
#if draw_buffers_blend
	id    = ivec2(texelFetch(tex_id   , ivec2(gl_FragCoord.xy), gl_SampleID).xy);
	count = ivec2(texelFetch(tex_count, ivec2(gl_FragCoord.xy), gl_SampleID).xy);
#else
	CountID_Front = ivec2(texelFetch(tex_blend_front, coords, gl_SampleID).ra);
	CountID_Back  = ivec2(texelFetch(tex_blend_back , coords, gl_SampleID).ra);
#endif
#else
#if draw_buffers_blend
	id    = ivec2(texture(tex_id   , gl_FragCoord.xy).xy);
	count = ivec2(texture(tex_count, gl_FragCoord.xy).xy);
#else
	CountID_Front = ivec2(texture(tex_blend_front, gl_FragCoord.xy).ra);
	CountID_Back  = ivec2(texture(tex_blend_back , gl_FragCoord.xy).ra);
#endif
#endif

#if !draw_buffers_blend
	id    = ivec2(CountID_Front.x, CountID_Back.x);
	count = ivec2(CountID_Front.y, CountID_Back.y);
#endif
	
	out_frag_color_front = (				gl_PrimitiveID == id.x) ? computePixelColor() : vec4(0.0f);
	out_frag_color_back  = (id.x != id.y && gl_PrimitiveID == id.y) ? computePixelColor() : vec4(0.0f);

	out_frag_depth.xy = vec2(-gl_FragCoord.z, gl_FragCoord.z);
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