#include "version.h"
#include "noise.h"

#if slow_rendering

#define tOBJECT_PLANE 0	
#define tEYE_PLANE	  1
#define tSPHERICAL	  2
#define tREFLECTIVE	  3
#define tNORMAL	 	  4

#define cTEXTURE	  5
#define cTEX_COORDS  12

	uniform int		ColoringMode;
	uniform int		TexturingPar;

	layout(location = 3) in vec2  in_texture;

	out vec4 texCoord_out;
	out vec3 stripCoord_out;

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
#endif

	layout(binding = 0, std140) uniform Camera
	{
		mat4 projection_matrix;
		mat4 modelview_matrix;
		mat4 normal_matrix;
	};
	uniform mat4  transformation_matrix;

	uniform bool  Instancing;
	uniform bool  randomBias;
	uniform int	  instancesCount;
	uniform vec3  instance_translation;

	layout(location = 0) in vec3  in_position;
	layout(location = 1) in vec3  in_normal; 
	layout(location = 2) in vec3  in_color;

	 out vec3 pos3_out;
	 out vec3 color_out;
	 out vec3 normal_out;
	 out vec3 instanceColor_out;
flat out int  instanceID_out;

void main(void)
{
	vec4 pos	 = vec4(in_position,1.0f);
	vec3 nor	 = vec3(in_normal);

	if(Instancing)
	{
		instanceID_out   = gl_InstanceID;
		float instanceID = (randomBias) ? intNoise(gl_VertexID*instancesCount + gl_InstanceID) : float(gl_InstanceID);
				
		pos.x  += instanceID*instance_translation.x;
		pos.y  += instanceID*instance_translation.y;
		pos.z  += instanceID*instance_translation.z;

		vec3 c = vec3(0.0f);
		int  k = gl_InstanceID%8;
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
	}

	pos			= transformation_matrix * pos;
    pos			= modelview_matrix * pos;
    pos3_out	= vec3(pos) / pos.w;
	gl_Position = pos;

	normal_out = normalize(mat3(normal_matrix) * nor);

#if slow_rendering
	color_out  = (ColoringMode == cTEX_COORDS) ? vec3(in_texture,1.0f) : in_color;
	if(ColoringMode == cTEXTURE)
	{
		if	   (TexturingPar == tOBJECT_PLANE) texCoord_out = vec4(1.0f); // gl_TexCoord[0].s = dot( gl_Vertex, gl_ObjectPlaneS[0] );
		else if(TexturingPar == tEYE_PLANE	 ) texCoord_out = vec4(1.0f); // gl_TexCoord[0].s = dot( gl_Vertex, gl_EyePlaneS[0] );
		else if(TexturingPar == tSPHERICAL	 ) texCoord_out = vec4(SphereMap	(normalize(pos3_out),normal_out),0.0f,1.0f);
		else if(TexturingPar == tREFLECTIVE	 ) texCoord_out = vec4(ReflectionMap(normalize(pos3_out),normal_out)	 ,1.0f);
		else if(TexturingPar == tNORMAL		 ) texCoord_out = vec4(normal_out, 1.0f);
	}

	stripCoord_out.xy = in_position.xy;
	stripCoord_out.z  = abs(normal_out.z);
#else
	color_out  = in_color;
#endif
}