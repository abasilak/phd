﻿#include "define.h"

#if random_discard
#include "noise.h"
	uniform int	width;
	flat in int instanceID;
#endif

#if early_Z
	layout (early_fragment_tests) in;
#endif

	vec4 computePixelColor();

	layout(binding = 4, r32ui) readonly  uniform  uimage2DArray image_peel_depth;
	layout(binding = 5, r32ui) writeonly uniform  uimage2DArray image_peel_color;

	uint getPixelFragDepthValue	(const int coord_z					) {return	imageLoad (image_peel_depth, ivec3(gl_FragCoord.xy, coord_z)).r;}
	void setPixelFragColorValue	(const int coord_z, const uint val	) {			imageStore(image_peel_color, ivec3(gl_FragCoord.xy, coord_z), uvec4(val,0U,0U,0U));}
	
	void main(void)
	{
#if random_discard
		if(distribution(int(gl_FragCoord.x) + width*(int(gl_FragCoord.y) + instanceID)))
			discard;
#endif

		// [Binary Search]
		int  low  = 0, mid;
		int	 high = HEAP_SIZE_1n;
		uint Zi, zTest = floatBitsToUint(gl_FragCoord.z);

		while(low <= high)
		{
			mid = int(floor(float(high+low)*0.5f));

			Zi  = getPixelFragDepthValue(mid);
			if(Zi == zTest)
			{
				//uint C = (packUnorm4x8(computePixelColor()));
				uint C = (packUnorm4x8(vec4(1.0f)));
				
				setPixelFragColorValue(mid, C);
				break;
			}
			else if(Zi < zTest)
				low  = mid + 1;
			else
				high = mid - 1;
		}
	}
