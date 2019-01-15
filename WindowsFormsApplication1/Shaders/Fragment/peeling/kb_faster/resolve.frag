#include "define.h"
#include "sort_b2.h"
#include "resolve_b2.h"

	layout(binding = 0, r32f ) readonly uniform  image2DArray image_peel_depth;
	layout(binding = 1, r32ui) readonly uniform uimage2DArray image_peel_color;
	
	float getPixelFragDepthValue(const int coord_z) { return imageLoad (image_peel_depth, ivec3(gl_FragCoord.xy, coord_z)).r;}
	uint  getPixelFragColorValue(const int coord_z) { return imageLoad (image_peel_color, ivec3(gl_FragCoord.xy, coord_z)).r;}

	layout(location = 0, index = 0) out  vec4 out_frag_color;

	void main(void)
	{
		int counter=0;
		for(int i=0; i<HEAP_SIZE; i++)
		{
			float Zi = getPixelFragDepthValue(i);
			if(Zi == 1.0f)
				break;

			fragments[i] = vec2(i, Zi);
			colors	 [i] = getPixelFragColorValue(i);

			counter++;
		}
		out_frag_color = resolve(counter, true);
	}