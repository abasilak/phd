#include "define.h"
#include "sort.h"
#include "resolve.h"

	layout(binding = 4, r32ui) readonly uniform uimage2DRect  image_next;
	layout(binding = 5, r32ui) readonly uniform uimage2DRect  image_counter;
	layout(binding = 6, r32ui) readonly uniform uimage2DArray image_pointers;
	layout(binding = 7, rg32f) readonly uniform  image2DArray image_peel;

	uint  getPixelCurrentPageID	()					{return imageLoad (image_next    , ivec2(gl_FragCoord.xy)).r;}
	uint  getPixelFragCounter	()					{return imageLoad (image_counter , ivec2(gl_FragCoord.xy)).r;}
	vec4  getPixelFragValue		(const int coord_z) {return imageLoad (image_peel	 , ivec3(gl_FragCoord.xy, coord_z));}
	uint  getPixelFragNext		(const int coord_z) {return imageLoad (image_pointers, ivec3(gl_FragCoord.xy, coord_z)).r;}

	layout(location = 0, index = 0) out vec4 out_frag_color;
	
	void main(void)
	{
		int counter = int(getPixelFragCounter());
		if(counter > 0)
		{
			int currID = int(getPixelCurrentPageID());
			for(int i=counter-1; i>=0; i--)
			{
				fragments[i] = getPixelFragValue(currID).rg;
				currID		 = int(getPixelFragNext(currID));
			}
			out_frag_color = resolve(counter, true);
		}
		else
			discard;
	}