
float intNoise(int n)
{
	n = (n << 13) ^ n;
	return 1.0 - float( (n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0;
}

bool distribution(int id, float t)
{
	return (intNoise(id)*0.5 + 0.5 < t) ? true : false;
	//return false;
}