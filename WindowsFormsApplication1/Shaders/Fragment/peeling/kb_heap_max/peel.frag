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

	layout(binding = 0, r32ui) coherent uniform uimage2DRect  image_counter;
	layout(binding = 4, rg32f) coherent uniform  image2DArray image_peel;
#if !(INTEL_ordering | NV_interlock)
	layout(binding = 1, r32ui) coherent uniform uimage2DRect  image_semaphore;
	bool  semaphoreAcquire		(									)	{return (imageLoad			(image_semaphore, ivec2(gl_FragCoord.xy)).r == 1U) ? false :
																				 imageAtomicExchange(image_semaphore, ivec2(gl_FragCoord.xy), 1U)==0U;}
	void  semaphoreRelease		(									)	{		 imageStore			(image_semaphore, ivec2(gl_FragCoord.xy), uvec4(0U));}
#endif

#if multipass
	layout(binding = 6)					uniform  sampler2DRect	 tex_depth;
	float getPixelDepthPrevMax  (									)	{return texture    (tex_depth, gl_FragCoord.xy).r;}
#endif

	void  setPixelFragValue		(const int coord_z, const  vec4 val	)	{		imageStore (image_peel, ivec3(gl_FragCoord.xy, coord_z), val);}
	vec4  getPixelFragValue		(const int coord_z					)	{return imageLoad  (image_peel, ivec3(gl_FragCoord.xy, coord_z));}

	void  setPixelFragCounter	(const uvec4 val					)	{		imageStore (image_counter, ivec2(gl_FragCoord.xy), val);}
	uint  getPixelFragCounter	(									)	{return imageLoad  (image_counter, ivec2(gl_FragCoord.xy)).r;}

	int   parentHeap(const int j) {return (j-1) >> 1;}
	int   leftHeap	(const int j) {return (j<<1) + 1;}

	void maxHeapify(float C, float Zo)
	{
#if early_clipping
		if(Zo >= getPixelFragValue(0).g)
			return;
#endif

		int    P;
		int	   I = 0;
		int    L = 1;
		int    R = 2;

		vec4  Zl = getPixelFragValue(L);
		vec4  Zr = getPixelFragValue(R);

		for (int i=0; i<HEAP_SIZE_LOG2; i++)
		{
			float Z = Zo;
			if(L < HEAP_SIZE && Z < Zl.g)
			{
				P = L;
				Z = Zl.g;
			}
			else
				P = I;

			if(R < HEAP_SIZE && Z < Zr.g)
				P = R;

			if(P != I)
			{
				setPixelFragValue(I, getPixelFragValue(P));

				I = P; 
				L = leftHeap(I);	Zl = getPixelFragValue(L);
				R = L + 1;			Zr = getPixelFragValue(R);
			}
			else
				break;
		}
		setPixelFragValue(I, vec4(C, Zo, 0.0f, 0.0f));
	}

	void insertFragmentHeap(float C)
	{
		int counter = int(getPixelFragCounter());

		if (counter == HEAP_SIZE)
			maxHeapify(C, gl_FragCoord.z);
		else
		{
			int i = counter;
			int p = parentHeap(i);

			vec4 Zp;
			while (i > 0 && gl_FragCoord.z > (Zp = getPixelFragValue(p)).g)
			{
				setPixelFragValue(i, Zp);
				i = p;
				p = parentHeap(i);
			}

			setPixelFragValue  (i, vec4(C, gl_FragCoord.z, 0.0f, 0.0f));
			setPixelFragCounter(uvec4(uint(counter)+1U, 0U, 0U, 0U));
		}
	}

	void main(void)
	{

#if random_discard
		if(distribution(int(gl_FragCoord.x) + width*(int(gl_FragCoord.y) + instanceID)))
			discard;
#endif

#if multipass
		if(gl_FragCoord.z <= getPixelDepthPrevMax())
			discard;
#endif

#if early_clipping
		if(getPixelFragCounter() < HEAP_SIZE || gl_FragCoord.z < getPixelFragValue(0).g) // [needs both conditions !]
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
					insertFragmentHeap(C);
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