#include "define.h"
#include "sort_b2.h"
#include "resolve_b2.h"

// IN
#if multisample
	layout(binding = 0, r32ui) readonly uniform uimage2DMS   image_next;
	layout(binding = 1, r32ui) readonly uniform uimage2DMS   image_counter;
#else
	layout(binding = 0, r32ui) readonly uniform uimage2DRect image_next;
	layout(binding = 1, r32ui) readonly uniform uimage2DRect image_counter;
#endif 
	layout(binding = 4, r32f ) readonly uniform  imageBuffer image_peel_depth;
	layout(binding = 5, r32ui) readonly uniform uimageBuffer image_peel_color;
	layout(binding = 6, r32ui) readonly uniform uimageBuffer image_pointers;

#if multisample
	uint getPixelCurrentPageID	(const ivec2 coords) {return imageLoad(image_next	, coords, gl_SampleID).x;}
	uint getPixelFragCounter	(const ivec2 coords) {return imageLoad(image_counter, coords, gl_SampleID).x;}
#else
	uint getPixelCurrentPageID	  ()				 {return imageLoad(image_next	, ivec2(gl_FragCoord.xy)).r;}
	uint getPixelFragCounter	  ()				 {return imageLoad(image_counter, ivec2(gl_FragCoord.xy)).r;}
#endif
	uint  sharedPoolGetLink		  (const uint index) {return imageLoad (image_pointers  , int(index)).r;}
	float sharedPoolGetDepthValue (const uint index) {return imageLoad (image_peel_depth, int(index)).r;}
	uint  sharedPoolGetColorValue (const uint index) {return imageLoad (image_peel_color, int(index)).r;}
	
	layout(location = 0, index = 0) out vec4 out_frag_color;

	void main(void)
	{
		uint page_id = getPixelCurrentPageID();
		if(page_id > 0U)
		{
			int counter = int(getPixelFragCounter());
			if(counter > 0)
			{
				int  i=0,j;
				uint cur_page_id = page_id;

				// Remove fragments from last page
				for(j=0; j < counter % PAGE_SIZE; j++)
				{
					fragments[i] = vec2(i, sharedPoolGetDepthValue(page_id));
					colors   [i] = sharedPoolGetColorValue(page_id);
					i++;
					page_id--;
				}

				if(j>0)
				{
					cur_page_id = sharedPoolGetLink(cur_page_id);
					page_id		= cur_page_id;
				}

				// Remove fragments from other pages
				while(cur_page_id != 0U)
				{
					fragments[i] = vec2(i, sharedPoolGetDepthValue(page_id));
					colors   [i] = sharedPoolGetColorValue(page_id);
					i++;
					
					if( int(page_id-1U) % PAGE_SIZE == 0)
					{
						cur_page_id = sharedPoolGetLink(cur_page_id);
						page_id		= cur_page_id;
					}
					else
						page_id--;
				}			
				out_frag_color = resolve(counter, false);
				return;
			}
		}
		discard;
	}