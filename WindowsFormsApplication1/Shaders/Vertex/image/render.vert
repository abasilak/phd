#include "version.h"

uniform mat4 modelview_matrix;

layout(location = 0) in vec3 in_position;

void main(void)
{
	 gl_Position = modelview_matrix * vec4(in_position, 1.0);
}