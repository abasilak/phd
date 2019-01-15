#include "define.h"
#extension GL_NV_shader_thread_group : require
#if NV_interlock
#extension GL_NV_fragment_shader_interlock : enable

	layout(pixel_interlock_unordered) in;
#endif
#include "data_structs.h"

	vec4 computePixelColor();

	layout(binding = 0, r32ui)   coherent	uniform uimage2DRect image_next;
	layout(binding = 4, r32ui)   coherent	uniform uimage2DRect image_semaphore;
	layout(binding = 5, std430)	 coherent   buffer  LinkedLists { NodeTypeLL nodes[]; };
	layout(binding = 6, offset = 0)			uniform atomic_uint  next_address;

#if !(INTEL_ordering & NV_interlock)
	layout(binding = 1, r32ui)	 coherent	uniform uimage2DRect image_counter;
	bool  semaphoreAcquire		(								   ) { return (imageLoad			(image_semaphore, ivec2(gl_FragCoord.xy)) == 1U).r ? false :
																			   imageAtomicExchange	(image_semaphore, ivec2(gl_FragCoord.xy), 1U)==0U;}
	void  semaphoreRelease		(								   ) {		   imageStore			(image_semaphore, ivec2(gl_FragCoord.xy), uvec4(0U));}
#endif
	uint getPixelCurrentPageID		()					{ return imageLoad	 (image_next	, ivec2(gl_FragCoord.xy)).x;}
	void setPixelCurrentPageID		(const uint val)	{		 imageStore	 (image_next	, ivec2(gl_FragCoord.xy), uvec4(val,0U,0U,0U));}
	uint getPixelFragCounter		()					{ return imageLoad	 (image_counter	, ivec2(gl_FragCoord.xy)).x;}
	void addPixelFragCounter		(const uint val)	{		 imageStore	 (image_counter	, ivec2(gl_FragCoord.xy), uvec4(val+1U,0U,0U,0U));}

	void main(void)
	{
		uint next_id = 0U, page_id = 0U;
		bool discard_frag = false;

#if		INTEL_ordering
		beginFragmentShaderOrderingINTEL();
#elif	NV_interlock
	    beginInvocationInterlockNV();
#else 
		int	   iter = 0;
		bool   stay_loop = !gl_HelperThreadNV; //or 'true'
		while (stay_loop && iter++ < MAX_ITERATIONS)
		{
			if (semaphoreAcquire())
			{
#endif
				uint counter	 = getPixelFragCounter();
				uint counter_mod = counter % PAGE_SIZE;
				if(  counter_mod == 0U)
				{
					page_id = (atomicCounterIncrement(next_address) + 1U) * PAGE_SIZE;
					if(page_id < nodes.length())
					{
						addPixelFragCounter(counter);

						next_id = getPixelCurrentPageID();
						setPixelCurrentPageID(page_id);
						discard_frag = true;
					}
				}
				else
				{
					addPixelFragCounter(counter);

					page_id = getPixelCurrentPageID() - counter_mod;
					discard_frag = true;
				}

#if		!(INTEL_ordering & NV_interlock)
				semaphoreRelease();
				stay_loop = false;
			}
		}
#elif	NV_interlock
		endInvocationInterlockNV();
#endif

		if(discard_frag)
		{
			nodes[page_id].color = packUnorm4x8(computePixelColor());
			nodes[page_id].depth = gl_FragCoord.z;
			nodes[page_id].next  = next_id;
			discard;
		}
	}
