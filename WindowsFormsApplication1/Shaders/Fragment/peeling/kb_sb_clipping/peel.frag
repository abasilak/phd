#include "define.h"
#if NV_interlock
#extension GL_NV_fragment_shader_interlock : enable

	layout(pixel_interlock_unordered) in;
#endif
#include "s-buffer.h"

#if random_discard
#include "noise.h"
	uniform int	  width;
	flat in int   instanceID;
#endif

#if early_Z
	layout (early_fragment_tests) in;
#endif

	vec4 computePixelColor();

	uniform uint *final_address[COUNTERS];

	layout(binding = 4, r32ui) coherent uniform uimage2DArray image_count_next_sem;
	layout(binding = 5, rg32f) coherent uniform  imageBuffer  image_peel;

	uint getPixelFragCounter		(								  ) { return imageLoad		(image_count_next_sem, ivec3(gl_FragCoord.xy,0)).r;}
	void setPixelFragCounter		(const uvec4 val				  ) {		 imageStore		(image_count_next_sem, ivec3(gl_FragCoord.xy,0), val);}
	uint getPixelFragNextAddress	(								  ) { return imageLoad		(image_count_next_sem, ivec3(gl_FragCoord.xy,1)).r;}

	vec4 sharedPoolGetValue			(const uint index				  ) { return imageLoad	(image_peel, int(index));}
	void sharedPoolSetValue			(const uint index, const vec4 val ) {		 imageStore	(image_peel, int(index), val);}

#if !(INTEL_ordering & NV_interlock)
	bool semaphoreAcquire			(								  ) {return (imageLoad			 (image_count_next_sem, ivec3(gl_FragCoord.xy,2)).r == 1U) ? false :
																				 imageAtomicExchange (image_count_next_sem, ivec3(gl_FragCoord.xy,2), 1U)==0U;}
	void semaphoreRelease			(								  ) {		 imageStore			 (image_count_next_sem, ivec3(gl_FragCoord.xy,2), uvec4(0U));}
#endif 

	void setMaxFromGlobalArray(uint start, float C)
	{	
		vec2 maxFR = vec2(-1.0f,0.0f);
		for(uint s=start; s < start + HEAP_SIZE; s++)
		{
			vec2 peel = sharedPoolGetValue(s).rg;
			if(maxFR.g < peel.g)
			{
				maxFR.r = s;
				maxFR.g = peel.g;
			}
		}	
		
		if(gl_FragCoord.z < maxFR.g)
			sharedPoolSetValue(uint(maxFR.r), vec4(C, gl_FragCoord.z, 0.0f, 0.0f));
	}

	void insertFragmentArray(uint start, float C)
	{
		uint  id, counter = getPixelFragCounter();

		if(counter < HEAP_SIZE)
		{	
			sharedPoolSetValue (start+counter, vec4(C, gl_FragCoord.z, 0.0f, 0.0f));
			setPixelFragCounter(uvec4(counter+1U, 0U, 0U, 0U));
		}
		else
			setMaxFromGlobalArray(start,C);
	}

	void main(void)
	{
#if random_discard
		if(distribution(int(gl_FragCoord.x) + width*(int(gl_FragCoord.y) + instanceID)))
			discard;
#endif
		uint  address = getPixelFragNextAddress();
		int   hash_id = hashFunction(ivec2(gl_FragCoord.y));
		uint  start   = (*final_address[hash_id]) + address;

		float C = uintBitsToFloat(packUnorm4x8(computePixelColor()));
		{

#if		INTEL_ordering
			beginFragmentShaderOrderingINTEL();
#elif	NV_interlock
			beginInvocationInterlockNV();
#else
			int  iter=0;
			bool stay_loop = true;
			while (stay_loop && iter++ < MAX_ITERATIONS)
			{
				if (semaphoreAcquire())
				{
#endif
					insertFragmentArray(start, C);

#if		!(INTEL_ordering & NV_interlock)
					semaphoreRelease();
					stay_loop = false;
				}
			}
#elif	NV_interlock
			endInvocationInterlockNV();
#endif
		}
	}