#include "define.h"

// IN
#if multisample

#if packing
	layout(binding = 0) uniform usampler2DMS   tex_color_front;
	layout(binding = 1) uniform usampler2DMS   tex_color_back;
#else
	layout(binding = 0) uniform  sampler2DMS   tex_color_front;
	layout(binding = 1) uniform  sampler2DMS   tex_color_back;
#endif
	layout(binding = 2) uniform  sampler2DMS   tex_depth;
	layout(binding = 3) uniform usampler2DMS   tex_lock_overlap;
#else

#if packing
	layout(binding = 0) uniform usampler2DRect tex_color_front;
	layout(binding = 1) uniform usampler2DRect tex_color_back;
#else
	layout(binding = 0) uniform  sampler2DRect tex_color_front;
	layout(binding = 1) uniform  sampler2DRect tex_color_back;
#endif
	layout(binding = 2) uniform  sampler2DRect tex_depth;
	layout(binding = 3) uniform usampler2DRect tex_lock_overlap;
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
		vec2 depth;
#if multisample
		depth = texelFetch(tex_depth, ivec2(gl_FragCoord.xy), gl_SampleID).rg;
#else
		depth = texture(tex_depth, gl_FragCoord.xy).rg;
#endif
		bool front = (depth.r <= depth.g) ? true : false;
		
		vec4 colorF,colorB;
#if packing
#if multisample
		colorF = unpackUnorm4x8(texelFetch(tex_color_front, ivec2(gl_FragCoord.xy), gl_SampleID).r);
		colorB = unpackUnorm4x8(texelFetch(tex_color_back , ivec2(gl_FragCoord.xy), gl_SampleID).r);
#else
		colorF = unpackUnorm4x8(texture(tex_color_front, gl_FragCoord.xy).r);
		colorB = unpackUnorm4x8(texture(tex_color_back , gl_FragCoord.xy).r);
#endif
#else
#if multisample
		colorF = texelFetch(tex_color_front, ivec2(gl_FragCoord.xy), gl_SampleID);
		colorB = texelFetch(tex_color_back , ivec2(gl_FragCoord.xy), gl_SampleID);
#else
		colorF = texture(tex_color_front, gl_FragCoord.xy);
		colorB = texture(tex_color_back , gl_FragCoord.xy);
#endif		
#endif
		vec4  color = front ? colorF : colorB;
		
		uvec2 O;
#if multisample
		O = texelFetch(tex_lock_overlap, ivec2(gl_FragCoord.xy), gl_SampleID).ga;
#else
		O = texture(tex_lock_overlap, gl_FragCoord.xy).ga;
#endif
		bool rule = checkRule(int(O.r) - int(O.g));

		vec4 c;
		if		( rule  && !front )  c.rgb = BLUE; 
		else if ( rule  &&  front )  c.rgb = CYAN; 
		else if (!rule  && !front )  c.rgb = PURPLE; 
		else						 c.rgb = YELLOW;

		c = vec4(c.rgb * color.rgb, color.a);
#if packing
		out_frag_color.r = packUnorm4x8(c);
#else
		out_frag_color   = c;
#endif
	}