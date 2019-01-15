#include "define.h"

// IN
uniform int    layer;
uniform int    size;
uniform float  transparency;
uniform bool   correctAlpha;
uniform bool   useTransparency;

uniform sampler2DRect tex_peel_0;
uniform sampler2DRect tex_peel_1;
uniform sampler2DRect tex_peel_2;
uniform sampler2DRect tex_peel_3;
uniform sampler2DRect tex_peel_4;
uniform sampler2DRect tex_peel_5;
uniform sampler2DRect tex_peel_6;
uniform sampler2DRect tex_peel_7;

// OUT
layout(location = 0, index = 0) out vec4 out_frag_color;

vec4 resolveAlphaBlend();
vec4 k[8];

void main(void)
{
	k[0] = texture(tex_peel_0, gl_FragCoord.xy);
	k[1] = texture(tex_peel_1, gl_FragCoord.xy);
	k[2] = texture(tex_peel_2, gl_FragCoord.xy);
	k[3] = texture(tex_peel_3, gl_FragCoord.xy);
	k[4] = texture(tex_peel_4, gl_FragCoord.xy);
	k[5] = texture(tex_peel_5, gl_FragCoord.xy);
	k[6] = texture(tex_peel_6, gl_FragCoord.xy);
	k[7] = texture(tex_peel_7, gl_FragCoord.xy);
		
	out_frag_color   = (useTransparency) ? resolveAlphaBlend() : vec4(k[layer/4][layer%4]);
}

vec4 resolveAlphaBlend()
{
	bool  entered=false;
	float thickness=0.0f;
	vec4  color, finalColor = vec4(0.0f);

	for(int i=0; i<size; i++)
	{
		if(k[i].a == 0.0f) continue;

		if(correctAlpha && !entered)
		{
			entered = true;
			thickness = k[i].w*0.5f;
		}
		color = vec4(k[i].rgb,transparency);
		if(correctAlpha)
		{
			if(i%2 == size%2)
				thickness=(k[i+1].w-k[i].w)*0.5f;
			color.w = 1.0f - pow(1.0f - color.w, thickness*sigma);
			color.rgb *= color.a;
		}
		finalColor += color*(1.0f-finalColor.a);
	}
	return finalColor;
}