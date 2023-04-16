#version 330

out vec4 outputColor;

in vec2 texCoord;
in float visibility;

uniform sampler2D texture0;
uniform vec4 sky_color;

void main()
{
    outputColor = texture(texture0, texCoord);
    //outputColor = mix(sky_color, outputColor, visibility);
}