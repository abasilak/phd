#include "define.h"
#if NV_interlock
#extension GL_NV_fragment_shader_interlock : enable

	layout(pixel_interlock_unordered) in;
#endif

#if random_discard
#include "noise.h"
	flat in int instanceID;
	uniform int width;
#endif

#if early_Z
	layout (early_fragment_tests) in;
#endif

	vec4 computePixelColor();

	layout(binding = 0, r32ui) coherent uniform uimage2DArray  image_count_sem_min_max;
	layout(binding = 4, rg32f) coherent uniform  image2DArray  image_peel;

	vec2  getPixelFragValue(const int coord_z					 ){return	imageLoad	(image_peel, ivec3(gl_FragCoord.xy, coord_z)).rg;}
	void  setPixelFragValue(const int coord_z, const vec2 value  ){			imageStore  (image_peel, ivec3(gl_FragCoord.xy, coord_z),  vec4(value, 0.0f, 0.0f));}

	uint  addPixelFragCounter	(				)					 {return  imageAtomicAdd (image_count_sem_min_max, ivec3(gl_FragCoord.xy, 0), 1U);}

#if !(INTEL_ordering | NV_interlock)
	bool  semaphoreAcquire		(								   ) { return (imageLoad			(image_count_sem_min_max, ivec3(gl_FragCoord.xy, 1)).r == 1U) ? false :
																			   imageAtomicExchange	(image_count_sem_min_max, ivec3(gl_FragCoord.xy, 1), 1U)==0U;}
	void  semaphoreRelease		(								   ) {		   imageStore			(image_count_sem_min_max, ivec3(gl_FragCoord.xy, 1), uvec4(0U));}
#endif

	void insertFragmentArray(float C)
	{	
		vec2 maxFR = vec2(-1.0f,0.0f);
		for(int i=0; i<HEAP_SIZE; i++)
		{
			vec2 Zi = getPixelFragValue(i);
			if(maxFR.g < Zi.g)
			{
				maxFR.r = i;
				maxFR.g = Zi.g;
			}
		}

		if(gl_FragCoord.z < maxFR.g)
			setPixelFragValue(int(maxFR.r), vec2(C, gl_FragCoord.z));
	}

	void main(void)
	{	
#if random_discard
		if(distribution(int(gl_FragCoord.x) + width*(int(gl_FragCoord.y) + instanceID)))
			discard;
#endif
		float C = uintBitsToFloat(packUnorm4x8(computePixelColor()));
		//float C = uintBitsToFloat(packUnorm4x8(vec4(1.0f)));

		int id = int(addPixelFragCounter());
		if (id < HEAP_SIZE)
			setPixelFragValue(id, vec2(C, gl_FragCoord.z));
		else
		{
#if		INTEL_ordering
			beginFragmentShaderOrderingINTEL();
#elif	NV_interlock
			beginInvocationInterlockNV();
#else
			int iter = 0;
			bool stay_loop = true;
			while (stay_loop && iter++ < MAX_ITERATIONS)
			{
				if (semaphoreAcquire())
				{
#endif
					insertFragmentArray(C);
#if		!(INTEL_ordering | NV_interlock)
					semaphoreRelease();
					stay_loop = false;
				}
			}
#endif
#if		NV_interlock
			endInvocationInterlockNV();
#endif
		}
	}