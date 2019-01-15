#include "define.h"
#if random_discard
#include "noise.h"
	uniform int	width;
	flat in int instanceID;
#endif

	vec4 computePixelColor();

	layout(binding = 0, r32ui) coherent  uniform uimage2DRect	image_counter;
	layout(binding = 4, rg32f) writeonly uniform  image2DArray	image_peel;

	uint  addPixelFragCounter(									 ) {return	imageAtomicAdd	(image_counter	, ivec2(gl_FragCoord.xy), 1U);}
	void  setPixelFragValue	 (const int   coord_z, const vec4 val) {			imageStore	(image_peel		, ivec3(gl_FragCoord.xy, coord_z), val);}

	void main(void)
	{
#if random_discard
		if(distribution(int(gl_FragCoord.x) + width*(int(gl_FragCoord.y) + instanceID)))
			discard;
#endif
		//float C = uintBitsToFloat(packUnorm4x8(computePixelColor()));
		float	C	  = uintBitsToFloat(packUnorm4x8(vec4(1.0f)));
		int		index = int(addPixelFragCounter());

		setPixelFragValue(index, vec4(C, gl_FragCoord.z, 0.0f, 0.0f));
	}