#include "define.h"
#extension GL_EXT_gpu_shader4 : enable

// INCLUDE
vec4 computePixelColor();
// IN
#if multisample
	uniform sampler2DMS   tex_depth;
	uniform sampler2DMS   tex_count;
	uniform sampler2DMS   tex_id;
#else
	uniform sampler2DRect tex_depth;
	uniform sampler2DRect tex_count;
	uniform sampler2DRect tex_id;
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

	int	count,id;
#if multisample
	count = int(texelFetch(tex_count, ivec2(gl_FragCoord.xy), gl_SampleID).r);
	id    = int(texelFetch(tex_id   , ivec2(gl_FragCoord.xy), gl_SampleID).r);
#else
	count = int(texture(tex_count, gl_FragCoord.xy).r);
	id    = int(texture(tex_id   , gl_FragCoord.xy).r);
#endif
	out_frag_color	 = (gl_PrimitiveID == id) ? computePixelColor() : vec4(0.0);
	out_frag_depth.r = (count > 1 || gl_FragCoord.z != depth) ? -gl_FragCoord.z : -depth_max;
}