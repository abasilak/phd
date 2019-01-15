#include "version.h"

	layout(location = 0) out vec4 out_frag_color;

	uniform vec4 color;

	void main(void)
	{
		out_frag_color = color;
	}