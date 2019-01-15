#include "define.h"
#extension GL_EXT_gpu_shader4 : enable

// IN
#if multisample
	uniform sampler2DMS	  tex_depth;
	uniform sampler2DMS	  tex_count;
	uniform sampler2DMS	  tex_id;
#else
	uniform sampler2DRect tex_depth;
	uniform sampler2DRect tex_count;
	uniform sampler2DRect tex_id;
#endif
// OUT
layout(location = 0, index = 0) out vec4 out_frag_count;
layout(location = 1, index = 0) out vec4 out_frag_id;

vec2 compute(const in float depth, const in int count, const in int id)
{
	vec2 frag_blend = vec2(0.0f);
	if(gl_FragCoord.z == depth)
	{
		if(count <= 1 || id > gl_PrimitiveID)
		{
			frag_blend.r = 1.0f;
			frag_blend.g = float(gl_PrimitiveID);
		}
	}
	return frag_blend;
}

void main(void)
{
	vec2   depth;
	ivec2  count,id;
#if multisample
	depth =		  texelFetch(tex_depth, ivec2(gl_FragCoord.xy), gl_SampleID).xy;
	count = ivec2(texelFetch(tex_count, ivec2(gl_FragCoord.xy), gl_SampleID).xy);
	id	  = ivec2(texelFetch(tex_id	  , ivec2(gl_FragCoord.xy), gl_SampleID).xy);
#else
	depth =		  texture(tex_depth, gl_FragCoord.xy).xy;
	count = ivec2(texture(tex_count, gl_FragCoord.xy).xy);
	id	  = ivec2(texture(tex_id   , gl_FragCoord.xy).xy);
#endif
	
	vec2 blend_front = compute(-depth.x, count.x, id.x);
	vec2 blend_back  = compute( depth.y, count.y, id.y);

	out_frag_count.rg= vec2(blend_front.x,blend_back.x);
	out_frag_id.rg 	 = vec2(blend_front.y,blend_back.y);
}