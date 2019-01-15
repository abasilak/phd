#include "define.h"
#extension GL_EXT_gpu_shader4 : enable

// IN
	//uniform int bufferSize;
#if multisample
	layout(binding = 0) uniform sampler2DMS	tex_peel_0;
	layout(binding = 1) uniform sampler2DMS tex_peel_1;
	layout(binding = 2) uniform sampler2DMS tex_peel_2;
	layout(binding = 3) uniform sampler2DMS tex_peel_3;
	layout(binding = 4) uniform sampler2DMS tex_peel_4;
	layout(binding = 5) uniform sampler2DMS tex_peel_5;
	layout(binding = 6) uniform sampler2DMS tex_peel_6;
	layout(binding = 7) uniform sampler2DMS tex_peel_7;
	layout(binding = 8) uniform sampler2DMS tex_depth_bounds;
#else
	layout(binding = 0) uniform sampler2DRect tex_peel_0;
	layout(binding = 1) uniform sampler2DRect tex_peel_1;
	layout(binding = 2) uniform sampler2DRect tex_peel_2;
	layout(binding = 3) uniform sampler2DRect tex_peel_3;
	layout(binding = 4) uniform sampler2DRect tex_peel_4;
	layout(binding = 5) uniform sampler2DRect tex_peel_5;
	layout(binding = 6) uniform sampler2DRect tex_peel_6;
	layout(binding = 7) uniform sampler2DRect tex_peel_7;
	layout(binding = 8) uniform sampler2DRect tex_depth_bounds;
#endif
// OUT
	layout(location = 0, index = 0) out vec4 out_frag_color[8];

void main(void)
{
	vec2 depth_bounds;
#if multisample
	depth_bounds = texelFetch(tex_depth_bounds, ivec2(gl_FragCoord.xy), gl_SampleID).rg;
#else
	depth_bounds = texture(tex_depth_bounds, gl_FragCoord.xy).rg;
#endif
	float depth_near   = -depth_bounds.x;
	float depth_length =  depth_bounds.y - depth_near;
	
	vec4 d[8];
#if multisample
	d[0] = texelFetch(tex_peel_0, ivec2(gl_FragCoord.xy), gl_SampleID);
	d[1] = texelFetch(tex_peel_1, ivec2(gl_FragCoord.xy), gl_SampleID);
	d[2] = texelFetch(tex_peel_2, ivec2(gl_FragCoord.xy), gl_SampleID);
	d[3] = texelFetch(tex_peel_3, ivec2(gl_FragCoord.xy), gl_SampleID);
	d[4] = texelFetch(tex_peel_4, ivec2(gl_FragCoord.xy), gl_SampleID);
	d[5] = texelFetch(tex_peel_5, ivec2(gl_FragCoord.xy), gl_SampleID);
	d[6] = texelFetch(tex_peel_6, ivec2(gl_FragCoord.xy), gl_SampleID);
	d[7] = texelFetch(tex_peel_7, ivec2(gl_FragCoord.xy), gl_SampleID);
#else
	d[0] = texture(tex_peel_0, gl_FragCoord.xy);
	d[1] = texture(tex_peel_1, gl_FragCoord.xy);
	d[2] = texture(tex_peel_2, gl_FragCoord.xy);
	d[3] = texture(tex_peel_3, gl_FragCoord.xy);
	d[4] = texture(tex_peel_4, gl_FragCoord.xy);
	d[5] = texture(tex_peel_5, gl_FragCoord.xy);
	d[6] = texture(tex_peel_6, gl_FragCoord.xy);
	d[7] = texture(tex_peel_7, gl_FragCoord.xy); 
#endif

	int   bucket_size  = KB_SIZE;
	int   bucket = int(floor(bucket_size*(gl_FragCoord.z-depth_near)/depth_length));
	if(gl_FragCoord.z == -d[bucket].x)
	{
		for(int i=0; i<KB_SIZE; i++)
			out_frag_color[i] = vec4(-depth_max, float_min, 0.0f, 0.0f);

		ivec3 CountIds = ivec3(d[bucket].gba);
		if( CountIds.b <= 1 || (-CountIds.r < gl_PrimitiveID && CountIds.g > gl_PrimitiveID))
			out_frag_color[bucket] = vec4(-gl_FragCoord.z, -gl_PrimitiveID, gl_PrimitiveID, 1.0f);
	}
	else
		discard;
}