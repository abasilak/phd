#include "define.h"
#include "sort.h"
#include "resolve_b2.h"

	layout(binding = 0, r32ui) readonly uniform uimage2DRect  image_counter;
	layout(binding = 4, r32f ) readonly uniform  image2DArray image_peel_depth;
	layout(binding = 5, r32ui) readonly uniform uimage2DArray image_peel_color;
	
	uint  getPixelFragCounter	(				  ) { return imageLoad (image_counter	, ivec2(gl_FragCoord.xy)).r;}
	float getPixelFragDepthValue(const int coord_z) { return imageLoad (image_peel_depth, ivec3(gl_FragCoord.xy, coord_z)).r;}
	uint  getPixelFragColorValue(const int coord_z) { return imageLoad (image_peel_color, ivec3(gl_FragCoord.xy, coord_z)).r;}

	layout(location = 0, index = 0) out  vec4 out_frag_color;

	void main(void)
	{
		int counter = int(getPixelFragCounter());
		if(counter > 0)
		{
			if(counter > HEAP_SIZE)
				counter = HEAP_SIZE;

			int j=0;
			for(int i=HEAP_SIZE_1n; i>=HEAP_SIZE-counter; i--)
			{
				fragments[j] = vec2(j, getPixelFragDepthValue(i));
				colors	 [j] = getPixelFragColorValue(i);
				j++;
			}
			out_frag_color = resolve(counter, false);
		}
		else
			discard;
	}