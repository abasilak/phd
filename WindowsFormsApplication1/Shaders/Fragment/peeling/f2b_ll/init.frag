#include "define.h"

#if multisample
	layout(binding = 6, r32ui) writeonly uniform uimage2DMS   image_page_id;
#if sorted_by_id
	layout(binding = 7, r32ui) writeonly uniform uimage2DMS   image_counter;
#endif
#else
	layout(binding = 6, r32ui) writeonly uniform uimage2DRect image_page_id;
#if sorted_by_id
	layout(binding = 7, r32ui) writeonly uniform uimage2DRect image_counter;
#endif
#endif 

#if multisample
	void setPixelCurrentPageID(const ivec2 coords, const uint val){imageStore(image_page_id  , coords, gl_SampleID, uvec4(val, 0U, 0U, 0U) );}
#if sorted_by_id
	void setPixelFragCounter  (const ivec2 coords, const uint val){imageStore(image_counter	 , coords, gl_SampleID, uvec4(val, 0U, 0U, 0U) );}
#endif
#else
	void setPixelCurrentPageID(const ivec2 coords, const uint val){imageStore(image_page_id  , coords, uvec4(val, 0U, 0U, 0U) );}
#if sorted_by_id
	void setPixelFragCounter  (const ivec2 coords, const uint val){imageStore(image_counter	 , coords, uvec4(val, 0U, 0U, 0U) );}
#endif
#endif
	
void main(void)
{
	ivec2 coords=ivec2(gl_FragCoord.xy);
	
	setPixelCurrentPageID(coords, 0U);
#if sorted_by_id
	setPixelFragCounter  (coords, 0U);
#endif
}
