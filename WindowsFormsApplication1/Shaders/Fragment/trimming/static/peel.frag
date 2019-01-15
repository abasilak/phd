#include "define.h"

// Include
vec4 computePixelColor();
// IN
uniform bool  first;
uniform float cappingPlane;
uniform float cappingAngle;

#if multisample
	layout(binding = 0) uniform sampler2DMS   tex_depth;
	layout(binding = 1) uniform sampler2DMS   tex_lock;
#else
	layout(binding = 0) uniform sampler2DRect tex_depth;
	layout(binding = 1) uniform sampler2DRect tex_lock;
#endif

// OUT
#if packing
	layout(location = 0, index = 0) out uvec4 out_frag_color;
#else
	layout(location = 0, index = 0) out  vec4 out_frag_color;
#endif
	layout(location = 1, index = 0) out uvec4 out_frag_facing;

void main(void)
{
#if multisample
	if(texelFetch(tex_lock, ivec2(gl_FragCoord.xy), gl_SampleID).a > 0.0f)
#else
	if(texture(tex_lock, gl_FragCoord.xy).a > 0.0f)
#endif
		discard;

	float depth;
	if(first)
	{
		float k = 1.0f - float(gl_FragCoord.x)/cappingAngle;
		depth   = cappingPlane*k;
	}
	else
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
	out_frag_facing.r = gl_FrontFacing ? 1U : 0U;
}