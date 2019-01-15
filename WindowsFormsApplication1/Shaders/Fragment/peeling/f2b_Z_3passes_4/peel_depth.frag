#include "define.h"
// IN
#if multisample
	uniform sampler2DMS	  tex_depth;
	uniform sampler2DMS	  tex_count;
#else
	uniform sampler2DRect tex_depth;
	uniform sampler2DRect tex_count;
#endif

void main(void)
{
	int   count;
	float depth;
#if multisample
	depth =		texelFetch(tex_depth, ivec2(gl_FragCoord.xy), gl_SampleID).r;
	count = int(texelFetch(tex_count, ivec2(gl_FragCoord.xy), gl_SampleID).r);
#else
	depth =		texture(tex_depth, gl_FragCoord.xy).r;
	count = int(texture(tex_count, gl_FragCoord.xy).r);
#endif
	if ((count <= 1 && gl_FragCoord.z <= depth) || (count >  1 && gl_FragCoord.z != depth))
		discard;
}