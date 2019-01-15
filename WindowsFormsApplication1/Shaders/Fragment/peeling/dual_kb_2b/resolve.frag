#include "define.h"
// IN
uniform int    layer;
//uniform int    bufferSize;
uniform bool   front;
uniform bool   useTransparency;
uniform float  transparency;

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

vec4 resolveAlphaBlend();
#if packing || sorted_by_id
	uvec4 k[8];
#else
	vec4  k[8];
#endif

void main(void)
{
#if multisample
	k[0] = texelFetch(tex_peel_0, ivec2(gl_FragCoord.xy), gl_SampleID);
	k[1] = texelFetch(tex_peel_1, ivec2(gl_FragCoord.xy), gl_SampleID);
	k[2] = texelFetch(tex_peel_2, ivec2(gl_FragCoord.xy), gl_SampleID);
	k[3] = texelFetch(tex_peel_3, ivec2(gl_FragCoord.xy), gl_SampleID);
	k[4] = texelFetch(tex_peel_4, ivec2(gl_FragCoord.xy), gl_SampleID);
	k[5] = texelFetch(tex_peel_5, ivec2(gl_FragCoord.xy), gl_SampleID);
	k[6] = texelFetch(tex_peel_6, ivec2(gl_FragCoord.xy), gl_SampleID);
	k[7] = texelFetch(tex_peel_7, ivec2(gl_FragCoord.xy), gl_SampleID);
#else
	k[0] = texture(tex_peel_0, gl_FragCoord.xy);
	k[1] = texture(tex_peel_1, gl_FragCoord.xy);
	k[2] = texture(tex_peel_2, gl_FragCoord.xy);
	k[3] = texture(tex_peel_3, gl_FragCoord.xy);
	k[4] = texture(tex_peel_4, gl_FragCoord.xy);
	k[5] = texture(tex_peel_5, gl_FragCoord.xy);
	k[6] = texture(tex_peel_6, gl_FragCoord.xy);
	k[7] = texture(tex_peel_7, gl_FragCoord.xy);
#endif

#if packing
	out_frag_color = (useTransparency) ? resolveAlphaBlend() : unpackUnorm4x8(k[layer/4][layer%4]);
#elif sorted_by_id
	out_frag_color = (useTransparency) ? resolveAlphaBlend() : unpackUnorm4x8(k[layer].r);
#else
	out_frag_color = (useTransparency) ? resolveAlphaBlend() : k[layer];
#endif
}

vec4 resolveAlphaBlend()
{
#if packing
	bool b=false;
#endif
	vec4 color, finalColor = vec4(0.0f);

	for(int i=0; i<KB_SIZE; i++)
	{
#if packing
		for(int j=0; j<4; j++)
		{
			color = unpackUnorm4x8(k[i][j]);
#else

#if sorted_by_id
			color = unpackUnorm4x8(k[i].r); 
#else
			color = k[i];
#endif

#endif
			if(color.a == 0.0f)
			{
#if packing
				b = true;
#endif
				break;
			}
#if packing
			color.a = transparency;
#endif
			if(front)
				finalColor += color*(1.0f-finalColor.a);
			else
			{
				finalColor *= (1.0f-finalColor.a);
				finalColor += color;
			}
#if packing
		}
		if(b)
			break;
#endif
	}
	return finalColor;
}