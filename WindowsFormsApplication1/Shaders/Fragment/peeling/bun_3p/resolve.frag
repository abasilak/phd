#include "define.h"

// IN
uniform int   layer;
uniform int   useMax;
//uniform int   bufferSize;
uniform bool  useTransparency;
uniform float transparency;

#if multisample
	layout(binding = 0) uniform sampler2DMS tex_peel_0;
	layout(binding = 1) uniform sampler2DMS tex_peel_1;
	layout(binding = 2) uniform sampler2DMS tex_peel_2;
	layout(binding = 3) uniform sampler2DMS tex_peel_3;
	layout(binding = 4) uniform sampler2DMS tex_peel_4;
	layout(binding = 5) uniform sampler2DMS tex_peel_5;
	layout(binding = 6) uniform sampler2DMS tex_peel_6;
	layout(binding = 7) uniform sampler2DMS tex_peel_7;
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

layout(location = 0, index = 0) out vec4 out_frag_color;

void main(void)
{
	vec2 k[8];
#if multisample
	k[0] = texelFetch(tex_peel_0, ivec2(gl_FragCoord.xy), gl_SampleID).rg;
	k[1] = texelFetch(tex_peel_1, ivec2(gl_FragCoord.xy), gl_SampleID).rg;
	k[2] = texelFetch(tex_peel_2, ivec2(gl_FragCoord.xy), gl_SampleID).rg;
	k[3] = texelFetch(tex_peel_3, ivec2(gl_FragCoord.xy), gl_SampleID).rg;
	k[4] = texelFetch(tex_peel_4, ivec2(gl_FragCoord.xy), gl_SampleID).rg;
	k[5] = texelFetch(tex_peel_5, ivec2(gl_FragCoord.xy), gl_SampleID).rg;
	k[6] = texelFetch(tex_peel_6, ivec2(gl_FragCoord.xy), gl_SampleID).rg;
	k[7] = texelFetch(tex_peel_7, ivec2(gl_FragCoord.xy), gl_SampleID).rg;
#else
	k[0] = texture(tex_peel_0, gl_FragCoord.xy).rg;
	k[1] = texture(tex_peel_1, gl_FragCoord.xy).rg;
	k[2] = texture(tex_peel_2, gl_FragCoord.xy).rg;
	k[3] = texture(tex_peel_3, gl_FragCoord.xy).rg;
	k[4] = texture(tex_peel_4, gl_FragCoord.xy).rg;
	k[5] = texture(tex_peel_5, gl_FragCoord.xy).rg;
	k[6] = texture(tex_peel_6, gl_FragCoord.xy).rg;
	k[7] = texture(tex_peel_7, gl_FragCoord.xy).rg;
#endif

	if(useTransparency)
	{
		vec4 color, finalColor=vec4(0.0f);
		for(int i=0; i<KB_SIZE; i++)
		{
			for(int j=0; j<2; j++)
			{
				if(k[i][j] == 0.0f) continue;
				color = vec4(unpackUnorm4x8(uint(k[i][j])).abg, transparency);
				
				if(j%2 == 0)
					finalColor += color*(1.0f-finalColor.a);
				else
				{
					finalColor *= (1.0f-finalColor.a);
					finalColor += color;
				}
			}
			//finalColor += unpackUnorm4x8(uint(k[i].r))*(1.0f-finalColor.a);
			//finalColor += unpackUnorm4x8(uint(k[i].g))*(1.0f-finalColor.a);
		}
		out_frag_color = finalColor;
	}
	else
		out_frag_color = unpackUnorm4x8(uint(k[layer][useMax])).abgr;
}
