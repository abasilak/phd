#include "define.h"

	layout(binding = 0, r32f)  readonly uniform  image2DArray	image_peel_depth;
	layout(binding = 1, r32ui) readonly uniform uimage2DArray	image_peel_color;

	float getPixelDepthPrevMax(const int val) {return imageLoad(image_peel_depth, ivec3(gl_FragCoord.xy, val)).r;}
	uint  getPixelFragCounter (				) {return imageLoad(image_peel_color, ivec3(gl_FragCoord.xy, FREEPIPE_SIZE)).r;}

	layout(location = 0, index = 0) out vec4 out_frag_depth;

	void main(void)
	{
		int counter = int(getPixelFragCounter());
		if(counter == 0)
			discard;

		out_frag_depth.r = getPixelDepthPrevMax (counter-1);
	}