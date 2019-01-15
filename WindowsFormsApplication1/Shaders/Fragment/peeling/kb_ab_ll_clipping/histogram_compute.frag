#include "define.h"
#include "data_structs.h"

	layout(binding = 0, r32ui ) readonly uniform uimage2DArray  image_min_max;
	layout(binding = 1, r32ui ) coherent uniform uimage2DArray  image_histogram;
	layout(binding = 4, std430)	readonly buffer  LinkedLists   { NodeTypeLL nodes[]; };
	layout(binding = 6, offset = 0)		 uniform atomic_uint   next_address;

	float getPixelFragDepthMin		(									) {return uintBitsToFloat (	imageLoad	 (image_min_max			, ivec3(gl_FragCoord.xy, 0)).r);}
	float getPixelFragDepthMax		(									) {return uintBitsToFloat (	imageLoad	 (image_min_max			, ivec3(gl_FragCoord.xy, 1)).r);}
	void  setPixelFragHistogramDepth(const int coord_z, const uint value) {							imageAtomicOr(image_histogram		, ivec3(gl_FragCoord.xy, coord_z), value);}

	void main(void)
	{
		uint page_id = atomicCounterIncrement(next_address) + 1U;
		if( page_id < nodes.length())
		{
			float depth_near   = getPixelFragDepthMin();
			float depth_far    = getPixelFragDepthMax();
			float depth_length = depth_far - depth_near;

			int bucket = int(floor(float(BUCKET_SIZE)*(gl_FragCoord.z-depth_near)/depth_length));
			int b = bucket / 32;
			int j = bucket % 32;

			setPixelFragHistogramDepth(b, 1U << j);

			discard;
		}
	}