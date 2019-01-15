#include "define.h"
#extension GL_EXT_gpu_shader4 : enable

// IN
#if multisample
	layout(binding = 0) uniform sampler2DMS tex_depth;
#if draw_buffers_blend
	layout(binding = 1) uniform sampler2DMS	tex_count;
	layout(binding = 2) uniform sampler2DMS	tex_id;
#else
	layout(binding = 1) uniform sampler2DMS tex_blend_front;
	layout(binding = 2) uniform sampler2DMS tex_blend_back;
#endif
#else
	layout(binding = 0) uniform sampler2DRect tex_depth;
#if draw_buffers_blend
	layout(binding = 1) uniform sampler2DRect tex_count;
	layout(binding = 2) uniform sampler2DRect tex_id;
#else
	layout(binding = 1) uniform sampler2DRect tex_blend_front;
	layout(binding = 2) uniform sampler2DRect tex_blend_back;
#endif
#endif

// OUT
#if draw_buffers_blend

layout(location = 0, index = 0) out vec4 out_frag_id;
layout(location = 1, index = 0) out vec4 out_frag_count;

vec2 compute(const in float depth, const in int count, const in int id)
{
	return (depth == gl_FragCoord.z && (count <= 1 || id > gl_PrimitiveID)) ? vec2(gl_PrimitiveID, 1.0f) : vec2(0.0f);
}

void main(void)
{
	out_frag_id.rg 	  = vec2(0.0);
	out_frag_count.rg = vec2(0.0);

	vec2 depth;
#if multisample
	depth =	texelFetch(tex_depth, ivec2(gl_FragCoord.xy), gl_SampleID).xy;
#else
	depth =	texture(tex_depth, gl_FragCoord.xy).xy;
#endif
	if (gl_FragCoord.z != -depth.x && gl_FragCoord.z != depth.y)
		discard;
	
	ivec2  count,id;
#if multisample
	count = ivec2(texelFetch(tex_count, ivec2(gl_FragCoord.xy), gl_SampleID).xy);
	id	  = ivec2(texelFetch(tex_id	  , ivec2(gl_FragCoord.xy), gl_SampleID).xy);
#else
	count = ivec2(texture(tex_count, gl_FragCoord.xy).xy);
	id	  = ivec2(texture(tex_id   , gl_FragCoord.xy).xy);
#endif
	vec2 blend_front = compute(-depth.x, count.x, id.x);
	vec2 blend_back  = compute( depth.y, count.y, id.y);

	out_frag_id.rg 	  = vec2(blend_front.x, blend_back.x);
	out_frag_count.rg = vec2(blend_front.y, blend_back.y);
}

#else

layout(location = 0, index = 0) out vec4 out_frag_blend_front;
layout(location = 1, index = 0) out vec4 out_frag_blend_back;

vec2 compute(const in sampler2DRect tex)
{
	ivec2 CountID;
#if multisample
	CountID = ivec2(texelFetch(tex, ivec2(gl_FragCoord.xy), gl_SampleID).ra;
#else
	CountID = ivec2(   texture(tex, gl_FragCoord.xy).ra);
#endif
	return (CountID.y <= 1 || CountID.x > gl_PrimitiveID) ? vec2(gl_PrimitiveID, 1.0f) : vec2(0.0f);
}

void main(void)
{
	out_frag_blend_front.ra = vec2(0.0f);
	out_frag_blend_back.ra  = vec2(0.0f);

	vec2 depth;
#if multisample
	depth =	texelFetch(tex_depth, ivec2(gl_FragCoord.xy), gl_SampleID).rg;
#else
	depth = texture   (tex_depth, gl_FragCoord.xy).rg;
#endif
	if (gl_FragCoord.z != -depth.x && gl_FragCoord.z != depth.y)
		discard;
	if (gl_FragCoord.z == -depth.x) out_frag_blend_front.ra = compute(tex_blend_front);
	if (gl_FragCoord.z ==  depth.y) out_frag_blend_back.ra  = compute(tex_blend_back);
}
#endif