#version 420 core

// IN
uniform sampler2DRect tex_depth;
// OUT
layout(location = 0, index = 0) out uvec4 out_frag_histogram[8];

void main(void)
{
	vec4  k[8];
	vec2  depth		   = texture(tex_depth, gl_FragCoord.xy).xy;
	float depth_near   = -depth.x;
	float depth_length =  depth.y - depth_near;
	int   bucket_size  = 1024;

	for(int i=0; i<8; i++)
		out_frag_histogram[i] = uvec4(0U);

	int bucket = int(floor(bucket_size*(gl_FragCoord.z-depth_near)/depth_length));
	int b	   = bucket/128;
	int p	   = (bucket/32)%4;
	int j	   = bucket%32;

	out_frag_histogram[b][p] = 1U << uint(j);
}