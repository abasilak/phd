#include "define.h"
#include "s-buffer.h"
#include "data_structs.h"
#include "sort_b2.h"
#include "resolve_b2.h"

	layout(binding = 0, r32ui ) readonly  uniform uimage2DRect	image_counter;
	layout(binding = 1, r32ui ) readonly  uniform uimage2DRect	image_next;
	layout(binding = 4, std430) readonly  buffer  SBufferD		{ NodeTypeKB_Depth nodes_depth[]; };
	layout(binding = 5, std430) readonly  buffer  SBufferG		{ NodeTypeKB_Color nodes_gbuffer[]; };
	layout(binding = 6, std430)	readonly  buffer  final_address { uint		 final[]; };

	uint getPixelNextAddress () {return imageLoad(image_next   , ivec2(gl_FragCoord.xy)).x-1U;}
	uint getPixelFragCounter () {return imageLoad(image_counter, ivec2(gl_FragCoord.xy)).x;   }

	layout(location = 0, index = 0) out vec4 out_frag_color;

	void main(void)
	{
		int counter = int(getPixelFragCounter());
		if(counter == 0) discard;

		uint address = getPixelNextAddress();
		int  hash_id = hashFunction(ivec2(gl_FragCoord.xy));
		uint sum	 = final[hash_id+COUNTERS];
		uint page_id = address + sum;

		for(int i=0; i<counter; i++, page_id--)
		{
			fragments[i] = vec2(i,	nodes_depth	 [page_id].depth);
			colors	 [i] =			nodes_gbuffer[page_id].color;
		}

		out_frag_color = resolve(counter, false);
		return;
	}