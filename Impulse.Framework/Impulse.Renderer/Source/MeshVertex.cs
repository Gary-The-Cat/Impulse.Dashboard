using System.Numerics;

namespace Impulse.Renderer
{
    public struct MeshVertex
    {
        public const int SizeInBytes = 24;

        public Vector3 Position;

        public Vector3 Normal;

        public MeshVertex(Vector3 position, Vector3 normal)
        {
            Position = position;
            Normal = normal;
        }
    }
}