#include "define.h"
#include "s-buffer.h"

	coherent uniform uint *next_address[COUNTERS];

	void setSharedNextAddress (int j, uint val)	 {(*next_address[j]) = val;}

	void main(void)
	{
		int id = int(gl_FragCoord.y);
		if(id < COUNTERS)
			setSharedNextAddress(id, 0U);
	}