#include "define.h"

// INCLUDE
	vec4 computePixelColor();
// IN
	uniform uint total_fragments;

	layout(binding = 0, r32ui) coherent uniform uimage2DRect  image_next;
	layout(binding = 1, r32ui) coherent uniform uimage2DRect  image_counter;
	layout(binding = 3, r32ui) coherent uniform uimage2DRect  image_semaphore;
	layout(binding = 4, r32f ) writeonly uniform  imageBuffer image_peel_depth;
	layout(binding = 5, r32ui) writeonly uniform uimageBuffer image_peel_color;
	layout(binding = 6, r32ui) writeonly uniform uimageBuffer image_pointers;
	layout(binding = 7, offset = 0)		 uniform atomic_uint  next_address;

	uint getPixelCurrentPageID		()					{ return imageLoad			(image_next	  , ivec2(gl_FragCoord.xy)).x;}
	uint getPixelFragCounter		()					{ return imageLoad			(image_counter, ivec2(gl_FragCoord.xy)).x;}
	void addPixelFragCounter		(const uint val)	{		 imageAtomicAdd		(image_counter, ivec2(gl_FragCoord.xy), val);}
	uint exchangePixelCurrentPageID	(const uint val)	{ return imageAtomicExchange(image_next   , ivec2(gl_FragCoord.xy), val);}
	void sharedPoolSetDepthValue	(const uint index  , const  vec4 val)	{		 imageStore(image_peel_depth, (int)index, val);}
	void sharedPoolSetColorValue	(const uint index  , const uvec4 val)	{		 imageStore(image_peel_color, (int)index, val);}
	void sharedPoolSetLink			(const uint index  , const uint  val)	{		 imageStore(image_pointers  , (int)index, uvec4(val, 0U, 0U, 0U) );}

	bool semaphoreAcquire			(){ return imageAtomicExchange(image_semaphore, ivec2(gl_FragCoord.xy), 1U)==0U;}
	void semaphoreRelease			(){		   imageStore	      (image_semaphore, ivec2(gl_FragCoord.xy), uvec4(0U));}

void main(void)
{
	bool  discard_frag = false, leave_loop = true;
	while (leave_loop)
	{
		if (semaphoreAcquire())
		{
			uint  counter	  = getPixelFragCounter  ();
			uint  counter_mod = counter % PAGE_SIZE;

			if(counter_mod == 0U)
			{
				uint page_id = atomicCounterIncrement(next_address) * PAGE_SIZE;

				if(page_id < total_fragments)
				{
					addPixelFragCounter(1U);
					sharedPoolSetLink  (page_id, exchangePixelCurrentPageID(page_id));
					sharedPoolSetDepthValue (page_id,  vec4(gl_FragCoord.z, 0.0f, 0.0f, 0.0f));
					sharedPoolSetColorValue (page_id, uvec4(packUnorm4x8(computePixelColor()), 0U, 0U, 0U));
					
					discard_frag = true;
				}
			}
			else
			{
				addPixelFragCounter(1U);
				
				uint page_id = getPixelCurrentPageID() - counter_mod;
				sharedPoolSetDepthValue (page_id,  vec4(gl_FragCoord.z, 0.0f, 0.0f, 0.0f));
				sharedPoolSetColorValue (page_id, uvec4(packUnorm4x8(computePixelColor()), 0U, 0U, 0U));

				discard_frag = true;
			}

			semaphoreRelease();

			leave_loop = false;
		}
	}

	if(discard_frag)
		discard;
}