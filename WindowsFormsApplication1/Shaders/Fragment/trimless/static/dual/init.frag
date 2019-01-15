#include "define.h"

layout(location = 0, index = 0) out vec4 out_frag_depth;

void main(void)
{
	if  (gl_FrontFacing) out_frag_depth.rg = vec2(-gl_FragCoord.z,-depth_max     );
	else				 out_frag_depth.rg = vec2(-depth_max     ,-gl_FragCoord.z);
}