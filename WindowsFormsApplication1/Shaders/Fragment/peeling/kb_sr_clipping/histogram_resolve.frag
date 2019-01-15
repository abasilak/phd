#include "define.h"

	layout(binding = 0, r32ui) readonly uniform uimage2DArray  image_min_max;
	layout(binding = 1, r32ui) readonly uniform uimage2DArray  image_histogram;
	
	uint getPixelFragDepthMin		(					) {return imageLoad (image_min_max	, ivec3(gl_FragCoord.xy, 0)).r;}
	uint getPixelFragDepthMax		(					) {return imageLoad (image_min_max	, ivec3(gl_FragCoord.xy, 1)).r;}
	uint getPixelFragHistogramDepth	(const int coord_z	) {return imageLoad (image_histogram, ivec3(gl_FragCoord.xy, coord_z)).r;}
	
	void main(void)
	{
		gl_FragDepth = 1.0f;

		uint depth_nearUI	= getPixelFragDepthMin();
		if(depth_nearUI == Packed_1f)
			return;

		float depth_near	= uintBitsToFloat (depth_nearUI);
		float depth_far		= uintBitsToFloat (getPixelFragDepthMax());
		float depth_length	= depth_far - depth_near;
		
		int	counterGlobal = 0;
		for(int i=0; i<BUCKET_SIZE_32d; i++)
		{
			uint bitVectorUint	= getPixelFragHistogramDepth(i);	
			
			int counterTmp		= counterGlobal + bitCount(bitVectorUint);
			if(counterTmp < HEAP_SIZE)
				counterGlobal = counterTmp;
			else
			{
				while (bitVectorUint > 0U)
				{
					if(++counterGlobal == HEAP_SIZE)
					{
						int			j = int(log2(bitVectorUint&-bitVectorUint))+1;
						float		d = depth_length*BUCKET_SIZE_div;
						float		Z = depth_near + d*float(32*i + j+1);

						gl_FragDepth  = Z;

						return;
					}
					bitVectorUint &= (bitVectorUint-1U);
				}
			}
		}
	}