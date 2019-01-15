#include "define.h"

	vec4 computePixelColor();

	layout(binding = 1, r32ui) coherent  uniform uimage2DRect image_next;
	layout(binding = 4, rg32f) writeonly uniform imageBuffer  image_peel;

	uint addPixelNextAddress  (						){ return imageAtomicAdd(image_next, ivec2(gl_FragCoord.xy), 1U);}
	void sharedPoolSetValue	  (uint index, vec4 val ){		  imageStore	(image_peel, int(index), val);}

	void main(void)
	{
		vec4  value		= vec4(uintBitsToFloat(packUnorm4x8(computePixelColor())), gl_FragCoord.z, 0.0f, 0.0f);
		uint  page_id	= addPixelNextAddress();
		sharedPoolSetValue (page_id, value);
	}
