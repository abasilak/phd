#version 420 core

uniform int size; // ?
uniform  sampler2DRect tex_depth;
uniform usampler2DRect tex_histogram_0;
uniform usampler2DRect tex_histogram_1;
uniform usampler2DRect tex_histogram_2;
uniform usampler2DRect tex_histogram_3;
uniform usampler2DRect tex_histogram_4;
uniform usampler2DRect tex_histogram_5;
uniform usampler2DRect tex_histogram_6;
uniform usampler2DRect tex_histogram_7;

// OUT
layout(location = 0, index = 0) out vec4 out_frag_histogram[8];

void main(void)
{
	vec2  depth		   = texture(tex_depth, gl_FragCoord.xy).xy;
	float depth_near   = -depth.x;
	float depth_length =  depth.y - depth_near;
	float d = depth_length*0.0009765625f; // 1/1024 = 0.0009765625

	uint  b; 
	uvec4 h[8];
	h[0] = texture(tex_histogram_0, gl_FragCoord.xy);
	h[1] = texture(tex_histogram_1, gl_FragCoord.xy);
	h[2] = texture(tex_histogram_2, gl_FragCoord.xy);
	h[3] = texture(tex_histogram_3, gl_FragCoord.xy);
	h[4] = texture(tex_histogram_4, gl_FragCoord.xy);
	h[5] = texture(tex_histogram_5, gl_FragCoord.xy);
	h[6] = texture(tex_histogram_6, gl_FragCoord.xy);
	h[7] = texture(tex_histogram_7, gl_FragCoord.xy);

	for(int i=0; i<8; i++)
		out_frag_histogram[i] = vec4(0.0f);

	int r=0,p=0;
	for(int i=0; i<1; i++)
		for(int j=0; j<4; j++)
		{
			b = h[i][j];
			for(int k=0; k<32; k++)
			{
				if((b & 1U) == 1U)
				{
					out_frag_histogram[r/4][r%4] = depth_near + d*p;
					r++;
				//	if(++r == 32)
					//return;
				}				
				b = b >> 1U;
				p++;
			}
		}
}
