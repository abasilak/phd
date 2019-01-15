#version 420 core

// Include
vec4 computePixelColor();
// IN
uniform int  size;
uniform bool useTranslucency;
uniform sampler2DRect tex_histogram_0;
uniform sampler2DRect tex_histogram_1;
uniform sampler2DRect tex_histogram_2;
uniform sampler2DRect tex_histogram_3;
uniform sampler2DRect tex_histogram_4;
uniform sampler2DRect tex_histogram_5;
uniform sampler2DRect tex_histogram_6;
uniform sampler2DRect tex_histogram_7;
// OUT
layout(location = 0, index = 0) out vec4 out_frag_color[8];

void main(void)
{
	vec4 d[8];
	d[0] = texture(tex_histogram_0, gl_FragCoord.xy);
	d[1] = texture(tex_histogram_1, gl_FragCoord.xy);
	d[2] = texture(tex_histogram_2, gl_FragCoord.xy);
	d[3] = texture(tex_histogram_3, gl_FragCoord.xy);
	d[4] = texture(tex_histogram_4, gl_FragCoord.xy);
	d[5] = texture(tex_histogram_5, gl_FragCoord.xy);
	d[6] = texture(tex_histogram_6, gl_FragCoord.xy);
	d[7] = texture(tex_histogram_7, gl_FragCoord.xy);

	for(int i=0; i<size; i++)
		out_frag_color[i] = vec4(0.0f);

	float next, pr = 0.0f;
	for(int i=0; i<2; i++)
		for(int j=0; j<4; j++)
		{
			next = d[i][j];
			if(gl_FragCoord.z > pr && gl_FragCoord.z <= next)
			{
				out_frag_color[4*i + j] = computePixelColor();
				return;
			}
			pr = next;
		}
//	if(useTranslucency) out_frag_color[bucket].z = float(gl_FrontFacing);
}

