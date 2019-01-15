#include "define.h"

#if multisample
	layout(binding = 2, r32ui) writeonly uniform uimage2DMS   image_page_id_front;
	layout(binding = 3, r32ui) writeonly uniform uimage2DMS   image_page_id_back;
#if sorted_by_id
	layout(binding = 4, r32ui) writeonly uniform uimage2DMS   image_counter_front;
	layout(binding = 5, r32ui) writeonly uniform uimage2DMS   image_counter_back;
#endif
#else
	layout(binding = 2, r32ui) writeonly uniform uimage2DRect image_page_id_front;
	layout(binding = 3, r32ui) writeonly uniform uimage2DRect image_page_id_back;
#if sorted_by_id	
	layout(binding = 4, r32ui) writeonly uniform uimage2DRect image_counter_front;
	layout(binding = 5, r32ui) writeonly uniform uimage2DRect image_counter_back;
#endif
#endif 

#if multisample
	void setPixelCurrentPageID_front(ivec2 coords, uint val){imageStore(image_page_id_front, coords, gl_SampleID, uvec4(val, 0U, 0U, 0U) );}
	void setPixelCurrentPageID_back (ivec2 coords, uint val){imageStore(image_page_id_back , coords, gl_SampleID, uvec4(val, 0U, 0U, 0U) );}
#if sorted_by_id	
	void setPixelFragCounter_front  (ivec2 coords, uint val){imageStore(image_counter_front, coords, gl_SampleID, uvec4(val, 0U, 0U, 0U) );}
	void setPixelFragCounter_back   (ivec2 coords, uint val){imageStore(image_counter_back , coords, gl_SampleID, uvec4(val, 0U, 0U, 0U) );}
#endif
#else
	void setPixelCurrentPageID_front(ivec2 coords, uint val){imageStore(image_page_id_front, coords, uvec4(val, 0U, 0U, 0U) );}
	void setPixelCurrentPageID_back (ivec2 coords, uint val){imageStore(image_page_id_back , coords, uvec4(val, 0U, 0U, 0U) );}
#if sorted_by_id
	void setPixelFragCounter_front  (ivec2 coords, uint val){imageStore(image_counter_front, coords, uvec4(val, 0U, 0U, 0U) );}
	void setPixelFragCounter_back   (ivec2 coords, uint val){imageStore(image_counter_back , coords, uvec4(val, 0U, 0U, 0U) );}
#endif
#endif
	
void main(void)
{
	ivec2 coords=ivec2(gl_FragCoord.xy);
	
	setPixelCurrentPageID_front	(coords, 0U);
	setPixelCurrentPageID_back	(coords, 0U);

#if sorted_by_id	
	setPixelFragCounter_front	(coords, 0U);
	setPixelFragCounter_back	(coords, 0U);
#endif
}
