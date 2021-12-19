#version 450

layout(set = 0, binding = 0) uniform ProjectionBuffer
{
    mat4 Projection;
};

layout(set = 0, binding = 1) uniform ViewBuffer
{
    mat4 View;
};

layout(set = 1, binding = 0) uniform WorldBuffer
{
    mat4 World;
};

layout(location = 0) in vec3 Position;
layout(location = 1) in vec3 Normal;

layout(location = 0) out vec3 WorldPosition;
layout(location = 1) out vec3 WorldNormal;

void main()
{
    // Apply world-space transformation to vertex position.
    WorldPosition = (World * vec4(Position, 1.0)).xyz;

    // Apply world-space rotation to vertex normal.
    // Discard translation by zeroing w-component.
    WorldNormal = normalize(World * vec4(Normal, 0.0)).xyz;

    // Final vertex position.
    gl_Position = Projection * View * World * vec4(Position, 1.0);
}
