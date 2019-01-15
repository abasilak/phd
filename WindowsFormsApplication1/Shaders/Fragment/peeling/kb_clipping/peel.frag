#include "define.h"

	vec4 computePixelColor();

	layout(binding = 0) uniform sampler2DRect tex_peel_0;
	layout(binding = 1) uniform sampler2DRect tex_peel_1;
	layout(binding = 4) uniform sampler2DRect tex_peel_2;
	layout(binding = 5) uniform sampler2DRect tex_peel_3;
	layout(binding = 6) uniform sampler2DRect tex_peel_4;
	layout(binding = 7) uniform sampler2DRect tex_peel_5;
	layout(binding = 8) uniform sampler2DRect tex_peel_6;
	layout(binding = 9) uniform sampler2DRect tex_peel_7;
	layout(binding =10) uniform sampler2DRect tex_depth;

	layout(location = 0, index = 0) out vec4 out_frag_color[8];

	vec4 k[8];

#if packing
	void insertionSort(ivec2 jk, vec2 value)
	{
		int r   = KB_SIZE-1;
		int num = r-jk.x;
		for(int i=0; i<num; i++)
		{
			k[r].zw = k[r].xy;
			k[r].xy = k[r-1].zw;
			r = r-1;
		}
	
		if(jk.y==0)
		{
			k[jk.x].zw = k[jk.x].xy;
			k[jk.x].xy = value;
		}
		else
			k[jk.x].zw = value;
	}
#else/*
	void insertionSort(int j, vec2 value)
	{
		int r   = KB_SIZE-1;
		int num = r-j;
		for(int i=0; i<num; i++)
		{
			k[r] = k[r-1];
			r = r-1;
		}
		k[j].rg = value;
	}*/
#endif

void main(void)
{
	if(gl_FragCoord.z > texture (tex_depth, gl_FragCoord.xy).r)
		discard;

	k[0] = texture(tex_peel_0, gl_FragCoord.xy);
	k[1] = texture(tex_peel_1, gl_FragCoord.xy);
	k[2] = texture(tex_peel_2, gl_FragCoord.xy);
	k[3] = texture(tex_peel_3, gl_FragCoord.xy);
	k[4] = texture(tex_peel_4, gl_FragCoord.xy);
	k[5] = texture(tex_peel_5, gl_FragCoord.xy);
	k[6] = texture(tex_peel_6, gl_FragCoord.xy);
	k[7] = texture(tex_peel_7, gl_FragCoord.xy); 
    
	//if(gl_FragCoord.z < texture (tex_depth, gl_FragCoord.xy).r)
	{
		vec2 value = vec2(uintBitsToFloat(packUnorm4x8(computePixelColor())), gl_FragCoord.z);
#if packing
		ivec2 ij;
		bool  b = false;
		for(int i=0; i<KB_SIZE; i++)
		{
			for(int j=0; j<2; j++)
				if(gl_FragCoord.z < k[i][j*2 +1])
				{
					ij = ivec2(i,j);
					b = true;
					break;
				}
			if(b)
				break;
		}

		if(b)
			insertionSort(ij, value);
#else
		for(int i=0; i<KB_SIZE; i++)
			if(value.g <= k[i].g)
			{
				vec2 temp = value;
				value = k[i].rg;
				k[i].rg = temp;
			}

		//int j=-1;
		//for(int i=0; i<KB_SIZE; i++)
			//if(gl_FragCoord.z <= k[i].g)
			//{
				//j=i; break;
			//}
		//if(j >= 0)
			//insertionSort(j, value);
#endif
		//for(int i=0; i<KB_SIZE; i++)
			//out_frag_color[i] = k[i];

	}
	for(int i=0; i<KB_SIZE; i++)
		out_frag_color[i] = k[i];
}