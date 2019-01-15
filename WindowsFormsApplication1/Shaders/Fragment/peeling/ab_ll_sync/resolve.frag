#include "define.h"
#include "data_structs.h"
#include "sort.h"
#include "resolve.h"

	layout(binding = 0, r32ui ) readonly uniform uimage2DRect  image_next;
	layout(binding = 4, std430) readonly buffer LinkedLists { NodeTypeLL nodes[]; };
	layout(binding = 6, r32ui ) readonly uniform uimage2DRect  image_counter;
	
	uint  getPixelFragCounter	()					{return imageLoad (image_counter , ivec2(gl_FragCoord.xy)).r;}
	uint  getPixelCurrentPageID	()					{return imageLoad (image_next    , ivec2(gl_FragCoord.xy)).r;}

	layout(location = 0, index = 0) out vec4 out_frag_color;
	
	void main(void)
	{
		int C = int(getPixelFragCounter());
		if(C > 0)
		{
			uint P = getPixelCurrentPageID();
			for(int i=0; i<C; i++)
			{
				fragments[i] = vec2(uintBitsToFloat(nodes[P].color), nodes[P].depth);
				P		     = nodes[P].next;
			}
			out_frag_color = resolve(C, true);
		}
		else
			discard;
	}