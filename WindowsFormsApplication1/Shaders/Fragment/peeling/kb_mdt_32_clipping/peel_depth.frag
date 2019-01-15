#include "define.h"

#if random_discard
#include "noise.h"
	uniform int	width;
	flat in int instanceID;
#endif	

#if early_Z
	layout (early_fragment_tests) in;
#endif

	layout(binding = 4, r32ui) coherent uniform uimage2DArray image_peel_depth;

	uint getMaxPixelFragDepthValue	(									) { return imageLoad     (image_peel_depth, ivec3(gl_FragCoord.xy, HEAP_SIZE_1n)).r;}
	uint setMinPixelFragDepthValue	(const int coord_z, const uint val	) { return imageAtomicMin(image_peel_depth, ivec3(gl_FragCoord.xy, coord_z), val);}

	void main(void)
	{
#if random_discard
		if(distribution(int(gl_FragCoord.x) + width*(int(gl_FragCoord.y) + instanceID)))
			discard;
#endif

		uint zOld, zTest = floatBitsToUint(gl_FragCoord.z);
		for(int i=0; i<HEAP_SIZE; i++)
		{
			zOld = setMinPixelFragDepthValue(i, zTest);
			if (zOld == 0xFFFFFFFFU || zOld == zTest)
				break;
			zTest = max (zOld, zTest);
		}
	}