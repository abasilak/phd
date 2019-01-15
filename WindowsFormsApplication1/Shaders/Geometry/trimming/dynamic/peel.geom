#version 420 core
#extension GL_EXT_geometry_shader4 : enable 

layout(triangles) in; 
layout(triangle_strip, max_vertices = 6) out;

in vec4  pos_prev_out[];
flat in int isEqual_out[];

in vec3 pos3_out[];
in vec3 color_out[];
in vec3 normal_out[];
in vec4 texCoord_out[];
in vec3 stripCoord_out[];
in vec3 instanceColor_out[];
flat in int instanceID_out[];

out vec3 pos3;
out vec3 color;
out vec3 normal;
out vec4 texCoord;
out vec3 stripCoord;
out vec3 instanceColor;
flat out int instanceID;
flat out uint classification;

uniform bool Trimming;

	void main() 
	{
		int i;
		bool st=true;

		if(Trimming)
			for(i=0; i<gl_in.length(); i++)
				if(isEqual_out[i]==0)
				{
					st=false;
					break;
				}	
		
		gl_PrimitiveID = 0; // to remove warning
		for(i=0; i<gl_in.length(); i++)
		{
			pos3			= pos3_out[i];
			color			= color_out[i];
			classification  = st ? 1U : 2U; // 1 -> PREV + NEW, 2 -> NEW
			normal			= normal_out[i];
			texCoord		= texCoord_out[i];
			stripCoord		= stripCoord_out[i];
			instanceColor	= instanceColor_out[i];
			instanceID		= instanceID_out[i];
			gl_Position		= gl_in[i].gl_Position;
			gl_PrimitiveID  = gl_PrimitiveIDIn;
			EmitVertex();
		}
		EndPrimitive();

		if(!st)
		{
			for(i=0; i<gl_in.length(); i++)
			{
				gl_Position		= pos_prev_out   [i];
				classification  = 0U;	// 0 -> PREV
				EmitVertex();
			}
			EndPrimitive();
		}
	}