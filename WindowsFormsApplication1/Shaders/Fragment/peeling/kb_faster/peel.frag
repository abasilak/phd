#include "define.h"
#include "sort_b2.h"

	vec4 computePixelColor();
	
	layout(binding = 0, r32f ) coherent uniform  image2DArray image_peel_depth;
	layout(binding = 1, r32ui) coherent uniform uimage2DArray image_peel_color;

	float getPixelFragDepthValue(const int coord_z					 ){return imageLoad	(image_peel_depth, ivec3(gl_FragCoord.xy, coord_z)).r;}
	uint  getPixelFragColorValue(const int coord_z					 ){return imageLoad	(image_peel_color, ivec3(gl_FragCoord.xy, coord_z)).r;}

	void  setPixelFragDepthValue(const int coord_z, const float depth){imageStore (image_peel_depth, ivec3(gl_FragCoord.xy, coord_z),  vec4(depth, 0.0f, 0.0f, 0.0f));}
	void  setPixelFragColorValue(const int coord_z, const uint  color){imageStore (image_peel_color, ivec3(gl_FragCoord.xy, coord_z), uvec4(color, 0U  , 0U  , 0U  ));}

	void main(void)
	{		
		for(int i=0; i<HEAP_SIZE; i++)
		{
			float Zi = getPixelFragDepthValue(i);
			fragments[i] = vec2(i, Zi);
			colors	 [i] = (Zi == 1.0f) ? 0U : getPixelFragColorValue(i);
		}

		float Z = gl_FragCoord.z;
		uint  C = packUnorm4x8(computePixelColor());

		for(int i=0; i<KB_SIZE; i++)
			if(Z <= fragments[i].g)
			{
				float tempZ = Z;
				uint  tempC = C;
				Z = fragments[i].g;
				C = colors[i];
				fragments[i].g = tempZ;
				colors[i] = tempC;
			}

		for(int i=0; i<HEAP_SIZE; i++)
		{
			setPixelFragDepthValue(i, fragments[i].g);
			setPixelFragColorValue(i, colors[i]);
		}
	}