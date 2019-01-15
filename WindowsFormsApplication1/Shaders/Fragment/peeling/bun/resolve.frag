#include "define.h"
// IN
uniform int    layer;
uniform bool   useFront;
uniform bool   useTransparency;
uniform float  transparency;

#if multisample
	layout(binding = 0) uniform sampler2DMS	tex_peel_0;
	layout(binding = 1) uniform sampler2DMS tex_peel_1;
	layout(binding = 2) uniform sampler2DMS tex_peel_2;
	layout(binding = 3) uniform sampler2DMS tex_peel_3;
#if !multipass
	layout(binding = 4) uniform sampler2DMS	tex_peel_4;
	layout(binding = 5) uniform sampler2DMS tex_peel_5;
	layout(binding = 6) uniform sampler2DMS tex_peel_6;
	layout(binding = 7) uniform sampler2DMS tex_peel_7;
#endif
#else
	layout(binding = 0) uniform sampler2DRect tex_peel_0;
	layout(binding = 1) uniform sampler2DRect tex_peel_1;
	layout(binding = 2) uniform sampler2DRect tex_peel_2;
	layout(binding = 3) uniform sampler2DRect tex_peel_3;
#if !multipass
	layout(binding = 4) uniform sampler2DRect tex_peel_4;
	layout(binding = 5) uniform sampler2DRect tex_peel_5;
	layout(binding = 6) uniform sampler2DRect tex_peel_6;
	layout(binding = 7) uniform sampler2DRect tex_peel_7;
#endif
#endif

// OUT
	layout(location = 0, index = 0) out vec4 out_frag_color;

#if multipass
	vec4 k[4];
#else
	vec4 k[8];
#endif
vec4 resolveAlphaBlend();

void main(void)
{
#if multisample
	k[0] = texelFetch(tex_peel_0, ivec2(gl_FragCoord.xy), gl_SampleID);
	k[1] = texelFetch(tex_peel_1, ivec2(gl_FragCoord.xy), gl_SampleID);
	k[2] = texelFetch(tex_peel_2, ivec2(gl_FragCoord.xy), gl_SampleID);
	k[3] = texelFetch(tex_peel_3, ivec2(gl_FragCoord.xy), gl_SampleID);
#if !multipass
	k[4] = texelFetch(tex_peel_4, ivec2(gl_FragCoord.xy), gl_SampleID);
	k[5] = texelFetch(tex_peel_5, ivec2(gl_FragCoord.xy), gl_SampleID);
	k[6] = texelFetch(tex_peel_6, ivec2(gl_FragCoord.xy), gl_SampleID);
	k[7] = texelFetch(tex_peel_7, ivec2(gl_FragCoord.xy), gl_SampleID);
#endif
#else
	k[0] = texture(tex_peel_0, gl_FragCoord.xy);
	k[1] = texture(tex_peel_1, gl_FragCoord.xy);
	k[2] = texture(tex_peel_2, gl_FragCoord.xy);
	k[3] = texture(tex_peel_3, gl_FragCoord.xy);
#if !multipass
	k[4] = texture(tex_peel_4, gl_FragCoord.xy);
	k[5] = texture(tex_peel_5, gl_FragCoord.xy);
	k[6] = texture(tex_peel_6, gl_FragCoord.xy);
	k[7] = texture(tex_peel_7, gl_FragCoord.xy);
#endif
#endif
	
#if multipass
	out_frag_color = (useTransparency) ? resolveAlphaBlend() : unpackUnorm4x8(uint(k[layer/2][2*(layer%2) + int(useFront)])).abgr;
#else
	out_frag_color = (useTransparency) ? resolveAlphaBlend() : unpackUnorm4x8(uint(k[layer/4][layer%4])).abgr;
#endif
}

vec4 resolveAlphaBlend()
{
	vec4 color, finalColor = vec4(0.0f);
#if multipass
	for(int i=0; i<4; i++)
#else
	for(int i=0; i<8; i++)
#endif
	{
		for(int j=0; j<4; j++)
		{
			if(k[i][j] == 0.0f) continue;
			color = vec4(unpackUnorm4x8(uint(k[i][j])).abg, transparency);
#if multipass
			if(j%2 == 0)
				finalColor += color*(1.0f-finalColor.a);
			else
			{
				finalColor *= (1.0f-finalColor.a);
				finalColor += color;
			}
#else
			finalColor += color*(1.0f-finalColor.a);
#endif
		}
	}
	return finalColor;
}