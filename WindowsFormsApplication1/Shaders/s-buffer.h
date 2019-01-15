#define inverse			0				// {S-Buffer}
#define COUNTERS		32				// {S-Buffer}
#define COUNTERS_2d		COUNTERS >> 1	// {S-Buffer}

// for resolution : 1024 x 1024
#define COUNTERS_X		256				// {S-Buffer}
#define COUNTERS_Y		192				// {S-Buffer}
#define COUNTERS_W		4				// {S-Buffer}

int hashFunction(ivec2 coords)
{
	// [2012] EG paper's hash function: 
	//return (coords.x + 1024*coords.y) % COUNTERS;

	ivec2 tile = ivec2(coords.x / COUNTERS_X, coords.y / COUNTERS_Y);
	return tile.x * COUNTERS_W + tile.y;
}