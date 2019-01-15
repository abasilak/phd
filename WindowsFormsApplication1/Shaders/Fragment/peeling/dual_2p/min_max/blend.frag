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

#if draw_buffers_blend

// OUT
layout(location = 0, index = 0) out vec4 out_frag_id;
layout(location = 1, index = 0) out vec4 out_frag_count;

vec3 compute(const in float depth, const in int count, const in ivec2 id)
{
	return (depth == gl_FragCoord.z && (count <= 2 || (-id.x < gl_PrimitiveID && id.y > gl_PrimitiveID))) ? vec3(-gl_PrimitiveID, gl_PrimitiveID, 1.0f) : vec3(float_min, 0.0f, 0.0f);
}

void main(void)
{
	out_frag_id 	  = vec4(float_min, 0.0f, float_min, 0.0f);
	out_frag_count.rg = vec2(0.0);

	vec2 depth;
#if multisample
	depth =	texelFetch(tex_depth, ivec2(gl_FragCoord.xy), gl_SampleID).xy;
#else
	depth =	texture(tex_depth, gl_FragCoord.xy).xy;
#endif
	if (gl_FragCoord.z != -depth.x && gl_FragCoord.z != depth.y)
		discard;
	
	ivec2 count;
	ivec4 id;

#if multisample
	id	  = ivec4(texelFetch(tex_id	  , ivec2(gl_FragCoord.xy), gl_SampleID));
	count = ivec2(texelFetch(tex_count, ivec2(gl_FragCoord.xy), gl_SampleID).xy);
#else
	id	  = ivec4(texture(tex_id   , gl_FragCoord.xy));
	count = ivec2(texture(tex_count, gl_FragCoord.xy).xy);
#endif
	vec3 blend_front = compute(-depth.x, count.x, id.xy);
	vec3 blend_back  = compute( depth.y, count.y, id.zw);

	out_frag_id 	  = vec4(blend_front.xy, blend_back.xy);
	out_frag_count.rg = vec2(blend_front.z, blend_back.z);
}

#else

// OUT
layout(location = 0, index = 0) out vec4 out_frag_blend_front;
layout(location = 1, index = 0) out vec4 out_frag_blend_back;

vec3 compute(const in sampler2DRect tex)
{
	ivec3 CountID;
#if multisample
	CountID = ivec3(texelFetch(tex, ivec2(gl_FragCoord.xy), gl_SampleID).rga;
#else
	CountID = ivec3(   texture(tex, gl_FragCoord.xy).rga);
#endif
	return (CountID.z <= 2 || (-CountID.x < gl_PrimitiveID && CountID.y > gl_PrimitiveID)) ? vec3(-gl_PrimitiveID, gl_PrimitiveID, 1.0f) : vec3(float_min, 0.0f, 0.0f);
}

void main(void)
{
	out_frag_blend_front.rga = vec3(float_min, 0.0f, 0.0f);
	out_frag_blend_back.rga  = vec3(float_min, 0.0f, 0.0f);

	vec2  depth;
#if multisample
	depth =	texelFetch(tex_depth, ivec2(gl_FragCoord.xy), gl_SampleID).rg;
#else
	depth =	texture	  (tex_depth, gl_FragCoord.xy).rg;
#endif
	if (gl_FragCoord.z != -depth.x && gl_FragCoord.z != depth.y)
		discard;
	if (gl_FragCoord.z == -depth.x) out_frag_blend_front.rga = compute(tex_blend_front);
	if (gl_FragCoord.z ==  depth.y) out_frag_blend_back.rga  = compute(tex_blend_back);
}

#endif