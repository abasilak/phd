#include "define.h"
#extension GL_EXT_gpu_shader4 : enable

// Include
vec4 computePixelColor();

#if multisample
#if multipass
	layout(binding = 10) uniform sampler2DMS	tex_depth;
#endif
	layout(binding = 0) uniform sampler2DMS	tex_peel_0;
	layout(binding = 1) uniform sampler2DMS tex_peel_1;
	layout(binding = 4) uniform sampler2DMS tex_peel_2;
	layout(binding = 5) uniform sampler2DMS tex_peel_3;
	layout(binding = 6) uniform sampler2DMS tex_peel_4;
	layout(binding = 7) uniform sampler2DMS tex_peel_5;
	layout(binding = 8) uniform sampler2DMS tex_peel_6;
	layout(binding = 9) uniform sampler2DMS tex_peel_7;
#else
#if multipass
	layout(binding = 10) uniform sampler2DRect tex_depth;
#endif
	layout(binding = 0) uniform sampler2DRect tex_peel_0;
	layout(binding = 1) uniform sampler2DRect tex_peel_1;
	layout(binding = 4) uniform sampler2DRect tex_peel_2;
	layout(binding = 5) uniform sampler2DRect tex_peel_3;
	layout(binding = 6) uniform sampler2DRect tex_peel_4;
	layout(binding = 7) uniform sampler2DRect tex_peel_5;
	layout(binding = 8) uniform sampler2DRect tex_peel_6;
	layout(binding = 9) uniform sampler2DRect tex_peel_7;
#endif
//
// OUT
	layout(location = 0, index = 0) out vec4 out_frag_color[8];

void main(void)
{
#if multipass
	float depth;
#if multisample
	depth = texelFetch(tex_depth, ivec2(gl_FragCoord.xy), gl_SampleID).r;
#else
	depth = texture(tex_depth, gl_FragCoord.xy).r;
#endif
	if(gl_FragCoord.z <= depth)
		discard;
#endif

	vec4 k[8];
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

	for(int i=0; i<KB_SIZE; i++)
#if packing
		if(gl_PrimitiveID == int(k[i].g) || gl_PrimitiveID == int(k[i].a))
#else 
		if(gl_PrimitiveID == floatBitsToInt(k[i].b))
#endif
			discard;

#if packing	
	int p;
	ivec2 max_index = ivec2(-1);
#else
	int   max_index = -1;	
#endif 
	float d, max_depth = -1.0f;

	for(int i=0; i<KB_SIZE; i++)
	{
#if packing
		for(int j=0; j<2; j++)
		{
			p = 2*j+1;
			d = fract(k[i][p]);
			if(d > max_depth)
			{
				max_index = ivec2(i,2*j);
				max_depth = d;
			}
		}
#else 
		d = k[i].g;
		if(d > max_depth)
		{
			max_index = i;
			max_depth = d;
		}
#endif
	}
	
	if(gl_FragCoord.z >= max_depth)
		discard;
	else
	{
		float c = uintBitsToFloat(packUnorm4x8(computePixelColor()));
#if packing
		float z_id = gl_FragCoord.z + float(gl_PrimitiveID);
		vec2 value = vec2(c, z_id); 
		k[max_index.x][max_index.y  ] = value.x;
		k[max_index.x][max_index.y+1] = value.y;
#else
		vec4 value = vec4(c, gl_FragCoord.z, intBitsToFloat(gl_PrimitiveID), 0.0f);
		k[max_index] = value;
#endif
	}
	for(int i=0; i<KB_SIZE; i++)
		out_frag_color[i] = k[i];
}