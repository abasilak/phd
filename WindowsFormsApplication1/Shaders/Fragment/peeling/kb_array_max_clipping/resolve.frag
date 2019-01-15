#include "define.h"
#include "sort.h"
#include "resolve.h"
//#include "heatmap.h"

	layout(binding = 0, r32ui) readonly uniform uimage2DArray image_count_sem_min_max;
	layout(binding = 4, rg32f) readonly uniform  image2DArray image_peel;
	
	uint getPixelFragCounter	(				  ) { return imageLoad (image_count_sem_min_max	, ivec3(gl_FragCoord.xy,	   0)).r;}
	vec2 getPixelFragValue		(const int coord_z) { return imageLoad (image_peel				, ivec3(gl_FragCoord.xy, coord_z)).rg;}

	layout(location = 0, index = 0) out  vec4 out_frag_color;

	void main(void)
	{
		int counter = int(getPixelFragCounter());
		
		if(counter > 0)
		{
			//out_frag_color.rgb  = getHeatMap(float(counter), 0.0f, 200.0f);
			//out_frag_color.a	= 1.0f;

			counter = min(counter,HEAP_SIZE);

			for(int i=0; i<counter; i++)
				fragments[i] = getPixelFragValue(i);
			out_frag_color = resolve(counter, false);
		}
		else
			discard;
	}