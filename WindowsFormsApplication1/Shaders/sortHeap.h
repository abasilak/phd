	vec2  fragments [HEAP_SIZE];
	uint  fragmentsC[HEAP_SIZE];
	uint  colors	[HEAP_SIZE];

	int leftHeap (const int j) {return (j<<1) + 1;}
	
	uint sort (const int num)
	{
		uint C0 = fragmentsC[0];
		if (num == 1)
			return C0;

		int counter = num-1;
		float Z1 = fragments [counter].g;
		uint  C  = fragmentsC[counter];

		int P, I = 0;
		int L = 1;
		int R = 2;

		for (int i=0; i<log2(counter); i++)
		{
			float Z = Z1;
			if(L < counter && Z < fragments[L].g)
			{
				P = L;
				Z = fragments[L].g;
			}
			else
				P = I;

			if(R < counter && Z < fragments[R].g)
				P = R;

			if(P != I)
			{
				fragments [I].g = fragments [P].g;
				fragmentsC[I]   = fragmentsC[P];

				I = P; 
				L = leftHeap(I);
				R = L+1;
			}
			else
				break;
		}

		fragments [I].g = Z1;
		fragmentsC[I]   = C;

		return C0;
	}
