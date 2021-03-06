﻿#version 330

uniform sampler2D gPositionSampler;
uniform sampler2D gNormalSampler;
uniform sampler2D gAlbedoSpecSampler;

uniform vec3 lightPos;
uniform vec3 lightColor;

uniform mat4 uMV;

// write to the texture
layout (location = 0) out vec4 outputColor;

void main()
{
	vec2 texCoords = vec2(gl_FragCoord.x / 1920.0f, gl_FragCoord.y / 1061.0f);

	vec3 posExtracted = texture(gPositionSampler, texCoords).rgb;
	vec3 normalExtracted = normalize(texture(gNormalSampler, texCoords).rgb);
	vec3 colorExtracted = texture(gAlbedoSpecSampler, texCoords).rgb;

	vec3 resultingColor = vec3(0);

	vec3 lightPosTranslated = vec3(uMV * vec4(lightPos, 1.0));  

	float dist = length(lightPosTranslated - posExtracted);

	// incident
	vec3 lightDirection = normalize(lightPosTranslated - posExtracted);
	vec3 diffuse = clamp(dot(normalExtracted, lightDirection), 0.0, 1.0) * colorExtracted  * lightColor;

	diffuse = 0.02 * diffuse * (1.0f / (1 +  0.25 * dist * dist));
	resultingColor += diffuse;

	outputColor = vec4(resultingColor, 1f);
}