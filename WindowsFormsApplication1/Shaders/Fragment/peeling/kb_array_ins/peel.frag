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

	vec4 computePixelColor();

	layout(binding = 0, rg32f) coherent uniform  image2DArray image_peel;
#if !(INTEL_ordering | NV_interlock)
	layout(binding = 1, r32ui) coherent uniform uimage2DRect  image_semaphore;
	bool  semaphoreAcquire		(								   ) { return  imageAtomicExchange (image_semaphore, ivec2(gl_FragCoord.xy), 1U)==0U;}
	void  semaphoreRelease		(								   ) {		   imageStore          (image_semaphore, ivec2(gl_FragCoord.xy), uvec4(0U));}
#endif 

	vec4  getPixelFragValue(const int coord_z			) { return	imageLoad  (image_peel, ivec3(gl_FragCoord.xy, coord_z));}
	void  setPixelFragValue(const int coord_z, vec4 val	) {			imageStore (image_peel, ivec3(gl_FragCoord.xy, coord_z), val);}

	/* Vasilakis's Function
	void insertionSort(int j, float C)
	{
		int r_1n, r= HEAP_SIZE_1n;
		int num = r-j;
		for(int i=0; i<num; i++)
		{
			r_1n = r-1;
			setPixelFragValue(r, getPixelFragValue(r_1n));
			r = r_1n;
		}

		setPixelFragValue(j, vec4(C, gl_FragCoord.z, 0.0f, 0.0f));
	}

	void insertFragmentArrayIns2(float C)
	{
		int j=-1;
		for(int i=0; i<HEAP_SIZE; i++)
			if(gl_FragCoord.z < getPixelFragValue(i).g)
			{
				j=i; break;
			}

		if(j >= 0)
			insertionSort(j,C);
	}*/

	// Salvi's Function
	void insertFragmentArrayIns(float C)
	{
		float _Z = gl_FragCoord.z;
		float _C = C;

#if early_clipping_rest
		if(_Z < getPixelFragValue(HEAP_SIZE_1n).g)
#endif
		for(int i=0; i<HEAP_SIZE; i++)
		{
			vec2 Zi = getPixelFragValue(i).rg;
			if(_Z <= Zi.g)
			{
				vec2 temp = vec2(_C, _Z);
				_C = Zi.r; _Z = Zi.g;
				setPixelFragValue(i, vec4(temp.r, temp.g, 0.0f, 0.0f));
			}
		}
	}

	void main(void)
	{		
#if random_discard
		if(distribution(int(gl_FragCoord.x) + width*(int(gl_FragCoord.y) + instanceID)))
			discard;
#endif

#if early_clipping_rest
		if (gl_FragCoord.z < getPixelFragValue(HEAP_SIZE_1n).g)
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