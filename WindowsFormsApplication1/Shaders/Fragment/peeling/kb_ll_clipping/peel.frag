#include "define.h"

#if random_discard
#include "noise.h"
	uniform int	width;
	flat in int instanceID;
#endif 

#if early_Z
	layout (early_fragment_tests) in;
#endif

	vec4 computePixelColor();

	uniform int  width_2;
	uniform uint total_fragments;

	layout(binding = 4, rg32f) coherent	 uniform  imageBuffer image_peel;
	layout(binding = 5, r32ui) coherent  uniform uimageBuffer image_pointers;
	layout(binding = 6, offset = 0)		 uniform atomic_uint  next_address;

	vec4 sharedPoolGetValue		(const uint index					)	{return imageLoad (image_peel	 , int(index));}
	void sharedPoolSetValue		(const uint index,	 const vec4 val	)	{		imageStore(image_peel	 , int(index), val);}

	uint sharedPoolGetLink		(const uint index					)	{return imageLoad (image_pointers, int(index)).r;}
	void sharedPoolSetLink		(const uint index,	 const uint val	)	{		imageStore(image_pointers, int(index), uvec4(val,   0U,   0U,   0U));}
	uint sharedPoolCompSwapLink	(const uint index, 
								 const uint compare, const uint val )	{return imageAtomicCompSwap(image_pointers, int(index), compare, val);}
	void main(void)
	{
#if random_discard
		if(distribution(int(gl_FragCoord.x) + width*(int(gl_FragCoord.y) + instanceID)))
			discard;
#endif
		// New Position
		uint newID = atomicCounterIncrement(next_address);
		// Store Color-Depth
		float C = uintBitsToFloat(packUnorm4x8(computePixelColor()));
		//float C = uintBitsToFloat(packUnorm4x8(vec4(1.0f)));

		// Prev Position
		uint headID = uint(gl_FragCoord.x + width_2*gl_FragCoord.y) * 2U + 2U;
		uint prevID = headID;

		int	 iter	 = 0;
		int  counter = 0;
		while(iter < MAX_ITERATIONS)
		{
			// Next Position
			uint nextID	= sharedPoolGetLink(prevID);
			if(nextID == 0U)
				break;

			if(gl_FragCoord.z < sharedPoolGetValue(nextID).g)
			{	
				// 1. [NEW]  --> [NEXT]
				sharedPoolSetLink		(newID, nextID);
				if(iter == 0)
					sharedPoolSetValue	(newID, vec4(C, gl_FragCoord.z, 0.0f, 0.0f));
				// 2. [PREV] --> [NEW]
				if(nextID == sharedPoolCompSwapLink(prevID, nextID, newID))
					break; // 3. [SUCCESS] 
				else
					++iter; // 3. [RETRY]
			}
			else
			{
				if(++counter == HEAP_SIZE)
					break;
				prevID = nextID;
			}
		}
	}