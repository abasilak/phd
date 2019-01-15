#include "define.h"
#include "sort_b2.h"
#include "resolve_b2.h"

	layout(binding = 0, r32f)  readonly uniform  image2DArray	image_peel_depth;
	layout(binding = 1, r32ui) readonly uniform uimage2DArray	image_peel_color;
	layout(binding = 2, r32ui) readonly uniform uimage2DRect	image_counter;

	uint getPixelFragCounter		(				  ) {return	imageLoad (image_counter, ivec2(gl_FragCoord.xy)).r;}
	vec2 getPixelFragIdDepthValues	(const int coord_z) {return vec2(float(coord_z), imageLoad (image_peel_depth, ivec3(gl_FragCoord.xy, coord_z)).r );}
	uint getPixelFragColorValue		(const int coord_z) {return imageLoad (image_peel_color, ivec3(gl_FragCoord.xy, coord_z)).r;}

// OUT
	layout(location = 0, index = 0) out  vec4 out_frag_color;

	void main(void)
	{
		int counter = int(getPixelFragCounter());
		if(counter > 0)
		{
			for(int i=0; i<counter; i++)
			{
				fragments[i] = getPixelFragIdDepthValues(i);
				colors	 [i] = getPixelFragColorValue	(i);
			}
			out_frag_color = resolve(counter, false);
		}
		else
			discard;
	}