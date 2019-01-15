//------
#define sigma	  20.0f
//------

	uniform int   layer;
	uniform bool  closest;
	uniform bool  correctAlpha;
	uniform bool  useCoplanarity;
	uniform bool  useTransparency;
	uniform bool  useTranslucency;
	uniform vec4  color_background;

#if trimless || trimming
	uniform float cappingPlane;
	uniform float cappingAngle;

	// COLORS
	const vec3 BLUE		= vec3(0.0,0.0,1.0); 
	const vec3 CYAN		= vec3(0.0,1.0,1.0); 
	const vec3 PURPLE	= vec3(1.0,0.5,0.5); 
	const vec3 YELLOW	= vec3(1.0,1.0,0.0);

	bool checkRule		   (const int val)	{return (int(abs(ceil(float(val)*0.5f)))%2 == 1) ? true : false;}
#endif

#if   trimless
	vec4 resolveTrim(const int num)
	{
		int   O = 0;
		bool  front, final_front = true;
		vec2  Ci, maxC = vec2(0.0f,1.0f);

		float k = 1.0f - float(gl_FragCoord.x)/cappingAngle;
		float capping = cappingPlane*k;
	
		for(int i=0; i<num; i++)
		{
			Ci = fragments[i];

			front = (Ci.g > 0.0f) ? true : false;
			Ci.g  = abs(Ci.g);

			if	   (Ci.g <= capping)
				O += front ? 1 : -1;
			else if(Ci.g < maxC.g)
			{
				final_front = front;
				maxC = Ci;
			}
		}

		bool rule  = checkRule(O);

		vec4  color;
		if		( rule  && !final_front )  color.rgb = BLUE; 
		else if ( rule  &&  final_front )  color.rgb = CYAN; 
		else if (!rule  && !final_front )  color.rgb = PURPLE; 
		else							   color.rgb = YELLOW; 

		if(maxC.g == 1.0f)
			return color_background;
		else
		{
			vec4 C = unpackUnorm4x8(colors[int(maxC.r)]);
			return vec4(color.rgb * C.rgb, 1.0f);
		}
	}
#elif trimming
	vec4 resolveTrim(const int num, const bool sorted)
	{
		if(!sorted)
			sort(num);

		int   O=0;
		vec2  Ci;
		bool  rule, front;
		float thickness=0.0f, prevZ = 0.0f, Zi1;
		vec4  c, color, finalColor = (useTransparency) ? vec4(0.0f) : color_background;
	
		bool  first = true;
		bool  prev_rule=false;
	
		float k = 1.0f - float(gl_FragCoord.x)/cappingAngle;
		float capping = cappingPlane*k;
	
		for(int i=0; i<num; i++)
		{
			Ci = fragments[i];

			front = (Ci.g > 0.0f) ? true : false;
			Ci.g = abs(Ci.g);

			if(Ci.g <= capping)
			{
				O += front ? 1 : -1;

				if(useTransparency)
				{
					color =	unpackUnorm4x8(colors[int(Ci.r)]);
					if(correctAlpha)
					{
						if(i%2 == num%2)
						{
							Zi1 = fragments[i+1].g;
							Zi1 = abs(Zi1);
							thickness = (Zi1-Ci.g)*0.5f;
						}
						color.w	   = 1.0f - pow(1.0f-color.w, thickness*sigma);
						color.rgb *= color.a;
					}
					finalColor += color*(1.0f-finalColor.a);
				}
			}
			else
			{
				if(first)
				{
					first = false;
					prev_rule = checkRule(O);
				}

				O += front ? 1 : -1;
				rule = checkRule(O);

				if(prev_rule != rule)
				{	
					rule	= prev_rule;
					if		( rule  && !front )  color.rgb = BLUE; 
					else if ( rule  &&  front )  color.rgb = CYAN; 
					else if (!rule  && !front )  color.rgb = PURPLE; 
					else						 color.rgb = YELLOW; 
								
					c =	unpackUnorm4x8(colors[int(Ci.r)]);
					color = vec4(color.rgb * c.rgb, 1.0f);
					finalColor = useTransparency ? finalColor + color : color;
					break;
				}
			}
		}

		if(useTransparency)
			finalColor += color_background*(1.0f-finalColor.a);
		return finalColor;
	}

#elif collision
	vec4 resolveCollision(const int num, const bool sorted)
	{
		if(!sorted)
			sort(num);

		vec2  Ci;
		bool  front;
		bool  prev_front;
		float thicknessAlpha=0.0f, Zi1;
		vec4  c, color, finalColor = vec4(0.0f);

		for(int i=0; i<num; i++)
		{
			Ci = fragments[i];
			c  = unpackUnorm4x8(colors[int(Ci.r)]);

			front = (Ci.g > 0.0f) ? true : false;

			if(prev_front != front || i == 0)
			{
				prev_front = front;
				
				color = c;
				if(correctAlpha)
				{
					Ci.g = abs(Ci.g);
					if(i==0)
						thicknessAlpha = Ci.g*0.5f;

					if(i%2 == num%2)
					{
						Zi1 = fragments[i+1].g;
						Zi1 = abs(Zi1);
						thicknessAlpha = (Zi1-Ci.g)*0.5f;
					}
					color.w	   = 1.0f - pow(1.0f-color.w, thicknessAlpha*sigma);
					color.rgb *= color.a;
				}
				finalColor += color*(1.0f-finalColor.a);
			}
			else
			{
				color.rgb  = vec3(1.0,0.0,0.0); 
				color	   = vec4(color.rgb * c.rgb, 1.0f);
				finalColor = finalColor + color;
				break;
			}
		}

		finalColor += color_background*(1.0f-finalColor.a);

		return finalColor;	
	}
#else
	vec4 resolveLayer(const int num, const bool sorted)
	{
		if(!sorted)
			sort(num);
		return unpackUnorm4x8(colors[int(fragments[layer].x)]);
	}

	vec4 resolveCoplanarity(const int num, const bool sorted)
	{
		if(!sorted)
			sort(num);

		int  count;
		vec2 Ci,Ci1;
		vec4 C0 = vec4(0.0f), color;
		bool coplanar = false;

		int i=layer;

		count = 0;
		while (i<num)
		{
			Ci = fragments[i];
	
			if(i==layer)
				C0 = unpackUnorm4x8(colors[int(Ci.r)]);

			int c = 0;
			for(int j=i+1; j<num; j++)
			{
				Ci1 = fragments[j];
				if(Ci.g == Ci1.g)
				{
					c++;
					count++;
					coplanar = true;
				}
				else
					break;
			}
			i += c + 1;
			//if(coplanar)
				//break;
		}

		if		(count == 1) color = vec4(1.0, 0.0, 0.0, 1.0f); // Red
		else if	(count == 2) color = vec4(0.0, 0.0, 1.0, 1.0f); // Blue
		else if	(count == 3) color = vec4(0.0, 1.0, 0.0, 1.0f); // Green
		else if	(count == 4) color = vec4(0.0, 1.0, 1.0, 1.0f); // Cyan
		else if	(count == 5) color = vec4(0.3, 1.0, 0.7, 1.0f); // ...
		else if	(count == 6) color = vec4(1.0, 0.0, 1.0, 1.0f); // Fuchsia 
		else if	(count == 7) color = vec4(1.0, 1.0, 0.0, 1.0f); // Yellow
		else if	(count == 8) color = vec4(0.65, 0.165, 0.165, 1.0f); // Brown
		else if	(count == 9) color = vec4(1.0, 0.65, 0.0, 1.0f); // Orange
		else			     color = C0;

		return color;
	}

	vec4 resolveAlphaBlend(const int num, const bool sorted)
	{
		//if(!sorted)
			sort(num);
	
		vec2  Ci;
		vec4  C0=vec4(0.0f), color, finalColor=vec4(0.0f);
		float thickness=0.0f, prevZ = 0.0f, thicknessAlpha=0.0f, Zi1;
	
		for(int i=0; i<num; i++)
		{
			Ci = fragments[i];
	
			if(Ci.g == 1.0f)
				break;

			if(i==0)
			{
				C0 = unpackUnorm4x8(colors[int(Ci.r)]);
				if(correctAlpha) thicknessAlpha = Ci.g*0.5f;
			}

		//	if(useTranslucency)
		//	{
		//		if(i%2==1) thickness += Ci.g - prevZ;
		//		prevZ = Ci.g;
		//	}
//			else
			if(useTransparency)
			{
				color = unpackUnorm4x8(colors[int(Ci.r)]);
				if(correctAlpha)
				{
					if(i%2 == num%2)
					{
						Zi1 = fragments[i+1].g;
						Zi1 = abs(Zi1);
						thicknessAlpha = (Zi1-Ci.g)*0.5f;
					}
					color.w		= 1.0f - pow(1.0f - color.w, thicknessAlpha*sigma);
					color.rgb  *= color.a;
				}
				finalColor += color*(1.0f-finalColor.a);
			}
		}

		//if(useTransparency)
			finalColor += color_background*(1.0f-finalColor.a);
		//else  
		{
		//	const float Ka    = 0.8f;
		//	const vec4  jade  = vec4(0.4, 0.14, 0.11, 1.0)*8.0f;
		//	const vec4  green = vec4(0.3, 0.7 , 0.1 , 1.0)*4.0f;
		
		//	finalColor = (Ka*exp(-sigma*thickness)*vec4(1.0f) + C0)*jade;	
		}

		return finalColor;
	}

	vec4 resolveClosest(const int num, const bool sorted)
	{
		if(sorted)
			return unpackUnorm4x8(colors[0]);
		else
		{
			vec2 Ci, maxC = vec2(0.0f,1.0f);

			for(int i=0; i<num; i++)
			{
				Ci = fragments[i];
				if(Ci.g < maxC.g)
					maxC = Ci;
			}
			return unpackUnorm4x8(colors[int(maxC.r)]);
		}
	}
#endif
	vec4 resolve(const int num, const bool sorted)
{
	vec4 c = vec4(0.0f);
#if   trimless
	c = resolveTrim(num);
#elif trimming
	c = resolveTrim(num, sorted);
#elif collision
	c = resolveCollision(num, sorted);
#else
	if(useTransparency || useTranslucency)
		c = resolveAlphaBlend(num, sorted);
	//else if (useCoplanarity)
		//c = resolveCoplanarity(num, sorted);
	else
	{
		//if(closest)
			//return resolveClosest (num, sorted);
		if(layer+1 > num)
			discard;

		c = resolveLayer(num, sorted);
	}
#endif
	return c;
}