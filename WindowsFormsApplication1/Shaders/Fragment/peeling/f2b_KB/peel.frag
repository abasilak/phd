#include "define.h"
#extension GL_EXT_gpu_shader4 : enable

// INCLUDE
vec4 computePixelColor();
// IN
#if multisample
#if packing	|| sorted_by_id
	layout(binding = 0) uniform usampler2DMS tex_peel_0;
	layout(binding = 1) uniform usampler2DMS tex_peel_1;
	layout(binding = 4) uniform usampler2DMS tex_peel_2;
	layout(binding = 5) uniform usampler2DMS tex_peel_3;
	layout(binding = 6) uniform usampler2DMS tex_peel_4;
	layout(binding = 7) uniform usampler2DMS tex_peel_5;
	layout(binding = 8) uniform usampler2DMS tex_peel_6;
	layout(binding = 9) uniform usampler2DMS tex_peel_7;
#else
	layout(binding = 0) uniform sampler2DMS	tex_peel_0;
	layout(binding = 1) uniform sampler2DMS tex_peel_1;
	layout(binding = 4) uniform sampler2DMS tex_peel_2;
	layout(binding = 5) uniform sampler2DMS tex_peel_3;
	layout(binding = 6) uniform sampler2DMS tex_peel_4;
	layout(binding = 7) uniform sampler2DMS tex_peel_5;
	layout(binding = 8) uniform sampler2DMS tex_peel_6;
	layout(binding = 9) uniform sampler2DMS tex_peel_7;
#endif

#else
#if packing || sorted_by_id
	layout(binding = 0) uniform usampler2DRect tex_peel_0;
	layout(binding = 1) uniform usampler2DRect tex_peel_1;
	layout(binding = 4) uniform usampler2DRect tex_peel_2;
	layout(binding = 5) uniform usampler2DRect tex_peel_3;
	layout(binding = 6) uniform usampler2DRect tex_peel_4;
	layout(binding = 7) uniform usampler2DRect tex_peel_5;
	layout(binding = 8) uniform usampler2DRect tex_peel_6;
	layout(binding = 9) uniform usampler2DRect tex_peel_7;
#else
	layout(binding = 0) uniform sampler2DRect tex_peel_0;
	layout(binding = 1) uniform sampler2DRect tex_peel_1;
	layout(binding = 4) uniform sampler2DRect tex_peel_2;
	layout(binding = 5) uniform sampler2DRect tex_peel_3;
	layout(binding = 6) uniform sampler2DRect tex_peel_4;
	layout(binding = 7) uniform sampler2DRect tex_peel_5;
	layout(binding = 8) uniform sampler2DRect tex_peel_6;
	layout(binding = 9) uniform sampler2DRect tex_peel_7;
#endif

#endif

#if packing || sorted_by_id
	layout(location = 0, index = 0) out uvec4 out_frag_color[8];
	uvec4 k[8];
#else
	layout(location = 0, index = 0) out  vec4 out_frag_color[8];
	vec4 k[8];
#endif

#if sorted_by_id
	void insertionSort(int j, uvec4 value);
#endif
	
#if early_Z
	layout (early_fragment_tests) in;
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
	bool b = false;
	for(int i=0; i<KB_SIZE; i++)
	{
		for(int j=0; j<4; j++)
			if(k[i][j] == 0U)
			{
				k[i][j] = packUnorm4x8(computePixelColor().abgr);
				b = true;
				break;
			}
		if(b)
			break;
	}
#else

#if sorted_by_id	
	uvec4 value;
#endif

	for(int i=0; i<KB_SIZE; i++)
	{
#if sorted_by_id
		if(int(k[i].g) < gl_PrimitiveID)
#else
		if(k[i].a == 0.0f)
#endif
		{
#if sorted_by_id
			value.r = packUnorm4x8(computePixelColor().abgr);
			value.g = uint(gl_PrimitiveID);
			insertionSort(i, value);
#else
			k[i] = computePixelColor();
#endif
			break;
		}
	}
#endif

	for(int i=0; i<KB_SIZE; i++)
		out_frag_color[i] = k[i];
}

#if sorted_by_id
void insertionSort(int j, uvec4 value)
{
	int r   = KB_SIZE-1;
	int num = r-j;
	for(int i=0; i<num; i++)
	{
		k[r] = k[r-1];
		r = r-1;
	}
	k[j] = value;
}
#endif