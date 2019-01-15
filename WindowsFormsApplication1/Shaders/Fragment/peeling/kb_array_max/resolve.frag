#include "define.h"
#include "sort.h"
#include "resolve.h"
//#include "heatmap.h"

	layout(binding = 0, r32ui) readonly uniform uimage2DRect  image_counter;
	layout(binding = 4, rg32f) readonly uniform  image2DArray image_peel;
	//layout(binding = 6, r32ui) readonly uniform uimage2DRect  image_skata;
		
	uint  getPixelFragCounter	(				  ) { return imageLoad (image_counter, ivec2(gl_FragCoord.xy)).r;}
	vec4  getPixelFragValue		(const int coord_z) { return imageLoad (image_peel	 , ivec3(gl_FragCoord.xy, coord_z));}

	layout(location = 0, index = 0) out  vec4 out_frag_color;

	void main(void)
	{
		int counter = int(getPixelFragCounter());
		if(counter > 0)
		{
			//out_frag_color.rgb  = getHeatMap(float(counter), 0.0f, 200.0f);
			//out_frag_color.a	= 1.0f;
			
			int j=0;
			for(int i=HEAP_SIZE_1n; i>=HEAP_SIZE-counter; i--)
				fragments[j++] = getPixelFragValue(i).rg;
			out_frag_color = resolve(counter, false);
		}
		else
			discard;
	}