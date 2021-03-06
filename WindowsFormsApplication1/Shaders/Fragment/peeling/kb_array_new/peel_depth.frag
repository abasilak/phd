﻿#include "define.h"

	layout(binding = 0, r32ui) readonly uniform uimage2DRect  image_counter;
	layout(binding = 4, r32f ) readonly uniform  image2DArray image_peel_depth;

	uint  getPixelFragCounter	(				  )	{ return imageLoad  (image_counter	 , ivec2(gl_FragCoord.xy)).r;}
	vec4  getPixelFragDepthValue(const int coord_z)	{ return imageLoad  (image_peel_depth, ivec3(gl_FragCoord.xy, coord_z));}

	layout(location = 0, index = 0) out vec4 out_frag_depth;

	void main(void)
	{
		if(getPixelFragCounter() == 0U)
			discard;

		out_frag_depth.r = getPixelFragDepthValue(0);
	}