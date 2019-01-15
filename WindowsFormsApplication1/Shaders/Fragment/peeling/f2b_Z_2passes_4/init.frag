#version 330 core

layout(location = 0, index = 0) out vec4 out_frag_depth;

void main(void)
{
	out_frag_depth.r = -gl_FragCoord.z;
}