#include "define.h"

// IN
	uniform int	layers;
#if multisample
	layout(binding = 0) uniform sampler2DMS   tex_depth;
	layout(binding = 1) uniform sampler2DMS   tex_thickness;
#else
	layout(binding = 0) uniform sampler2DRect tex_depth;
	layout(binding = 1) uniform sampler2DRect tex_thickness;
#endif
// OUT
	layout(location = 0, index = 0) out vec4 out_frag_thickness;

void main(void)
{
	vec2  depth;
	vec3  thickness;
	ivec2 coords = ivec2(gl_FragCoord.xy);

	// r: final, g: prev_near, b:prev_far
#if multisample
	thickness = texelFetch(tex_thickness, coords, gl_SampleID).xyz;
	depth     = texelFetch(tex_depth	, coords, gl_SampleID).xy ;
#else
	thickness = texture(tex_thickness, gl_FragCoord.xy).xyz;
	depth	  = texture(tex_depth    , gl_FragCoord.xy).xy ;
#endif
	float depth_near = abs(depth.x);
	float depth_far	 = abs(depth.y);

	if(layers == 1)
	{
		if(depth_near != 1.0f					   ) thickness.r += depth_near  - thickness.g;
		if(depth_far  != 1.0f					   ) thickness.r += thickness.b - depth_far;
		if(depth_near == 1.0f && depth_far  == 1.0f) thickness.r += thickness.b - thickness.g;
	}
	out_frag_thickness.rgb = vec3(thickness.r, depth_near, depth_far);
}