#include "version.h"
#extension GL_EXT_geometry_shader4 : enable 

layout(triangles) in; 
layout(triangle_strip, max_vertices = 3) out;

	 uniform int init_PrimitiveID;
	 uniform int instancesCount;

	 in vec3 pos3_out[];
	 in vec3 color_out[];
	 in vec3 normal_out[];
	 in vec3 instanceColor_out[];
flat in int  instanceID_out[];

#if slow_rendering
	 in  vec4 texCoord_out[];
	 in  vec3 stripCoord_out[];

	 out vec4 texCoord;
	 out vec3 stripCoord;
#endif

	 out vec3 pos3;
	 out vec3 color;
	 out vec3 normal;
	 out vec3 instanceColor;
flat out int  instanceID;

	void main() 
	{
		gl_PrimitiveID = 0; // to remove warning

		for(int i=0; i<gl_in.length(); i++)
		{
			pos3			= pos3_out[i];
			normal			= normal_out[i];
			color			= color_out[i];
			gl_Position		= gl_in[i].gl_Position;
			
			instanceID		= instanceID_out[i] * instancesCount;
			instanceColor	= instanceColor_out[i];

#if slow_rendering			
			texCoord		= texCoord_out[i];
			stripCoord		= stripCoord_out[i];

			gl_PrimitiveID  = gl_PrimitiveIDIn + init_PrimitiveID;
#endif
			EmitVertex();
		}
		
		EndPrimitive();
	}

//in vec3 instanceColor_out[]; 
//out vec3 instanceColor;
//instanceColor	= instanceColor_out[i];