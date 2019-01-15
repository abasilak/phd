#include "define.h"
#if NV_interlock
#extension GL_NV_fragment_shader_interlock : enable

	layout(pixel_interlock_unordered) in;
#endif

#if random_discard
#include "noise.h"
	uniform int width;
	flat in int instanceID;
#endif

#if early_Z
	layout (early_fragment_tests) in;
#endif

	vec4 computePixelColor();
	
	layout(binding = 0, r32ui) coherent uniform uimage2DArray  image_min_max_sem;
	layout(binding = 1, r32ui) coherent uniform uimage2DArray  image_histogram;
	layout(binding = 4, rg32f) coherent uniform  image2DArray  image_peel;

	vec4  getPixelFragValue(const int coord_z			) { return	imageLoad  (image_peel, ivec3(gl_FragCoord.xy, coord_z));}
	void  setPixelFragValue(const int coord_z, vec4 val	) {			imageStore (image_peel, ivec3(gl_FragCoord.xy, coord_z), val);}

#if !(INTEL_ordering | NV_interlock)
	bool  semaphoreAcquire		(								   ) { return (imageLoad (image_min_max_sem, ivec3(gl_FragCoord.xy, 2)).r == 1U) ? false :
																			   imageAtomicExchange (image_min_max_sem, ivec3(gl_FragCoord.xy, 2), 1U)==0U;}
	void  semaphoreRelease		(								   ) {		   imageStore(image_min_max_sem, ivec3(gl_FragCoord.xy, 2), uvec4(0U));}
#endif

	// Salvi's Function
	void insertFragmentArrayIns(float C)
	{
		float _Z = gl_FragCoord.z;
		float _C = C;

		for(int i=0; i<HEAP_SIZE; i++)
		{
			vec2 Zi = getPixelFragValue(i).rg;
			if(_Z <= Zi.g)
			{
				vec2 temp = vec2(_C, _Z);
				_C = Zi.r; _Z = Zi.g;
				setPixelFragValue(i, vec4(temp, 0.0f, 0.0f));
			}
		}
	}

	void main(void)
	{		
#if random_discard
		if(distribution(int(gl_FragCoord.x) + width*(int(gl_FragCoord.y) + instanceID)))
			discard;
#endif

		{
		float C = uintBitsToFloat(packUnorm4x8(computePixelColor()));
		//float C = uintBitsToFloat(packUnorm4x8(vec4(1.0f)));

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
				insertFragmentArrayIns(C);
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