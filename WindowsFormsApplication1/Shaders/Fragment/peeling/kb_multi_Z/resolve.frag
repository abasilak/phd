#include "define.h"
#include "sort.h"
#include "resolve_b2.h"

#if multisample
	layout(binding = 0) uniform sampler2DMS	tex_peel_0;
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

// OUT
	layout(location = 0, index = 0) out  vec4 out_frag_color;

#if packing
	vec2 k[16];
#else
	vec3 k[8];
#endif

void main(void)
{
#if multisample
#if packing
	vec4 kp;
	kp = texelFetch(tex_peel_0, ivec2(gl_FragCoord.xy), gl_SampleID);
	k[0]  = kp.xy;
	k[1]  = kp.zw;
	kp = texelFetch(tex_peel_1, ivec2(gl_FragCoord.xy), gl_SampleID);
	k[2]  = kp.xy;
	k[3]  = kp.zw;
	kp = texelFetch(tex_peel_2, ivec2(gl_FragCoord.xy), gl_SampleID);
	k[4]  = kp.xy;
	k[5]  = kp.zw;
	kp = texelFetch(tex_peel_3, ivec2(gl_FragCoord.xy), gl_SampleID);
	k[6]  = kp.xy;
	k[7]  = kp.zw;
	kp = texelFetch(tex_peel_4, ivec2(gl_FragCoord.xy), gl_SampleID);
	k[8]  = kp.xy;
	k[9]  = kp.zw;
	kp = texelFetch(tex_peel_5, ivec2(gl_FragCoord.xy), gl_SampleID);
	k[10] = kp.xy;
	k[11] = kp.zw;
	kp = texelFetch(tex_peel_6, ivec2(gl_FragCoord.xy), gl_SampleID);
	k[12] = kp.xy;
	k[13] = kp.zw;
	kp = texelFetch(tex_peel_7, ivec2(gl_FragCoord.xy), gl_SampleID);
	k[14] = kp.xy;
	k[15] = kp.zw;
#else
	k[0] = texelFetch(tex_peel_0, ivec2(gl_FragCoord.xy), gl_SampleID).rgb;
	k[1] = texelFetch(tex_peel_1, ivec2(gl_FragCoord.xy), gl_SampleID).rgb;
	k[2] = texelFetch(tex_peel_2, ivec2(gl_FragCoord.xy), gl_SampleID).rgb;
	k[3] = texelFetch(tex_peel_3, ivec2(gl_FragCoord.xy), gl_SampleID).rgb;
	k[4] = texelFetch(tex_peel_4, ivec2(gl_FragCoord.xy), gl_SampleID).rgb;
	k[5] = texelFetch(tex_peel_5, ivec2(gl_FragCoord.xy), gl_SampleID).rgb;
	k[6] = texelFetch(tex_peel_6, ivec2(gl_FragCoord.xy), gl_SampleID).rgb;
	k[7] = texelFetch(tex_peel_7, ivec2(gl_FragCoord.xy), gl_SampleID).rgb;
#endif
#else
#if packing
	vec4 kp;
	kp = texture(tex_peel_0, gl_FragCoord.xy);
	k[0]  = kp.xy;
	k[1]  = kp.zw;
	kp = texture(tex_peel_1, gl_FragCoord.xy);
	k[2]  = kp.xy;
	k[3]  = kp.zw;
	kp = texture(tex_peel_2, gl_FragCoord.xy);
	k[4]  = kp.xy;
	k[5]  = kp.zw;
	kp = texture(tex_peel_3, gl_FragCoord.xy);
	k[6]  = kp.xy;
	k[7]  = kp.zw;
	kp = texture(tex_peel_4, gl_FragCoord.xy);
	k[8]  = kp.xy;
	k[9]  = kp.zw;
	kp = texture(tex_peel_5, gl_FragCoord.xy);
	k[10] = kp.xy;
	k[11] = kp.zw;
	kp = texture(tex_peel_6, gl_FragCoord.xy);
	k[12] = kp.xy;
	k[13] = kp.zw;
	kp = texture(tex_peel_7, gl_FragCoord.xy);
	k[14] = kp.xy;
	k[15] = kp.zw;
#else
	k[0] = texture(tex_peel_0, gl_FragCoord.xy).rgb;
	k[1] = texture(tex_peel_1, gl_FragCoord.xy).rgb;
	k[2] = texture(tex_peel_2, gl_FragCoord.xy).rgb;
	k[3] = texture(tex_peel_3, gl_FragCoord.xy).rgb;
	k[4] = texture(tex_peel_4, gl_FragCoord.xy).rgb;
	k[5] = texture(tex_peel_5, gl_FragCoord.xy).rgb;
	k[6] = texture(tex_peel_6, gl_FragCoord.xy).rgb;
	k[7] = texture(tex_peel_7, gl_FragCoord.xy).rgb; 
#endif

#endif

	int SIZE, counter=0;

#if packing
	SIZE = 2*KB_SIZE;
#else
	SIZE =   KB_SIZE;
#endif

	for(int i=0; i<SIZE; i++)
	{
		if(k[i].g == 1.0f)
			break;
		
		fragments[i] = vec2(i,fract(k[i].g));
		colors[i]	 = floatBitsToUint(k[i].r);
		counter++;
	}

	out_frag_color = resolve(counter, false);
}