#include "define.h"

#if random_discard
#include "noise.h"
	
	flat in int instanceID;
#endif

	vec4 computePixelColor();
	
	uniform int width;

	layout(binding = 0, r32ui) coherent uniform uimage2DRect  image_counter;
	layout(binding = 1, r32ui) coherent uniform uimage2DRect  image_semaphore;
	layout(binding = 4, r32f ) coherent uniform  image2DArray image_peel_depth;
	layout(binding = 5, r32ui) coherent uniform uimage2DArray image_peel_color;

#if multipass
	layout(binding = 6)					uniform  sampler2DRect	 tex_depth;
	float getPixelDepthPrevMax  (									) {return texture    (tex_depth, gl_FragCoord.xy).r;}
#endif

	float getPixelFragDepthValue(const int coord_z					 ){return imageLoad	(image_peel_depth, ivec3(gl_FragCoord.xy, coord_z)).r;}
	uint  getPixelFragColorValue(const int coord_z					 ){return imageLoad	(image_peel_color, ivec3(gl_FragCoord.xy, coord_z)).r;}

	void  setPixelFragDepthValue(const int coord_z, const float depth){imageStore (image_peel_depth, ivec3(gl_FragCoord.xy, coord_z),  vec4(depth, 0.0f, 0.0f, 0.0f));}
	void  setPixelFragColorValue(const int coord_z, const uint  color){imageStore (image_peel_color, ivec3(gl_FragCoord.xy, coord_z), uvec4(color, 0U  , 0U  , 0U  ));}

	uint  addPixelFragCounter	(				)					 {return  imageAtomicAdd (image_counter, ivec2(gl_FragCoord.xy), 1U);}

	bool  semaphoreAcquire		(								   ) { return (imageLoad (image_semaphore, ivec2(gl_FragCoord.xy)) == 1U) ? false :
																			   imageAtomicExchange (image_semaphore, ivec2(gl_FragCoord.xy), 1U)==0U;}
	void  semaphoreRelease		(								   ) {		   imageStore(image_semaphore, ivec2(gl_FragCoord.xy), uvec4(0U));}

	vec2 getMaxFromGlobalArray()
	{
		vec2 maxFR = vec2(-1.0f,0.0f);
		
		for(int i=HEAP_SIZE_1n; i>0; i--)
		{
			float Zi = getPixelFragDepthValue(i);
			if(maxFR.g < Zi)
			{
				maxFR.r = i;
				maxFR.g = Zi;
			}
		}

		return maxFR;
	}

	int setMaxFromGlobalArray(float Z)
	{
		int id;
		vec2 maxFR = getMaxFromGlobalArray();

		if(Z < maxFR.g)
		{
			id = int(maxFR.r);
			setPixelFragDepthValue(0, maxFR.g);
			setPixelFragColorValue(0, getPixelFragColorValue(id));
		}
		else
			id = 0;

		return id;
	}

	void insertFragmentArrayMax(int index, float Z, uint C)
	{
		if(index == HEAP_SIZE_1n || Z < getPixelFragDepthValue(0))
		{
			int id = setMaxFromGlobalArray(Z);

			setPixelFragDepthValue(id, Z);
			setPixelFragColorValue(id, C);
		}
	}

	void main(void)
	{
#if multipass
		if(gl_FragCoord.z <= getPixelDepthPrevMax())
			discard;
#endif
			
#if random_discard
		if(distribution(int(gl_FragCoord.x) + width*(int(gl_FragCoord.y) + instanceID)))
			discard;
#endif

		float Z = gl_FragCoord.z;
		uint  C = packUnorm4x8(computePixelColor());

		int id = int(addPixelFragCounter());
		if (id < HEAP_SIZE_1n)
		{
			setPixelFragDepthValue(HEAP_SIZE_1n-id, Z);
			setPixelFragColorValue(HEAP_SIZE_1n-id, C);
		}
		else if (Z < getPixelFragDepthValue(0))
		{
/**/		int iter = 0;
			bool stay_loop = true;
/**/		while (stay_loop && iter++ < MAX_ITERATIONS)
			{
				if (semaphoreAcquire())
				{
					insertFragmentArrayMax(id,Z,C);

					semaphoreRelease();
					stay_loop = false;
				}
			}
		}
		discard;
	}