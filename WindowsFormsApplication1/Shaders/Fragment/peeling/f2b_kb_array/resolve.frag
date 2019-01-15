#include "define.h"

// IN
uniform int    layer;
uniform int    bufferSize;
uniform bool   useTransparency;

#if multisample

#if packing
#else
	uniform sampler2DMSArray tex_peel;
#endif

#else

#if packing
#else
	uniform sampler2DArray tex_peel;
#endif

#endif

// OUT
#if packing
	layout(location = 0, index = 0) out uvec4 out_frag_color;
#else
	layout(location = 0, index = 0) out  vec4 out_frag_color;
#endif

#if packing
	uint  resolveAlphaBlend();
	uvec4 k[8];
#else
	vec4  resolveAlphaBlend();
	vec4  k[8];
#endif

void main(void)
{
#if multisample
	for(int i=0; i<bufferSize; i++)
		k[i] = texelFetch(tex_peel, ivec3(gl_FragCoord.xy, i), gl_SampleID);
#else
	for(int i=0; i<bufferSize; i++)
		k[i] = texture	 (tex_peel, vec3(gl_FragCoord.xy, float(i)));
#endif

#if packing
	out_frag_color.r = (useTransparency) ? resolveAlphaBlend() : k[layer/4][layer%4];
#else
	out_frag_color   = (useTransparency) ? resolveAlphaBlend() : k[layer];
#endif		
}

#if packing
uint resolveAlphaBlend()
{
	vec4 color,finalColor = vec4(0.0f);
	for(int i=0; i<bufferSize; i++)
		for(int j=0; j<4; j++)
		{
			color=unpackUnorm4x8(k[i][j]);
			if(color.a == 0.0f)
				return packUnorm4x8(finalColor);
			finalColor += color*(1.0f-finalColor.a);
		}
	return packUnorm4x8(finalColor);
}
#else
vec4 resolveAlphaBlend()
{
	vec4 finalColor = vec4(0.0f);
	for(int i=0; i<bufferSize; i++)
	{
		k[i].a = fract(k[i].a); // code for sorted fragments
		if(k[i].a > 0.0f)
			finalColor += k[i]*(1.0f-finalColor.a);
		else
			break;
	}
	return finalColor;
}
#endif