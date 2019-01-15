#include "define.h"

// IN
#if multisample
	uniform sampler2DMS	  tex_depth;
	uniform sampler2DMS	  tex_count;
#else
	uniform sampler2DRect tex_depth;
	uniform sampler2DRect tex_count;
#endif

// OUT
layout(location = 0, index = 0) out vec4 out_frag_depth;

void main(void)
{
	 vec2 depth;
	ivec2 count;
	
#if multisample
	depth =		  texelFetch(tex_depth, ivec2(gl_FragCoord.xy), gl_SampleID).xy;
	count = ivec2(texelFetch(tex_count, ivec2(gl_FragCoord.xy), gl_SampleID).xy);
#else
	depth =		  texture(tex_depth, gl_FragCoord.xy).xy;
	count = ivec2(texture(tex_count, gl_FragCoord.xy).xy);
#endif
	float depth_near = -depth.x;
	float depth_far	 =  depth.y;

	if (gl_FragCoord.z < depth_near || gl_FragCoord.z > depth_far)
		discard;
	out_frag_depth.xy	= vec2(-gl_FragCoord.z, gl_FragCoord.z);

	// Front		
	if	   (count.x <= 1 && (gl_FragCoord.z == depth_near || gl_FragCoord.z ==  depth_far))
		out_frag_depth.x = -depth_max;
	else if(count.x >  1 && gl_FragCoord.z != depth_near)
		out_frag_depth.x = -depth_max;
	// Back
	if	   (count.y  <= 1 && (gl_FragCoord.z == depth_near || gl_FragCoord.z == depth_far))
		out_frag_depth.y = -depth_max;
	else if(count.y  >  1 && gl_FragCoord.z != depth_far)
		out_frag_depth.y = -depth_max;
}