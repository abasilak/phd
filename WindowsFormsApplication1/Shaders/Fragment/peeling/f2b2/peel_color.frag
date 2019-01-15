#include "define.h"

// INCLUDE
vec4 computePixelColor();

layout(location = 0, index = 0) out  vec4 out_frag_color;

#if early_Z
	layout (early_fragment_tests) in;
#endif

void main(void)
{
	out_frag_color = computePixelColor();
}