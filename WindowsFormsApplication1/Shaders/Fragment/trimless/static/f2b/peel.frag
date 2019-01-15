#include "define.h"

// Include
vec4 computePixelColor();
// IN
uniform bool back_facing;

#if multisample
	layout(binding = 0) uniform sampler2DMS   tex_depth;
#else
	layout(binding = 0) uniform sampler2DRect tex_depth;
#endif
// OUT
#if packing
	layout(location = 0, index = 0) out uvec4 out_frag_color;
#else
	layout(location = 0, index = 0) out  vec4 out_frag_color;
#endif

#if early_Z
	layout (depth_greater) out float gl_FragDepth;
#endif

void main(void)
{
	if(back_facing == gl_FrontFacing)
		discard;

	float depth;
#if multisample
	depth = texelFetch(tex_depth, ivec2(gl_FragCoord.xy), gl_SampleID).r;
#else
	depth = texture(tex_depth, gl_FragCoord.xy).r;
#endif
	if(gl_FragCoord.z <= depth)
		discard;

	vec4 c = computePixelColor();
#if packing
	out_frag_color.r = packUnorm4x8(c);
#else
	out_frag_color   = c;
#endif
}