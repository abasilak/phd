#include "define.h"

// IN
#if multisample
	layout(binding = 0) uniform sampler2DMS	  tex_depth;
	layout(binding = 1) uniform sampler2DMS	  tex_count;
#else
	layout(binding = 0) uniform sampler2DRect tex_depth;
	layout(binding = 1) uniform sampler2DRect tex_count;
#endif

#if early_Z
	layout (depth_greater) out float gl_FragDepth;
#endif

void main(void)
{
	int   count;
	float depth;

#if multisample
	depth =		texelFetch(tex_depth, ivec2(gl_FragCoord.xy), gl_SampleID).r;

#if draw_buffers_blend
	count = int(texelFetch(tex_count, ivec2(gl_FragCoord.xy), gl_SampleID).r);
#else
	count = int(texelFetch(tex_count, ivec2(gl_FragCoord.xy), gl_SampleID).a);
#endif

#else
	depth =		texture(tex_depth, gl_FragCoord.xy).r;
#if draw_buffers_blend
	count = int(texture(tex_count, gl_FragCoord.xy).r);
#else
	count = int(texture(tex_count, gl_FragCoord.xy).a);
#endif

#endif
	if ((count <= 1 && gl_FragCoord.z <= depth) || (count > 1 && gl_FragCoord.z != depth))
		discard;
}