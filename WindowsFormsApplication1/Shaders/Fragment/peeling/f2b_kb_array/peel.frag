#include "define.h"
#extension GL_EXT_gpu_shader4 : enable

// INCLUDE
vec4 computePixelColor();
// IN
uniform int bufferSize;
#if multisample
	uniform sampler2DMS	tex_depth;
#if packing	

#else
	uniform sampler2DMSArray tex_peel;
#endif

#else
	uniform sampler2DRect tex_depth;
#if packing
#else
	uniform sampler2DArray tex_peel;
#endif

#endif

#if packing
	layout(location = 0, index = 0) out uvec4 out_frag_color[8];
	uvec4 k[8];
#else
	layout(location = 0, index = 0) out vec4 out_frag_color[8];

	//void insertionSort(int j, vec4 value);
	//vec4 k[8];
#endif

void main(void)
{
	float depth;
#if multisample
	depth = texelFetch(tex_depth, ivec2(gl_FragCoord.xy), gl_SampleID).r;
#else
	depth = texture(tex_depth, gl_FragCoord.xy).r;
#endif
	//if (gl_FragCoord.z != depth)
		//discard;

	for(int i=0; i<8; i++)
		out_frag_color[i] = vec4(1);
}
/*
	int	i;
#if multisample
#else
	for(i=0; i<bufferSize; i++)
		k[i] = texture(tex_peel, ivec3(gl_FragCoord.xy, i));
#endif
	
#if packing
	int j;
	bool b = false;
	for(i=0; i<bufferSize; i++)
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
	// code for sorting fragmetns by primitive ID
	vec4 value;
	for(i=0; i<bufferSize; i++)
		//if(k[i].w == 0.0f)
		if(int(k[i].w) < gl_PrimitiveID )
		{
			//k[i] = computePixelColor();
			value = computePixelColor();
			value.a += float(gl_PrimitiveID);
			insertionSort(i, value);
			break;
		}
#endif
	//
	// edo kane diaforetiko for gia na ta apothikefseis me min/max diataksi !!!
	//
	for(i=0; i<bufferSize; i++)
		out_frag_color[i] = k[i];
}


void insertionSort(int j, vec4 value)
{
	int r   = bufferSize-1;
	int num = r-j;
	for(int i=0; i<num; i++)
	{
		k[r] = k[r-1];
		r = r-1;
	}
	k[j] = value;
}*/