#include "version.h"
//#include "heatmap.h"

uniform int	 ColoringMode;
uniform bool Instancing;
in vec3 instanceColor;

#if slow_rendering

struct Light_Par
{
    vec4 position;
    vec4 ambient;
    vec4 diffuse;
    vec4 specular;
    float constantAttenuation;
    float linearAttenuation;
    float quadraticAttenuation;
    float Ka;
    float Kd;
    float Ks;
};

layout(std140, binding = 1) uniform Light
{
    Light_Par light;
};

uniform int	 IlluminationMode;

in vec3 pos3;
in vec3 normal;
in vec3 color;

void Illumination(in int model, in bool mat_used, in vec3 E, in vec3 P, in vec3 N, out vec4 final_color);

#define tREPLACE	0	
#define tMODULATE	1
#define tDECAL		2
#define tBLEND		3
#define tADD		4

#define cNONE		0	
#define cVERTEX		1
#define cFACET		2
#define cMATERIAL	3
#define cSTRIPS		4
#define cTEXTURE	5
#define cNORMAL		6
#define cDEPTH		7
#define cLUMIN		8
#define cTOON		9
#define cXRAY		10
#define cGOUCH		11
#define cTEX_COORDS 12

// 1 : Vertex Color
// 2 : Facet  Color
// 3 : Material

layout(binding = 2)     uniform samplerBuffer	tex_colorsF;

// 4 : Strips 
uniform int				stripXY;
uniform float			stripSize;
uniform vec4			stripColor;
// 5 : Texture
//layout(binding = 2)     
uniform sampler2D	    tex_model;

uniform int				TexturingApp;
// 6 : Normal
// 7 : Depth
uniform float			zNear;
uniform float			zFar;
// 10 : Xray
uniform float			edgeFalloff;
// 11 : Gouch
uniform float			diffuseWarm;
uniform float			diffuseCool;

in vec3 stripCoord;
in vec4 texCoord;

float LinearizeDepth(float z)
{
  return (2.0f * zNear) / (zFar + zNear - z * (zFar - zNear));
}

vec3 shadeStrips()
{
	float coord = (stripXY==0) ? stripCoord.x : stripCoord.y;
	float i		= floor(coord*stripSize);
	
	vec3 col	= fract(i*0.5f) == 0.0f ? ((Instancing) ? instanceColor : stripColor.rgb) : vec3(1.0f);
	
	col *= stripCoord.z;
	return col;
}

vec4 computePixelColor()
{
	bool  material_used=false;
	if	   (ColoringMode == cNONE && !Instancing) return vec4(0,0,0,1); // itan (1.0f) to evala 0.0 gia Line Rendering
	else if(ColoringMode == cMATERIAL || ColoringMode == cLUMIN || ColoringMode == cTOON || ColoringMode == cNORMAL || ColoringMode == cDEPTH || ColoringMode == cXRAY || ColoringMode == cGOUCH)
	material_used = true;
		
	vec3 eye = -normalize(pos3);
	vec3 n   =  normalize(normal);
	if(!gl_FrontFacing) n = -n;

	vec4  final_color;
	Illumination(IlluminationMode, material_used, eye, pos3, n, final_color);

	if(!material_used)
	{
		vec3 c = vec3(1.0);
		if		(ColoringMode == cSTRIPS ) c = shadeStrips();
		else if (Instancing				 ) c = instanceColor;
		else if	(ColoringMode == cVERTEX || ColoringMode == cTEX_COORDS) c = color;
		else if (ColoringMode == cFACET  ) c = texelFetch(tex_colorsF, gl_PrimitiveID).rgb;
		else if (ColoringMode == cTEXTURE)
		{
			vec4 t = texture(tex_model, texCoord.xy);
			
			if	   (TexturingApp == tREPLACE)  final_color  = t;
			else if(TexturingApp == tMODULATE) final_color *= t;
			else if(TexturingApp == tDECAL)
			{
				c = mix(final_color.rgb, t.rgb, t.a);
				final_color = vec4(c, final_color.a);
			}
			else if(TexturingApp == tBLEND)
			{
				//c = mix(final_color.rgb, gl_TextureEnvColor[0].rgb, t.rgb); // ??
				final_color = vec4(c, final_color.a * t.a);
			}
			else if(TexturingApp == tADD)
			{
				final_color.rgb += t.rgb;
				final_color.a   *= t.a;
				final_color	     = clamp(final_color, 0.0f, 1.0f);
			}
		}
		if (ColoringMode < cTEXTURE || ColoringMode == cTEX_COORDS)
			final_color.rgb *= c;
	}
	
	if		(ColoringMode == cLUMIN) final_color	  = vec4((final_color.r+final_color.g+final_color.b)*0.33333f);
	else if (ColoringMode == cTOON ) 
	{
		float intensity = dot(normalize(light.position.xyz),n);
		if(intensity < 0.25f)
			final_color.xyz *= 0.1f;
		else if(intensity < 0.5f)
			final_color.xyz *= 0.5f;
		else if(intensity < 0.95f)
			final_color.xyz *= 0.8;
	}
	else if (ColoringMode == cNORMAL) final_color = vec4(n*0.5f+0.5f,1.0f);
	else if (ColoringMode == cDEPTH)  final_color = vec4(LinearizeDepth(gl_FragCoord.z));
	else if (ColoringMode == cXRAY)
	{
		float opac = dot(n, eye);
		opac = abs(opac);
		opac = 1.0f - pow(opac, edgeFalloff);

		final_color   = opac * vec4(1.0f);
		final_color.a = opac;
	}  
	else if (ColoringMode == cGOUCH)
	{
		const vec3  WarmColor	= vec3(0.6f, 0.6f, 0.0f);
		const vec3  CoolColor   = vec3(0.0f, 0.0f, 0.6f);

		vec3  lightVec   = normalize(light.position.xyz - pos3);
		vec3  ReflectVec = normalize(reflect(-lightVec, n));
		float NdotL      = (dot(lightVec, n) + 1.0f) * 0.5f;

	    vec3 kcool    = min(CoolColor + diffuseCool * final_color.rgb, 1.0f);
		vec3 kwarm    = min(WarmColor + diffuseWarm * final_color.rgb, 1.0f); 
		vec3 kfinal   = mix(kcool, kwarm, NdotL);

		vec3 nreflect = normalize(ReflectVec);

		float spec    = max(dot(nreflect, eye), 0.0f);
		spec          = pow(spec, 32.0f);

		final_color = vec4 (min(kfinal + spec, 1.0f), 1.0f);
	}

	return final_color;
}

#else 
vec4 computePixelColor()
{
	//vec3 eye = -normalize(pos3);
//	vec3 n   =  normalize(normal);
//	if(!gl_FrontFacing) n = -n;

	vec4  final_color;
//	Illumination(IlluminationMode, true, eye, pos3, n, final_color);
	
	if(ColoringMode == 1)
		final_color.rgb = (Instancing) ? instanceColor : vec3(1.0f);

	return final_color;
}
#endif