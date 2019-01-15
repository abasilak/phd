#include "version.h"

#if slow_rendering

uniform bool  useTransparency;
uniform bool  useTranslucency;
uniform float transparency;
uniform float gamma;

//layout(binding = 3)
uniform sampler2DRect tex_thickness;

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

struct Material_Par
{
    vec4  ambient;
    vec4  diffuse;
    vec4  specular;
    float shininess;
    float ni;
    float absorption;
    float gaussian_m;
    float gaussian_c;
};

layout(std140, binding = 1) uniform Light
{
    Light_Par light;
};

layout(std140, binding = 2) uniform Material
{
    Material_Par material;
};

void use_material( inout vec4 ambient, inout vec4 diffuse, inout vec4 specular)
{
	ambient  *= material.ambient;
	diffuse  *= material.diffuse;
	specular *= material.specular;
}

float LightThickness(in bool mat_used)
{
	float thickness;
	float absorption = (mat_used) ? material.absorption : 20.0f;
	
	thickness = abs(texture(tex_thickness, gl_FragCoord.xy).r);

	return exp(-absorption*thickness); // Beer-Lambert's Law
}

float FresnelRefraction(in bool mat_used, in float NdotV)
{
	const float fresnelPow = 5.0f;

	float ni = (mat_used) ? material.ni : 0.2f;
	float Ft = 1.0f - pow(1.0f - NdotV, fresnelPow);
	Ft		*= (1.0f-ni);
	Ft		+= ni;
	return (1.0f-Ft);
}

void Cook(in bool mat_used, in float NdotL, in float NdotH, in float NdotV, in float VdotH, out vec4 ambient, out vec4 diffuse, out vec4 specular)
{
	// D term (gaussian)
	float m = (mat_used)? material.gaussian_m : 0.5f;
	float M = m*m;
	float C = (mat_used) ? material.gaussian_c : 1.0f;
	float alpha = acos(NdotH);	alpha *= alpha;
	float Dt = C * exp(- alpha / M );
	
	// Geometric factor (G)
	float g  = 2.0 * NdotH / VdotH;
	float G1 = NdotV * g;
	float G2 = NdotL * g;
	float Gt = min(1.0, min(G1, G2));

	// Fresnel Refraction (F)
	float Ft = FresnelRefraction(mat_used, NdotV);
		
	float spec = ((Ft * Dt * Gt) / NdotV);
	ambient = light.ambient;
	diffuse = light.diffuse  * NdotL;
	specular= light.specular * spec;
}

void Phong(in bool mat_used, in float NdotL, in float NdotH, out vec4 ambient, out vec4 diffuse, out vec4 specular)
{
	ambient = light.ambient;
	diffuse = light.diffuse*NdotL;
	
	float shininess = (mat_used) ? material.shininess : 35.0f;
	//float spec = pow(RdotV, shininess);	// Phong
	float spec = pow(NdotH, shininess);		// Blinn-Phong
	//float spec = exp(- alpha /m));		// Gaussian
	specular = light.specular * spec;
}

void Illumination(in int model, in bool mat_used, in vec3 E, in vec3 P, in vec3 N, out vec4 final_color)
{
	vec3  L, H;
	float NdotL,NdotV,NdotH,VdotH;
	float attenuation,d;
	vec4  ambient, diffuse, specular;
	
	ambient = vec4(0.0f);
	diffuse = vec4(0.0f);
	specular= vec4(0.0f);
	
	if(light.position.w == 0.0f){
		L = vec3(light.position);
		attenuation = 1.0f;
	}
	else{
		L = vec3(light.position) - P;
		d = length(L);
		attenuation = 1.0f/(light.constantAttenuation	   +
							light.linearAttenuation    * d +
							light.quadraticAttenuation * d * d);
	}
	L = normalize(L);
	H = normalize(L + E);
		
	NdotL = max(0.0f, dot(N, L));
	NdotH = max(0.0f, dot(N, H));
	NdotV = dot(N, E);
	VdotH = dot(E, H);
	
	if	   (model == 0) Phong(mat_used, NdotL, NdotH,				ambient, diffuse, specular);
	else if(model == 1) Cook (mat_used, NdotL, NdotH, NdotV, VdotH, ambient, diffuse, specular);
	
	ambient  *= light.Ka * attenuation;
	diffuse  *= light.Kd * attenuation; 
	specular *= light.Ks * attenuation;
	
	if(mat_used) use_material(ambient, diffuse, specular);
	
	const float Ka    = 0.8f;
	const vec4  jade  = vec4(0.4, 0.14, 0.11, 1.0)*8.0f;
	const vec4  green = vec4(0.3, 0.7 , 0.1 , 1.0)*4.0f;

	final_color = (useTranslucency) ? (
									  LightThickness(mat_used)*Ka +
									  (1.0f-Ka)*
									  FresnelRefraction(mat_used, NdotV))
									  //*vec4(1.0f)
									  *
									  jade
									  //green
									: ambient + diffuse + specular;
	if(useTransparency)
	{
		final_color.a	 = transparency;
		final_color.rgb *= final_color.a;
	}
	else
		final_color.a = 1.0f;
	final_color.rgb = pow(final_color.rgb, vec3(1.0f));

	//final_color.rgb = pow(final_color.rgb, vec3(1.0f / gamma));
}
#endif