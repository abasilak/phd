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
layout(location = 1, index = 0) out vec4 out_frag_color[4];

void main(void)
{
	out_frag_depth.xy = vec2(float_min, float_min);
	for(int i=0; i<4; i++)
		out_frag_color[i] = vec4(0.0f);

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

	ivec4 id;
	ivec2 count;
	
#if !draw_buffers_blend
	ivec3 CountID_Front, CountID_Back;
#endif

#if multisample
#if draw_buffers_blend
	id    = ivec4(texelFetch(tex_id   , ivec2(gl_FragCoord.xy), gl_SampleID));
	count = ivec2(texelFetch(tex_count, ivec2(gl_FragCoord.xy), gl_SampleID).xy);
#else
	CountID_Front = ivec3(texelFetch(tex_blend_front, coords, gl_SampleID).rga);
	CountID_Back  = ivec3(texelFetch(tex_blend_back , coords, gl_SampleID).rga);
#endif
#else
#if draw_buffers_blend
	id    = ivec4(texture(tex_id   , gl_FragCoord.xy));
	count = ivec2(texture(tex_count, gl_FragCoord.xy).xy);
#else
	CountID_Front = ivec3(texture(tex_blend_front, gl_FragCoord.xy).rga);
	CountID_Back  = ivec3(texture(tex_blend_back , gl_FragCoord.xy).rga);
#endif
#endif

#if !draw_buffers_blend
	id    = ivec4(CountID_Front.xy, CountID_Back.xy);
	count = ivec2(CountID_Front.z, CountID_Back.z);
#endif
	id.xz = -id.xz;
	
	for(int i=0; i<4; i++)
		if(gl_PrimitiveID == id[i])
		{
			out_frag_color[i] = computePixelColor();
			break;
		}

	out_frag_depth.xy = vec2(-gl_FragCoord.z, gl_FragCoord.z);
	// Front		
	if	   (count.x <= 2 && (gl_FragCoord.z == depth_near || gl_FragCoord.z ==  depth_far))
		out_frag_depth.x = -depth_max;
	else if(count.x >  2 && gl_FragCoord.z != depth_near)
		out_frag_depth.x = -depth_max;
	// Back
	if	   (count.y  <= 2 && (gl_FragCoord.z == depth_near || gl_FragCoord.z == depth_far))
		out_frag_depth.y = -depth_max;
	else if(count.y  >  2 && gl_FragCoord.z != depth_far)
		out_frag_depth.y = -depth_max;
}