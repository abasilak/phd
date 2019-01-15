#include "define.h"

// IN
uniform int    layer;
uniform float  transparency;
uniform bool   useTransparency;
uniform vec4   color_background;

#if multisample

#if packing || sorted_by_id
	layout(binding = 0) uniform usampler2DMS tex_peel_0;
	layout(binding = 1) uniform usampler2DMS tex_peel_1;
	layout(binding = 2) uniform usampler2DMS tex_peel_2;
	layout(binding = 3) uniform usampler2DMS tex_peel_3;
	layout(binding = 4) uniform usampler2DMS tex_peel_4;
	layout(binding = 5) uniform usampler2DMS tex_peel_5;
	layout(binding = 6) uniform usampler2DMS tex_peel_6;
	layout(binding = 7) uniform usampler2DMS tex_peel_7;
#else
	layout(binding = 0) uniform sampler2DMS	tex_peel_0;
	layout(binding = 1) uniform sampler2DMS tex_peel_1;
	layout(binding = 2) uniform sampler2DMS tex_peel_2;
	layout(binding = 3) uniform sampler2DMS tex_peel_3;
	layout(binding = 4) uniform sampler2DMS tex_peel_4;
	layout(binding = 5) uniform sampler2DMS tex_peel_5;
	layout(binding = 6) uniform sampler2DMS tex_peel_6;
	layout(binding = 7) uniform sampler2DMS tex_peel_7;
#endif

#else

#if packing || sorted_by_id
	layout(binding = 0) uniform usampler2DRect tex_peel_0;
	layout(binding = 1) uniform usampler2DRect tex_peel_1;
	layout(binding = 2) uniform usampler2DRect tex_peel_2;
	layout(binding = 3) uniform usampler2DRect tex_peel_3;
	layout(binding = 4) uniform usampler2DRect tex_peel_4;
	layout(binding = 5) uniform usampler2DRect tex_peel_5;
	layout(binding = 6) uniform usampler2DRect tex_peel_6;
	layout(binding = 7) uniform usampler2DRect tex_peel_7;
#else
	layout(binding = 0) uniform sampler2DRect tex_peel_0;
	layout(binding = 1) uniform sampler2DRect tex_peel_1;
	layout(binding = 2) uniform sampler2DRect tex_peel_2;
	layout(binding = 3) uniform sampler2DRect tex_peel_3;
	layout(binding = 4) uniform sampler2DRect tex_peel_4;
	layout(binding = 5) uniform sampler2DRect tex_peel_5;
	layout(binding = 6) uniform sampler2DRect tex_peel_6;
	layout(binding = 7) uniform sampler2DRect tex_peel_7;
#endif

#endif

// OUT
layout(location = 0, index = 0) out vec4 out_frag_color;

vec4  resolveAlphaBlend(const int count);

#if packing || sorted_by_id
	uvec4 k[8];
#else
	vec4  k[8];
#endif

void main(void)
{
#if packing
	bool b;
#endif
	int count = KB_SIZE;
	for(int i=0; i<KB_SIZE; i++)
	{
#if multisample
		if		(i==0) k[0] = texelFetch(tex_peel_0, ivec2(gl_FragCoord.xy), gl_SampleID);
		else if (i==1) k[1] = texelFetch(tex_peel_1, ivec2(gl_FragCoord.xy), gl_SampleID);
		else if (i==2) k[2] = texelFetch(tex_peel_2, ivec2(gl_FragCoord.xy), gl_SampleID);
		else if (i==3) k[3] = texelFetch(tex_peel_3, ivec2(gl_FragCoord.xy), gl_SampleID);
		else if (i==4) k[4] = texelFetch(tex_peel_4, ivec2(gl_FragCoord.xy), gl_SampleID);
		else if (i==5) k[5] = texelFetch(tex_peel_5, ivec2(gl_FragCoord.xy), gl_SampleID);
		else if (i==6) k[6] = texelFetch(tex_peel_6, ivec2(gl_FragCoord.xy), gl_SampleID);
		else		   k[7] = texelFetch(tex_peel_7, ivec2(gl_FragCoord.xy), gl_SampleID);
#else
		if		(i==0) k[0] = texture(tex_peel_0, gl_FragCoord.xy);
		else if (i==1) k[1] = texture(tex_peel_1, gl_FragCoord.xy);
		else if (i==2) k[2] = texture(tex_peel_2, gl_FragCoord.xy);
		else if (i==3) k[3] = texture(tex_peel_3, gl_FragCoord.xy);
		else if (i==4) k[4] = texture(tex_peel_4, gl_FragCoord.xy);
		else if (i==5) k[5] = texture(tex_peel_5, gl_FragCoord.xy);
		else if (i==6) k[6] = texture(tex_peel_6, gl_FragCoord.xy);
		else		   k[7] = texture(tex_peel_7, gl_FragCoord.xy);
#endif

#if packing
		b=false;
		for(int j=0; j<4; j++)
			if(unpackUnorm4x8(k[i][j]).r == 0U)
			{
				b=true;
				break;
			}
		if(b)
			break;

#elif sorted_by_id
		if(unpackUnorm4x8(k[i][0]).r == 0U)
#else
		if(k[i].a == 0.0f)
#endif
		{
			count = i;
			break;
		}
	}

	for(int i=count; i<KB_SIZE; i++)
#if packing
		for(int j=0; j<4; j++)
			k[i][j] = packUnorm4x8(color_background.abgr);
#elif sorted_by_id
		k[i].r = packUnorm4x8(color_background.abgr);
#else
		k[i] = color_background;
#endif

#if packing
	out_frag_color = (useTransparency) ? resolveAlphaBlend(count) : unpackUnorm4x8(k[layer/4][layer%4]).abgr;
#elif sorted_by_id
	out_frag_color = (useTransparency) ? resolveAlphaBlend(count) : unpackUnorm4x8(k[layer].r).abgr;
#else
	out_frag_color = (useTransparency) ? resolveAlphaBlend(count) : k[layer];
#endif
}

#if packing
vec4 resolveAlphaBlend(const int count)
{
	vec4 color,finalColor = vec4(0.0f);
	for(int i=0; i<count; i++)
		for(int j=0; j<4; j++)
		{
			color = unpackUnorm4x8(k[i][j]).abgr;
			if(color.a == 0.0f)
				return finalColor;
			color.a = transparency;
			finalColor += color*(1.0f-finalColor.a);
		}
	return finalColor;
}
#else
vec4 resolveAlphaBlend(const int count)
{
	vec4 c,finalColor = vec4(0.0f);
	for(int i=0; i<count; i++)
	{
#if sorted_by_id
		c = unpackUnorm4x8(k[i].r).abgr;
#else
		c = k[i];
#endif
		if(c.a > 0.0f)
			finalColor += c*(1.0f-finalColor.a);
		else
			break;
	}
	return finalColor;
}
#endif