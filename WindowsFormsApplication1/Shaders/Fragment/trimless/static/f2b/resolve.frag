#include "define.h"

// IN
#if multisample

#if packing
	layout(binding = 0) uniform usampler2DMS   tex_colorF;
	layout(binding = 1) uniform usampler2DMS   tex_colorB;
#else
	layout(binding = 0) uniform  sampler2DMS   tex_colorF;
	layout(binding = 1) uniform  sampler2DMS   tex_colorB;
#endif
	layout(binding = 2) uniform  sampler2DMS   tex_depthF;
	layout(binding = 3) uniform  sampler2DMS   tex_depthB;
	layout(binding = 4) uniform usampler2DMS   tex_lock_overlapF;
	layout(binding = 5) uniform usampler2DMS   tex_lock_overlapB;
#else

#if packing
	layout(binding = 0) uniform usampler2DRect tex_colorF;
	layout(binding = 1) uniform usampler2DRect tex_colorB;
#else
	layout(binding = 0) uniform  sampler2DRect tex_colorF;
	layout(binding = 1) uniform  sampler2DRect tex_colorB;
#endif
	layout(binding = 2) uniform  sampler2DRect tex_depthF;
	layout(binding = 3) uniform  sampler2DRect tex_depthB;
	layout(binding = 4) uniform usampler2DRect tex_lock_overlapF;
	layout(binding = 5) uniform usampler2DRect tex_lock_overlapB;
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
		float depthF,depthB;
#if multisample
		depthF = texelFetch(tex_depthF, ivec2(gl_FragCoord.xy), gl_SampleID).r;
		depthB = texelFetch(tex_depthB, ivec2(gl_FragCoord.xy), gl_SampleID).r;
#else
		depthF = texture(tex_depthF, gl_FragCoord.xy).r;
		depthB = texture(tex_depthB, gl_FragCoord.xy).r;
#endif
		bool front = (depthF <= depthB) ? true : false;
		
		vec4 colorF,colorB;
#if packing
#if multisample
		colorF = unpackUnorm4x8(texelFetch(tex_colorF, ivec2(gl_FragCoord.xy), gl_SampleID).r);
		colorB = unpackUnorm4x8(texelFetch(tex_colorB, ivec2(gl_FragCoord.xy), gl_SampleID).r);
#else
		colorF = unpackUnorm4x8(texture(tex_colorF, gl_FragCoord.xy).r);
		colorB = unpackUnorm4x8(texture(tex_colorB, gl_FragCoord.xy).r);
#endif
#else
#if multisample
		colorF = texelFetch(tex_colorF, ivec2(gl_FragCoord.xy), gl_SampleID);
		colorB = texelFetch(tex_colorB, ivec2(gl_FragCoord.xy), gl_SampleID);
#else
		colorF = texture(tex_colorF, gl_FragCoord.xy);
		colorB = texture(tex_colorB, gl_FragCoord.xy);
#endif		
#endif
		vec4 color = front ? colorF : colorB;
		
		uint oF,oB;
#if multisample
		oF = texelFetch(tex_lock_overlapF, ivec2(gl_FragCoord.xy), gl_SampleID).g;
		oB = texelFetch(tex_lock_overlapB, ivec2(gl_FragCoord.xy), gl_SampleID).g;
#else
		oF = texture(tex_lock_overlapF, gl_FragCoord.xy).g;
		oB = texture(tex_lock_overlapB, gl_FragCoord.xy).g;
#endif
		bool rule = checkRule(int(oF) - int(oB));

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