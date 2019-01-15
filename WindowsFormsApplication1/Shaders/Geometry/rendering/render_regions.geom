#include "version.h"
#extension GL_EXT_geometry_shader4 : enable 

layout(triangles) in; 
layout(line_strip, max_vertices = 6) out;

in  vec3 color_out[];
out vec3 color;

	void main() 
	{
		int e1		=-1;
		int e2		=-1;
		int e3		=-1;
		int count	= 0;
		
		for(int i=0; i<gl_in.length(); i++)
		{
			int j = (i==2) ? 0 : i+1;
			int k = (j==2) ? 0 : j+1;
			if(all(equal(color_out[i], color_out[j])))
			{
				e1 = i;
				e2 = j;
				e3 = k;
				count++;
			}
		}

		color = vec3(0.0f);
		if(count == 0)
		{
			vec4 center = (gl_in[0].gl_Position + gl_in[1].gl_Position + gl_in[2].gl_Position)*0.3333333f;
			for(int i=0; i<gl_in.length(); i++)
			{
				int j = (i==2) ? 0 : i+1;
				
				gl_Position		= (gl_in[i].gl_Position + gl_in[j].gl_Position)*0.5f;
				EmitVertex();

				gl_Position		= vec4(center);
				EmitVertex();
			}
		}
		else if(count == 1)
		{
			gl_Position		= (gl_in[e1].gl_Position + gl_in[e3].gl_Position)*0.5f;
			EmitVertex();

			gl_Position		= (gl_in[e2].gl_Position + gl_in[e3].gl_Position)*0.5f;
			EmitVertex();
		}
		
		EndPrimitive();
	}