#include "define.h"

	vec4 computePixelColor();

#if multipass
	layout(binding = 4)					uniform  sampler2DRect	tex_depth;
#endif
	layout(binding = 0, r32f)  writeonly uniform  image2DArray	image_peel_depth;
	layout(binding = 1, r32ui) writeonly uniform uimage2DArray	image_peel_color;
	layout(binding = 2, r32ui) coherent  uniform uimage2DRect	image_counter;

#if multipass
	float getPixelDepth			(							){return texture	(tex_depth, gl_FragCoord.xy).r;}
#endif
	uint  addPixelFragCounter	(							){return imageAtomicIncWrap	(image_counter, ivec2(gl_FragCoord.xy), HEAP_SIZE);}
	void  setPixelFragValues	(const int   coord_z, 
								 const float depth, 
								 const uint  color			){imageStore (image_peel_depth, ivec3(gl_FragCoord.xy, coord_z),  vec4(depth, 0.0f, 0.0f, 0.0f));
															  imageStore (image_peel_color, ivec3(gl_FragCoord.xy, coord_z), uvec4(color, 0U  , 0U  , 0U  ));}

void main(void)
{
#if multipass
	if(gl_FragCoord.z <= getPixelDepth())
		discard;
#endif

#if trimless || trimming || collision
	float frontZ = (gl_FrontFacing) ? gl_FragCoord.z : -gl_FragCoord.z;
#else
	float frontZ = gl_FragCoord.z;
#endif

	int index = int(addPixelFragCounter());
	setPixelFragValues(index, frontZ, packUnorm4x8(computePixelColor()));

	discard;
}