#include "define.h"
#include "s-buffer.h"

layout(binding = 5, std430)	coherent  buffer  final_address { uint final[]; };

void main(void)
{
	int id = int(gl_FragCoord.y);
	if(id < COUNTERS)
	{
		int  k   = 0;
		uint sum = 0U;
		for(int i = id; i > k; i--)
			sum += final[i-1];
		final[id+COUNTERS] = sum;
	}
}