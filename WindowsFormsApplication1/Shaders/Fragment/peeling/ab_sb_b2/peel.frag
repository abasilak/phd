#include "define.h"
#include "s-buffer.h"
#include "data_structs.h"

	vec4 computePixelColor();

	layout(binding = 1, r32ui)  coherent  uniform uimage2DRect		image_next;
	layout(binding = 4, std430) coherent  buffer  SBufferD		  { NodeTypeKB_Depth nodes_depth  []; };
	layout(binding = 5, std430) coherent  buffer  SBufferG		  { NodeTypeKB_Color nodes_gbuffer[]; };
	layout(binding = 6, std430)	readonly  buffer  final_address	  { uint final[]; };
	
	uint addPixelNextAddress() {return imageAtomicAdd	(image_next, ivec2(gl_FragCoord.xy), 1U);}

	void main(void)
	{
		//1. LOCAL POS [from '0' to 'per pixel k']
		uint  page_id = addPixelNextAddress();
		
		//2. GLOBAL POS
		int   hash_id = hashFunction(ivec2(gl_FragCoord.xy))+COUNTERS;
		uint  index   = final[hash_id] + page_id;

		//3. STORE
		nodes_depth	 [index].depth = gl_FragCoord.z;
		nodes_gbuffer[index].color = packUnorm4x8(computePixelColor());
	}