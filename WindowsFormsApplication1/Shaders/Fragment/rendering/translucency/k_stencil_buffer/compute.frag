#version 420 core
// IN
layout(binding = 0 ) uniform sampler2DMS tex_depth;
// OUT
layout(location = 0, index = 0) out vec4 out_frag_thickness;

const int max_samples = 8;

void main(void)
{
	float count=0.0f;
	vec2  thickness;
	// r: final, g: prev
	for(int i=0; i<max_samples; i++)
	{
		thickness = texelFetch(tex_depth, ivec2(gl_FragCoord.xy), i).zw;

		if(    thickness.y  == 1.0f) thickness.y =  0.0f;
		if(int(thickness.x) ==   0 ) thickness.y = -thickness.y;
		
		count += thickness.y;
	}
	out_frag_thickness.r = count;
}