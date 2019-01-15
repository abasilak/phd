#include "define.h"
#include "sort_fixed.h"

	uniform int	layer;

	layout(binding = 0, r32ui)	 readonly uniform uimage2DRect image_next;
	layout(binding = 1, r32ui)	 readonly uniform uimage2DRect image_counter;
	layout(binding = 4, rgba32f) readonly uniform  imageBuffer image_peel_pointers;

	uint getPixelCurrentPageID	()					 {return imageLoad (image_next			, ivec2(gl_FragCoord.xy)).x;}
	uint getPixelFragCounter	()					 {return imageLoad (image_counter		, ivec2(gl_FragCoord.xy)).x;}
	vec4 sharedPoolGetValue		(const uint index)	 {return imageLoad (image_peel_pointers	, int(index))  ;}
	
	layout(location = 0, index = 0) out vec4 out_frag_color;
	
	void main(void)
	{
		int counterTotal = int(getPixelFragCounter());
		if(counterTotal > 0)
		{
			float prevZ = 0.0f;
			
			uint init_page_id = getPixelCurrentPageID();

			int  Iter = int(ceil(float(counterTotal)/float(LOCAL_SIZE)));
			for(int I=0; I<Iter; I++)
			{
				int  counterLocal = 0;
				uint page_id = init_page_id;
				fragments [LOCAL_SIZE_1n].g = 1.0f;

				for(int C=0; C<counterTotal; C++)
				{
					vec3 peel_pointer = sharedPoolGetValue(page_id).rgb;
					
					page_id	= floatBitsToUint(peel_pointer.b);
					if(I>0 && peel_pointer.g <= prevZ)
						continue;

					if(counterLocal < LOCAL_SIZE_1n)
						fragments [counterLocal++] = peel_pointer.rg;
					else if(peel_pointer.g < fragments [LOCAL_SIZE_1n].g)
						fragments [setMaxFromGlobalArray(peel_pointer.g)] = peel_pointer.rg;
				}

				prevZ = fragments [LOCAL_SIZE_1n].g;

				sort(counterLocal);

				if(layer < (I+1)*LOCAL_SIZE)
				{
					out_frag_color = unpackUnorm4x8(floatBitsToUint(fragments [layer-I*LOCAL_SIZE].r));
					break;
				}
			}
		}

		if(layer < counterTotal)
			return;
		else
			discard;
	}