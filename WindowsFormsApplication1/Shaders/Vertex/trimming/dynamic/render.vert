#include "version.h"

#define tOBJECT_PLANE 0	
#define tEYE_PLANE	  1
#define tSPHERICAL	  2
#define tREFLECTIVE	  3
#define tNORMAL	 	  4

#define cTEXTURE	  5

layout(binding = 0, std140) uniform Camera
{
    mat4 projection_matrix;
    mat4 modelview_matrix;
    mat4 normal_matrix;
};

uniform bool  Instancing;
uniform float translation;
uniform mat4  transformation_matrix;
uniform int	TexturingPar;
uniform int	ColoringMode;

layout(location = 0) in vec3 in_position;
layout(location = 1) in vec3 in_normal; 
layout(location = 2) in vec3 in_color;

out vec4 texCoord_out;
out vec3 stripCoord_out;
out vec3 instanceColor_out;
flat out int instanceID_out;

out vec3 pos3_out;
out vec3 color_out;
out vec3 normal_out;

//////////////////////
uniform bool Trimming;
layout(location = 3) in vec3 in_position_prev;
out vec4  pos_prev_out;
flat out int isEqual_out;
/////////////////////

vec2 SphereMap (const in vec3 U, const in vec3 N)
{
	vec3 R = reflect(U,N);
	R.z   += 1.0f;
	R      = normalize(R);
	return R.xy*0.5f + 0.5f;
}

vec3 ReflectionMap(const in vec3 U, const in vec3 N)
{
	return reflect(U,N);
}

void main(void)
{
	vec4 pos;
	color_out  = in_color;
  	normal_out = normalize(mat3(normal_matrix) * in_normal);
    pos		   = transformation_matrix*vec4(in_position,1.0f);

	/*instanceID_out = gl_InstanceID;
	if(Instancing)
	{
		pos.xy += float(gl_InstanceID)*translation;
		
		vec3 c= vec3(0.0f);
		int k = gl_InstanceID%8;
		switch(k)
		{
			case 0 : c = vec3(0.0,0.0,1.0);break;		
			case 1 : c = vec3(0.0,1.0,0.0);break;
			case 2 : c = vec3(0.0,1.0,1.0);break;
			case 3 : c = vec3(1.0,0.0,0.0);break;
			case 4 : c = vec3(1.0,0.0,1.0);break;
			case 5 : c = vec3(1.0,1.0,0.0);break;
			case 6 : c = vec3(1.0,1.0,1.0);break;
			case 7 : c = vec3(0.5,0.5,0.5);break;
		}
		instanceColor_out = c;
	}*/
	
	stripCoord_out.xy = in_position.xy;
	stripCoord_out.z  = abs(normal_out.z);

    pos  = modelview_matrix * pos;
    pos3_out = vec3(pos) / pos.w;

	if(ColoringMode == cTEXTURE)
	{
		if	   (TexturingPar == tOBJECT_PLANE) texCoord_out = vec4(1.0f); // gl_TexCoord[0].s = dot( gl_Vertex, gl_ObjectPlaneS[0] );
		else if(TexturingPar == tEYE_PLANE	 ) texCoord_out = vec4(1.0f); // gl_TexCoord[0].s = dot( gl_Vertex, gl_EyePlaneS[0] );
		else if(TexturingPar == tSPHERICAL	 ) texCoord_out = vec4(SphereMap	(normalize(pos3_out),normal_out),0.0f,1.0f);
		else if(TexturingPar == tREFLECTIVE	 ) texCoord_out = vec4(ReflectionMap(normalize(pos3_out),normal_out)	 ,1.0f);
		else if(TexturingPar == tNORMAL		 ) texCoord_out = vec4(normal_out, 1.0f);
	}

	isEqual_out	= 1;
	if(Trimming)
	{
		pos_prev_out	= projection_matrix * modelview_matrix * vec4(in_position_prev,1.0f);
		isEqual_out		= all(equal(in_position.xyz,in_position_prev.xyz)) ? 1 : 0;
	}
	gl_Position = projection_matrix * pos;
}