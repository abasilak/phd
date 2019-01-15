#include "define.h"

// Include
vec4 computePixelColor();

// IN
#if multisample
	layout(binding = 0) uniform sampler2DMS	 tex_depth;
#else
	layout(binding = 0) uniform sampler2DRect tex_depth;
#endif

// OUT
	layout(location = 0, index = 0) out  vec4 out_frag_color;

#if early_Z
	layout (depth_greater) out float gl_FragDepth;
#endif

void main(void)
{
	float depth;
#if multisample
	depth = texelFetch(tex_depth, ivec2(gl_FragCoord.xy), gl_SampleID).r;
#else
	depth = texture	  (tex_depth, gl_FragCoord.xy).r;
#endif
	if(gl_FragCoord.z <= depth)
		discard;
	out_frag_color = computePixelColor();
}