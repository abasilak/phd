#include "define.h"
#extension GL_EXT_gpu_shader4 : enable

// INCLUDE
vec4 computePixelColor();
// IN
#if multisample
	uniform sampler2DMS	  tex_depth;
	uniform sampler2DMS	  tex_count;
	uniform sampler2DMS	  tex_id;
#else
	uniform sampler2DRect tex_depth;
	uniform sampler2DRect tex_count;
	uniform sampler2DRect tex_id;
#endif
//
flat in int instanceID;
uniform int init_PrimitiveID;
// OUT
layout(location = 0, index = 0) out vec4 out_frag_depth;
layout(location = 1, index = 0) out vec4 out_frag_color_front;
layout(location = 2, index = 0) out vec4 out_frag_color_back;

void main(void)
{
	 vec2 depth;
	ivec2 count,id;

#if multisample
	depth =		  texelFetch(tex_depth, ivec2(gl_FragCoord.xy), gl_SampleID).xy;
	count = ivec2(texelFetch(tex_count, ivec2(gl_FragCoord.xy), gl_SampleID).xy);
	id    = ivec2(texelFetch(tex_id   , ivec2(gl_FragCoord.xy), gl_SampleID).xy);
#else
	depth =		  texture(tex_depth, gl_FragCoord.xy).xy;
	count = ivec2(texture(tex_count, gl_FragCoord.xy).xy);
	id    = ivec2(texture(tex_id   , gl_FragCoord.xy).xy);
#endif

	float depth_near = -depth.x;
	float depth_far	 =  depth.y;

	if (gl_FragCoord.z < depth_near || gl_FragCoord.z > depth_far)
		discard;

	out_frag_color_front = (				gl_PrimitiveID == id.x) ? computePixelColor() : vec4(0.0);
	out_frag_color_back  = (id.x != id.y && gl_PrimitiveID == id.y) ? computePixelColor() : vec4(0.0);

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