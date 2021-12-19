using System;
using System.Collections.Generic;
using System.Linq;
using Veldrid;

namespace Impulse.Renderer
{
    public class Mesh : IDisposable
    {
        public static readonly IndexFormat IndexFormat = IndexFormat.UInt32;

        private readonly Transform transform;

        private readonly DeviceBuffer vertexBuffer;
        private readonly DeviceBuffer indexBuffer;

        private readonly int vertexCount;
        private readonly int indexCount;

        public Mesh(GraphicsDevice device, IEnumerable<MeshVertex> vertices, IEnumerable<uint> indices)
        {
            ResourceFactory factory = device.ResourceFactory;

            transform = new Transform();

            vertexBuffer = factory.CreateBuffer(
                new BufferDescription(
                    (uint)vertices.Count() * MeshVertex.SizeInBytes,
                    BufferUsage.VertexBuffer));

            device.UpdateBuffer(VertexBuffer, 0, vertices.ToArray());

            indexBuffer = factory.CreateBuffer(
                new BufferDescription(
                    (uint)indices.Count() * sizeof(uint),
                    BufferUsage.IndexBuffer));

            device.UpdateBuffer(IndexBuffer, 0, indices.ToArray());

            vertexCount = vertices.Count();
            indexCount = indices.Count();
        }

        public Transform Transform
        {
            get => transform;
        }

        public DeviceBuffer VertexBuffer
        {
            get => vertexBuffer;
        }

        public int VertexCount
        {
            get => vertexCount;
        }

        public DeviceBuffer IndexBuffer
        {
            get => indexBuffer;
        }

        public int IndexCount
        {
            get => indexCount;
        }

        public void Dispose()
        {
            VertexBuffer.Dispose();
            IndexBuffer.Dispose();
        }
    }
}