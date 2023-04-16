#version 330 core

in vec3 aPosition;
in vec2 aTexCoord;

out vec2 texCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;

const float density = 0.02;
const float gradient = 5;
out float visibility;

void main()
{
    texCoord = aTexCoord;
    gl_Position =  vec4(aPosition, 1.0) * model * view * projection;

    float distance = length(vec4(aPosition, 1.0) * model * view);
    visibility = exp(-pow((distance * density), gradient));
    visibility = clamp(visibility, 0.0, 1.0);
}