#include "define.h"

	vec4 computePixelColor();

	uniform int width;

	layout(binding = 0, r32ui) coherent  uniform uimage2DRect image_counter;
	layout(binding = 1, rg32f) writeonly uniform  imageBuffer image_peel;

	uint getPixelNextAddress (					  ) { return uint((gl_FragCoord.x + width*gl_FragCoord.y)*ARRAY_SIZE);}
	uint addPixelFragCounter (					  ) { return imageAtomicAdd (image_counter, ivec2(gl_FragCoord.xy), 1U);}
	void sharedPoolSetValue	 (uint index, vec4 val) {		 imageStore		(image_peel   , int(index), val);}

	void main(void)
	{
		vec4  value   = vec4(uintBitsToFloat(packUnorm4x8(computePixelColor())), gl_FragCoord.z, 0.0f, 0.0f);
		uint  page_id = getPixelNextAddress() + addPixelFragCounter();;
		sharedPoolSetValue (page_id, value);
	}
