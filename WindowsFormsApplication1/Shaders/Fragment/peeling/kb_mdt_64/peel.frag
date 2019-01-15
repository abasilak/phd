#include "define.h"
#extension GL_NV_shader_atomic_int64 : enable
#if random_discard
#include "noise.h"
	flat in int instanceID;
#endif	

	uniform int	width;

	vec4 computePixelColor();

	layout(binding = 0, std430)	coherent buffer myKbuffer { uint64_t nodes[]; };

	uint hi32 (const uint64_t val) {return uint(val>>32);}

	void main(void)
	{
#if random_discard
		if(distribution(int(gl_FragCoord.x) + width*(int(gl_FragCoord.y) + instanceID)))
			discard;
#endif
		int index = (int(gl_FragCoord.x) + width*int(gl_FragCoord.y))*HEAP_SIZE;

		uint C_32 = packUnorm4x8(computePixelColor());
		//uint C_32 = packUnorm4x8(vec4(1.0f));

		uint zOld_32, zTest_32, Z_32 = floatBitsToUint(gl_FragCoord.z);
		uint64_t zOld_64, zTest_64 = packUint2x32 (uvec2(C_32, Z_32));
#if early_clipping_rest
		if(Z_32 <= hi32(nodes[HEAP_SIZE_1n])) // [my idea]
#endif
		for(int i=0; i<HEAP_SIZE; i++)
		{
			zOld_64 = atomicMin (nodes[index + i], zTest_64);
				
			zOld_32 = hi32(zOld_64); zTest_32 = hi32(zTest_64);
			if (zOld_32 == 0xFFFFFFFFU || zOld_32 == zTest_32)
				break;
			else if (zOld_32 > zTest_32)
				zTest_64 = zOld_64;
		}
	}