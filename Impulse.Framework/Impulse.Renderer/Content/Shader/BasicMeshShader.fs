#version 450

layout(set = 0, binding = 2) uniform LightInfoBuffer
{
    vec3 LightDirection;
    float padding;
};

layout(location = 0) in vec3 WorldPosition;
layout(location = 1) in vec3 WorldNormal;

layout(location = 0) out vec4 FragColor;

void main()
{
    vec4 meshColor = vec4(0.3, 0.3, 1, 1.0);

    // Ambient lighting component.
    vec4 ambientLight = 0.2 * vec4(1.0, 1.0, 1.0, 1.0);

    // Diffuse lighting component.
    float diffuseIntensity = max(dot(WorldNormal, -LightDirection), 0.0);
    vec4 diffuseLight = diffuseIntensity * vec4(1.0, 1.0, 1.0, 1.0);

    // Final fragment color.
    FragColor = (ambientLight + diffuseLight) * meshColor;
}