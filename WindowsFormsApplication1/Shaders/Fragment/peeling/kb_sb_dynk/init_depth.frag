#include "define.h"
#include "s-buffer.h"

	layout(binding = 0, r32ui)  readonly  uniform uimage2DArray image_count_countT_next_sem;
	layout(binding = 4, r32f )  writeonly uniform  imageBuffer  image_peel_depth;
	layout(binding = 5, std430)	readonly  buffer  final_address { uint final[]; };

	uint  getPixelNextAddress		(				 ) { return imageLoad  (image_count_countT_next_sem	, ivec3(gl_FragCoord.xy, 2)).r;}
	void  sharedPoolResetDepthValue	(const uint index) {		imageStore (image_peel_depth			, int(index), vec4 (0.0f,1.0f,0.0f,0.0f));}

	void main(void)
	{
		uint  address = getPixelNextAddress();
		int   hash_id = hashFunction(ivec2(gl_FragCoord.xy))+COUNTERS;
		uint  start   = final[hash_id] + address;

		sharedPoolResetDepthValue(start);
	}