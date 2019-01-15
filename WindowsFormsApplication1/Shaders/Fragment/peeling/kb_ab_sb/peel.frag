#include "define.h"
#include "s-buffer.h"

#if random_discard
#include "noise.h"
	uniform int	  width;
	flat in int   instanceID;
#endif
	
	vec4 computePixelColor();

	uniform uint *final_address[COUNTERS];

	layout(binding = 1, r32ui) coherent  uniform uimage2DRect image_next;
	layout(binding = 4, rg32f) writeonly uniform  imageBuffer image_peel;
	
	uint incPixelNextAddress(								  )	{return imageAtomicAdd	(image_next, ivec2(gl_FragCoord.xy), 1U);}
	void sharedPoolSetValue (const uint index, const  vec4 val)	{		imageStore		(image_peel, int(index), val);}

	void main(void)
	{
#if random_discard
		if(distribution(int(gl_FragCoord.x) + width*(int(gl_FragCoord.y) + instanceID)))
			discard;
#endif

		float C = uintBitsToFloat(packUnorm4x8(computePixelColor()));
		//float C = uintBitsToFloat(packUnorm4x8(vec4(1.0f)));

		uint  page_id = incPixelNextAddress();
		int   hash_id = hashFunction(ivec2(gl_FragCoord.y));
		uint  sum     = (*final_address[hash_id]) + page_id;

	#if inverse
		uint  index = hash_id < COUNTERS_2d ? sum : imageSize(image_peel) - sum;
	#else
		uint  index = sum;
	#endif

		sharedPoolSetValue (index,  vec4(C, gl_FragCoord.z,0.0f,0.0f));
	}