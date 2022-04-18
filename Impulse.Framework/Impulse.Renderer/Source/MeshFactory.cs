using System.Collections.Generic;
using System.Numerics;
using Assimp;
using Veldrid;

namespace Impulse.Renderer;

public class MeshFactory
{
    public static List<Mesh> ImportModel(GraphicsDevice device, string filename)
    {
        var importer = new AssimpContext();

        var scene = importer.ImportFile(filename);

        var meshes = new List<Mesh>();

        foreach (var sceneMesh in scene.Meshes)
        {
            var vertices = new List<MeshVertex>();

            for (int i = 0; i < sceneMesh.VertexCount; i++)
            {
                var position = new Vector3(
                    sceneMesh.Vertices[i].X,
                    sceneMesh.Vertices[i].Y,
                    sceneMesh.Vertices[i].Z);

                var normal = new Vector3(
                    sceneMesh.Normals[i].X,
                    sceneMesh.Normals[i].Y,
                    sceneMesh.Normals[i].Z);

                vertices.Add(new MeshVertex(position, normal));
            }

            var indices = new List<uint>();

            foreach (var index in sceneMesh.GetIndices())
            {
                indices.Add((uint)index);
            }

            meshes.Add(new Mesh(device, vertices, indices));
        }

        return meshes;
    }

    public static Mesh CreateBox(GraphicsDevice device, float width = 1.0f, float height = 1.0f, float depth = 1.0f)
    {
        float w = width / 2;
        float h = height / 2;
        float d = depth / 2;

        var vertices = new MeshVertex[]
        {
            new MeshVertex(new Vector3(-w, -h,  d), new Vector3( 0,  0,  1)),
            new MeshVertex(new Vector3( w, -h,  d), new Vector3( 0,  0,  1)),
            new MeshVertex(new Vector3(-w,  h,  d), new Vector3( 0,  0,  1)),
            new MeshVertex(new Vector3( w,  h,  d), new Vector3( 0,  0,  1)),
            new MeshVertex(new Vector3(-w,  h,  d), new Vector3( 0,  1,  0)),
            new MeshVertex(new Vector3( w,  h,  d), new Vector3( 0,  1,  0)),
            new MeshVertex(new Vector3(-w,  h, -d), new Vector3( 0,  1,  0)),
            new MeshVertex(new Vector3( w,  h, -d), new Vector3( 0,  1,  0)),
            new MeshVertex(new Vector3(-w,  h, -d), new Vector3( 0,  0, -1)),
            new MeshVertex(new Vector3( w,  h, -d), new Vector3( 0,  0, -1)),
            new MeshVertex(new Vector3(-w, -h, -d), new Vector3( 0,  0, -1)),
            new MeshVertex(new Vector3( w, -h, -d), new Vector3( 0,  0, -1)),
            new MeshVertex(new Vector3(-w, -h, -d), new Vector3( 0, -1,  0)),
            new MeshVertex(new Vector3( w, -h, -d), new Vector3( 0, -1,  0)),
            new MeshVertex(new Vector3(-w, -h,  d), new Vector3( 0, -1,  0)),
            new MeshVertex(new Vector3( w, -h,  d), new Vector3( 0, -1,  0)),
            new MeshVertex(new Vector3( w, -h,  d), new Vector3( 1,  0,  0)),
            new MeshVertex(new Vector3( w, -h, -d), new Vector3( 1,  0,  0)),
            new MeshVertex(new Vector3( w,  h,  d), new Vector3( 1,  0,  0)),
            new MeshVertex(new Vector3( w,  h, -d), new Vector3( 1,  0,  0)),
            new MeshVertex(new Vector3(-w, -h, -d), new Vector3(-1,  0,  0)),
            new MeshVertex(new Vector3(-w, -h,  d), new Vector3(-1,  0,  0)),
            new MeshVertex(new Vector3(-w,  h, -d), new Vector3(-1,  0,  0)),
            new MeshVertex(new Vector3(-w,  h,  d), new Vector3(-1,  0,  0))
        };

        var indices = new uint[]
        {
             0,  1,  2,
             2,  1,  3,
             4,  5,  6,
             6,  5,  7,
             8,  9, 10,
            10,  9, 11,
            12, 13, 14,
            14, 13, 15,
            16, 17, 18,
            18, 17, 19,
            20, 21, 22,
            22, 21, 23
        };

        return new Mesh(device, vertices, indices);
    }
}
