#include "define.h"
#include "sort_b2.h"
#include "resolve_b2.h"

	layout(binding = 0, r32ui) readonly uniform uimage2DRect image_next;
	layout(binding = 1, r32ui) readonly uniform uimage2DRect image_counter;
	layout(binding = 4, r32f ) readonly uniform  imageBuffer image_peel_depth;
	layout(binding = 5, r32ui) readonly uniform uimageBuffer image_peel_color;
	layout(binding = 6, r32ui) readonly uniform uimageBuffer image_pointers;

	uint  getPixelCurrentPageID	() {return imageLoad(image_next	  , ivec2(gl_FragCoord.xy)).x;}
	uint  getPixelFragCounter	() {return imageLoad(image_counter, ivec2(gl_FragCoord.xy)).x;}
	float sharedPoolGetDepthValue(const uint index)	 {return imageLoad (image_peel_depth, int(index)).r;}
	uint  sharedPoolGetColorValue(const uint index)	 {return imageLoad (image_peel_color, int(index)).r;}
	uint  sharedPoolGetLink		 (const uint index)	 {return imageLoad (image_pointers  , int(index)).r;}
	
	layout(location = 0, index = 0) out vec4 out_frag_color;

	void main(void)
	{
		uint page_id = getPixelCurrentPageID();
		if(page_id > 0U)
		{
			int counter = int(getPixelFragCounter());
			if(counter > 0)
			{
				for(int i=0; i<counter; i++)
				{
					fragments[i] = vec2(i, sharedPoolGetDepthValue(page_id));
					colors	 [i] = sharedPoolGetColorValue(page_id);

					page_id = sharedPoolGetLink(page_id);
				}			

				out_frag_color = resolve(counter, false);
				return;
			}
		}
		discard;
	}