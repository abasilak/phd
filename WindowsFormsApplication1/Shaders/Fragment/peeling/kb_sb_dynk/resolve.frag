#include "define.h"
#include "s-buffer.h"
#include "sort.h"
#include "resolve.h"

	layout(binding = 0, r32ui) readonly  uniform uimage2DArray  image_count_countT_next_sem;
	layout(binding = 4, rg32f) readonly  uniform  imageBuffer   image_peel;
	layout(binding = 5, std430)	readonly  buffer  final_address { uint final[]; };

	uint getPixelFragCounterTotal(				  ) {return imageLoad(image_count_countT_next_sem, ivec3(gl_FragCoord.xy, 0)).r;}
	uint getPixelNextAddress	 (				  ) {return imageLoad(image_count_countT_next_sem, ivec3(gl_FragCoord.xy, 2)).r;}
	vec4 sharedPoolGetValue		 (const uint index) {return imageLoad(image_peel, int(index));}

	layout(location = 0, index = 0) out vec4 out_frag_color;

	void main(void)
	{
		int counter = int(getPixelFragCounterTotal());
		if(counter == 0) discard;

		uint address = getPixelNextAddress();
		int  hash_id = hashFunction(ivec2(gl_FragCoord.xy));
		uint sum	 = final[hash_id+COUNTERS];
		uint page_id = address + sum;

		for(int i=0; i<counter; i++, page_id++)
			fragments[i] = sharedPoolGetValue(page_id).rg;
		out_frag_color = resolve(counter, false);
	}