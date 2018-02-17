﻿#version 330

out vec4 outputColor;


uniform sampler2D gPositionSampler;
uniform sampler2D gNormalSampler;
uniform sampler2D gAlbedoSpecSampler;

uniform vec3 uCameraPos;

in vec2 texCoords;

struct LightPoint{
	vec3 pos;
	vec3 color;
	float radius;
};

uniform LightPoint light;

void main()
{	
	vec3 posExtracted = texture(gPositionSampler, texCoords).rgb;
	vec3 normalExtracted = texture(gNormalSampler, texCoords).rgb;
	vec3 colorExtracted = texture(gAlbedoSpecSampler, texCoords).rgb;
	colorExtracted = colorExtracted * 0.35f;

	vec3 resultingColor = colorExtracted;
	vec3 viewDir = normalize(uCameraPos - posExtracted);

	float dist = length(light.pos - posExtracted);
	//if(dist < light.radius)
	//{
		vec3 lightDirection = normalize(light.pos - posExtracted);
		vec3 diffuse = max(dot(normalExtracted, lightDirection), 0.0) * colorExtracted * light.color;

		diffuse = 1 * diffuse * (1.0f / (1 +  0.25 * dist * dist));
		resultingColor += diffuse;
	//}

	outputColor = vec4(resultingColor, 1.0);
}
