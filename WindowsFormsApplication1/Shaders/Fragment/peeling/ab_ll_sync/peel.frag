#include "define.h"
#include "data_structs.h"

	vec4 computePixelColor();

	layout(binding = 0, r32ui)		coherent uniform uimage2DRect  image_next;
	layout(binding = 1, r32ui)		coherent uniform uimage2DRect  image_semaphore;
	layout(binding = 4, std430)		coherent buffer  LinkedLists   { NodeTypeLL nodes[]; };
	layout(binding = 5, offset = 0)			 uniform atomic_uint   next_address;
	layout(binding = 6, r32ui)		coherent uniform uimage2DRect  image_counter;

	
	bool semaphoreAcquire	(									) { return (imageLoad			(image_semaphore, ivec2(gl_FragCoord.xy)).r == 1U) ? false :
																			imageAtomicExchange (image_semaphore, ivec2(gl_FragCoord.xy), 1U)==0U;}
	void semaphoreRelease	(									) {			imageStore			(image_semaphore, ivec2(gl_FragCoord.xy), uvec4(0U));}

	uint getPixelFragPageID	(									) { return  imageLoad	(image_next, ivec2(gl_FragCoord.xy)).r;}
	void setPixelFragPageID	(const uint  val					) {	        imageStore	(image_next, ivec2(gl_FragCoord.xy), uvec4(val, 0U, 0U, 0U));}

	void setPixelFragCounter(const uint  val					) {			imageStore	(image_counter, ivec2(gl_FragCoord.xy), uvec4(val, 0U, 0U, 0U));}
	uint getPixelFragCounter(									) { return  imageLoad	(image_counter, ivec2(gl_FragCoord.xy)).r;}


	void insertFragmentLinkedList(uint P)
	{
		nodes[P].next = 0U;

		uint C  = getPixelFragCounter();
		uint Ps = getPixelFragPageID();
		if(C == 0U)
			setPixelFragPageID(P);
		else 
		{
			if (gl_FragCoord.z < nodes[Ps].depth)
			{
				nodes[P].next = Ps;
				setPixelFragPageID(P);
			}
			else
			{
				int I=0;
				uint P1 = Ps;
				uint P2 = nodes[int(P1)].next; 
				while(I++ < C-1U && gl_FragCoord.z >= nodes[int(P2)].depth)
				{
					P1 = P2;
					P2 = nodes[int(P2)].next;
				}
				nodes[int(P )].next = P2;
				nodes[int(P1)].next = P ;
			}
		}
		setPixelFragCounter(C+1U);
	}

	void main(void)
	{
		uint P = atomicCounterIncrement(next_address) + 1U;
		if(	 P < nodes.length())
		{
			nodes[P].color = packUnorm4x8(computePixelColor());
			nodes[P].depth = gl_FragCoord.z;

			bool stay_loop = true;
			while (stay_loop)
			{
				if (semaphoreAcquire())
				{
					insertFragmentLinkedList(P);

					semaphoreRelease();
					stay_loop = false;
				}
			}

			discard;
		}
	}
	