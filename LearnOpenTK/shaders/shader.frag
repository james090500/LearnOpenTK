#version 330

out vec4 outputColor;

in float worldLight;
in vec2 texCoord;
in float lightValue;

uniform sampler2D texture0;

void main()
{
    vec3 ambient = lightValue * vec3(1.0,1.0,1.0) * worldLight;
    vec4 result = vec4(ambient, 1.0) * texture(texture0, texCoord);
    outputColor = result;
}