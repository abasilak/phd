#include "define.h"

// Include
	vec4 computePixelColor();
// IN
	uniform float cappingPlane;
	uniform float cappingAngle;

#if multisample
	layout(binding = 0) uniform  sampler2DMS   tex_overlap;
#else
	layout(binding = 0) uniform  sampler2DRect tex_overlap;
#endif
// OUT
#if packing
	layout(location = 0, index = 0) out uvec4 out_frag_color;
#else
	layout(location = 0, index = 0) out  vec4 out_frag_color;
#endif

	const vec3 BLUE		= vec3(0.0,0.0,1.0); 
	const vec3 CYAN		= vec3(0.0,1.0,1.0); 
	const vec3 PURPLE	= vec3(1.0,0.5,0.5); 
	const vec3 YELLOW	= vec3(1.0,1.0,0.0); 

	bool checkRule(const int value)
	{
		return (int(abs(ceil(float(value)*0.5f)))%2 == 1) ? true : false;
	}

void main(void)
{
	float k = 1.0f - float(gl_FragCoord.x)/cappingAngle;
	float capping = cappingPlane*k;

	if(gl_FragCoord.z <= capping)
		discard;

	float O;
#if multisample
	O = texelFetch(tex_overlap, ivec2(gl_FragCoord.xy), gl_SampleID).r;
#else
	O = texture	  (tex_overlap, gl_FragCoord.xy).r;
#endif
	bool rule = checkRule(int(O));

	vec4 c,color = computePixelColor();
	if		( rule  && !gl_FrontFacing ) c.rgb = BLUE; 
	else if ( rule  &&  gl_FrontFacing ) c.rgb = CYAN; 
	else if (!rule  && !gl_FrontFacing ) c.rgb = PURPLE; 
	else								 c.rgb = YELLOW;

	c = vec4(c.rgb * color.rgb, color.a);
#if packing
	out_frag_color.r = packUnorm4x8(c);
#else
	out_frag_color   = c;
#endif
}