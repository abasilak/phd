#include "define.h"
#include "data_structs.h"

	layout(binding = 0, r32ui) readonly uniform uimage2DArray  image_min_max;
	layout(binding = 1, r32ui) coherent uniform uimage2DArray  image_histogram;
	layout(binding = 4, r32ui) coherent uniform uimage2DArray  image_count_next_sem;

	float getPixelFragDepthMin		(									) {return uintBitsToFloat (	imageLoad	 (image_min_max			, ivec3(gl_FragCoord.xy, 0)).r);}
	float getPixelFragDepthMax		(									) {return uintBitsToFloat (	imageLoad	 (image_min_max			, ivec3(gl_FragCoord.xy, 1)).r);}
	void  setPixelFragHistogramDepth(const int coord_z, const uint value) {							imageAtomicOr(image_histogram		, ivec3(gl_FragCoord.xy, coord_z), value);}

	uint getPixelFragCounter() { return	imageLoad		(image_count_next_sem, ivec3(gl_FragCoord.xy,0)).r  ;}
	void incPixelFragCounter() {		imageAtomicAdd	(image_count_next_sem, ivec3(gl_FragCoord.xy,0), 1U);}

	void main(void)
	{
		float depth_near   = getPixelFragDepthMin();
		float depth_far    = getPixelFragDepthMax();
		float depth_length = depth_far - depth_near;

		int bucket = int(floor(float(BUCKET_SIZE)*(gl_FragCoord.z-depth_near)/depth_length));
		int b = bucket / 32;
		int j = bucket % 32;

		setPixelFragHistogramDepth(b, 1U << j);

		if(getPixelFragCounter() < HEAP_SIZE)
			incPixelFragCounter();
		else
			discard;
	}