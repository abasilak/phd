#include "version.h"

#define cTEX_COORDS  12

	layout(binding = 0, std140) uniform Camera
	{
		mat4 projection_matrix;
		mat4 modelview_matrix;
		mat4 normal_matrix;
	};

	uniform mat4  transformation_matrix;
	uniform int	  ColoringMode;

	layout(location = 0) in vec3 in_position;
	layout(location = 2) in vec3 in_color;
	layout(location = 3) in vec2 in_texture;

	out vec3 color_out;

	void main(void)
	{
		vec4 pos	 = vec4(in_position,1.0f);
	
		if(ColoringMode == cTEX_COORDS) color_out  = vec3(in_texture,1.0f);
		else							color_out  = in_color;

		pos			= transformation_matrix*pos;
		pos			= modelview_matrix * pos;
		pos			= projection_matrix *pos;
		gl_Position = pos;
	}