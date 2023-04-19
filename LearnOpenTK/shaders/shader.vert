#version 330 core

uniform float aWorldLight;
in vec3 aPosition;
in vec2 aTexCoord;
in float aLightValue;

out float worldLight;
out vec2 texCoord;
out float lightValue;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

void main()
{
    worldLight = aWorldLight;
    gl_Position = vec4(aPosition, 1.0) * model * view * projection;    
    texCoord = aTexCoord;
    lightValue = aLightValue;
}