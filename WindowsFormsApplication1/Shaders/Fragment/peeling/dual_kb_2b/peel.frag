#include "define.h"
#extension GL_EXT_gpu_shader4 : enable

// INCLUDE
vec4 computePixelColor();
// IN
uniform bool front;
//uniform int  bufferSize;
#if multisample
	layout(binding = 10) uniform sampler2DMS	tex_depth;
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
	layout(binding = 10) uniform sampler2DRect tex_depth;
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
// OUT
#if packing || sorted_by_id
	layout(location = 0, index = 0) out uvec4 out_frag_color[8];
	uvec4 k[8];
#else
	layout(location = 0, index = 0) out  vec4 out_frag_color[8];
	vec4  k[8];
#endif

#if sorted_by_id
	void insertionSort(const int j, const uvec4 value);
#endif

void main(void)
{
	int  i;
	vec2 depth;
	
#if multisample
	depth = texelFetch(tex_depth, ivec2(gl_FragCoord.xy), gl_SampleID).xy;
#else
	depth = texture(tex_depth, gl_FragCoord.xy).xy;
#endif
	float depth_near = -depth.x;
	float depth_far	 =  depth.y;

	float d = (front) ? depth_near : depth_far;
	if (gl_FragCoord.z != d)
		discard;
	// Only Front exports final (middle) layer
	if(!front && depth_near == depth_far)
		discard;

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

#if sorted_by_id
	uvec4 value = uvec4(0U);
#endif

#if packing
	int j;
	bool b = false;
	for(i=0; i<KB_SIZE; i++)
	{
		for(j=0; j<4; j++)
			if(k[i][j] == 0U)
			{
				k[i][j] = packUnorm4x8(computePixelColor());
				b = true;
				break;
			}
		if(b)
			break;
	}

#else
	for(i=0; i<KB_SIZE; i++)
#if sorted_by_id
		if(int(k[i].g) < gl_PrimitiveID)
#else
		if(k[i].w == 0.0f)
#endif
		{
#if sorted_by_id
			value.r = packUnorm4x8(computePixelColor());
			value.g = uint(gl_PrimitiveID);
			if(k[i].r == 0U)
				k[i] = value;
			else
				insertionSort(i, value);
#else
			k[i] = computePixelColor();
#endif
			break;
		}
#endif
	for(i=0; i<KB_SIZE; i++)
		out_frag_color[i] = k[i];
}

#if sorted_by_id
void insertionSort(const int j, const uvec4 value)
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