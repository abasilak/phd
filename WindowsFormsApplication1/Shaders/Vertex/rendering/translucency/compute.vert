#include "version.h"

layout(std140) uniform Camera
{
    mat4 projection_matrix;
    mat4 modelview_matrix;
    mat4 normal_matrix;
};
uniform mat4  transformation_matrix;

layout(location = 0) in vec3 in_position;

void main(void)
{
    vec4 pos = vec4(in_position,1.0f);
	gl_Position = projection_matrix * modelview_matrix * transformation_matrix * pos;
}