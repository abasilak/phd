#include "define.h"

#if NV_interlock
#extension GL_NV_fragment_shader_interlock : enable

	layout(pixel_interlock_unordered) in;
#endif

	vec4 computePixelColor();

	layout(binding = 4, r32ui) coherent uniform uimage2DRect  image_next;
	layout(binding = 5, r32ui) coherent uniform uimage2DRect  image_counter;
	layout(binding = 6, r32ui) coherent uniform uimage2DArray image_pointers;
	layout(binding = 7, rg32f) coherent uniform  image2DArray image_peel;

#if !(INTEL_ordering | NV_interlock)
	layout(binding = 1, r32ui) coherent uniform uimage2DRect  image_semaphore;
	bool semaphoreAcquire	(									) { return (imageLoad (image_semaphore, ivec2(gl_FragCoord.xy)).r == 1U) ? false :
																			imageAtomicExchange (image_semaphore, ivec2(gl_FragCoord.xy), 1U)==0U;}
	void semaphoreRelease	(									) {			imageStore(image_semaphore, ivec2(gl_FragCoord.xy), uvec4(0U));}
#endif 

	void setPixelFragCounter(const uint  val					) {			imageStore	(image_counter, ivec2(gl_FragCoord.xy), uvec4(val, 0U, 0U, 0U));}
	uint getPixelFragCounter(									) { return  imageLoad	(image_counter, ivec2(gl_FragCoord.xy)).r;}

	uint getPixelFragPageID	(									) { return  imageLoad	(image_next, ivec2(gl_FragCoord.xy)).r;}
	void setPixelFragPageID	(const uint  val					) {	        imageStore	(image_next, ivec2(gl_FragCoord.xy), uvec4(val, 0U, 0U, 0U));}

	vec4 getPixelFragValue	(const int coord_z					) { return	imageLoad	(image_peel, ivec3(gl_FragCoord.xy, coord_z));}
	void setPixelFragValue	(const int coord_z, const vec4 val	) {			imageStore	(image_peel, ivec3(gl_FragCoord.xy, coord_z), val);}

	uint getPixelFragNext	(const int coord_z					) { return	imageLoad	(image_pointers, ivec3(gl_FragCoord.xy, coord_z)).r;}
	void setPixelFragNext	(const int coord_z, const uint val	) {			imageStore	(image_pointers, ivec3(gl_FragCoord.xy, coord_z), uvec4(val, 0U, 0U, 0U));}

	void insertFragmentLinkedList(float C)
	{
		uint P  = getPixelFragCounter();
		uint Ps = getPixelFragPageID();
		if(int(P) < HEAP_SIZE)
		{
			setPixelFragCounter(P+1U);
			if (P > 0U)
			{
				if (gl_FragCoord.z > getPixelFragValue(int(Ps)).g)
				{
					setPixelFragNext  (int(P), Ps);
					setPixelFragPageID(P);
				}
				else
				{
					uint P1 = Ps;
					uint P2 = getPixelFragNext(int(P1));
					for(int I=0; I<int(P)-1 && (gl_FragCoord.z <= getPixelFragValue(int(P2)).g); I++)
					{
						P1 = P2;
						P2 = getPixelFragNext(int(P2));
					}
					setPixelFragNext(int(P ),P2);
					setPixelFragNext(int(P1),P);
				}
			}
			setPixelFragValue (int(P), vec4(C, gl_FragCoord.z, 0.0f, 0.0f));
		}
		else if (gl_FragCoord.z < getPixelFragValue(int(Ps)).g)
		{
	/*		!!!	NOT WORKING !!!
			int  I=0;
			uint P1 = Ps;
			uint P2 = getPixelFragNext(int(P1));
			uint Ps2= P2;
			bool found = false;
			while(I<HEAP_SIZE_1n)
			{
				if (gl_FragCoord.z > getPixelFragValue(int(P2)).g)
				{
					if(I>0)
					{
						setPixelFragNext(int(P ),P2);
						setPixelFragNext(int(P1),P);
						setPixelFragPageID(Ps2);
					}
					break;
				}
				else
				{
					P1 = P2;
					P2 = getPixelFragNext(int(P2));
				}
				I++;
			}
			setPixelFragValue (int(Ps), vec4(C, gl_FragCoord.z, 0.0f, 0.0f));
	*/	
			uint P1 = Ps;
			uint P2 = getPixelFragNext(int(P1));
			vec2 PL2 = getPixelFragValue(int(P2)).rg;
			for(int I=0; I<HEAP_SIZE_1n && (gl_FragCoord.z <= PL2.g); I++)
			{
				setPixelFragValue (int(P1), vec4(PL2, 0.0f, 0.0f));

				P1 = P2;
				P2 = getPixelFragNext(int(P2));
				PL2 = getPixelFragValue(int(P2)).rg;
			}

			setPixelFragValue (int(P1), vec4(C, gl_FragCoord.z, 0.0f, 0.0f));
		}
	}

	void main(void)
	{

#if early_clipping
		if(getPixelFragCounter() < HEAP_SIZE || gl_FragCoord.z < getPixelFragValue(int(getPixelFragPageID())).g)
#endif
		{
			//float C = uintBitsToFloat(packUnorm4x8(computePixelColor()));
			float C = uintBitsToFloat(packUnorm4x8(vec4(1.0f)));

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
					insertFragmentLinkedList(C);

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