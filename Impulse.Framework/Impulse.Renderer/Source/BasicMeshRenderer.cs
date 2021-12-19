using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using Veldrid;
using Veldrid.SPIRV;

namespace Impulse.Renderer
{
    public class BasicMeshRenderer : IDisposable
    {
        private readonly DeviceBuffer projectionBuffer;
        private readonly DeviceBuffer viewBuffer;
        private readonly DeviceBuffer lightInfoBuffer;
        private readonly DeviceBuffer worldBuffer;

        private readonly ResourceSet sceneResourceSet;
        private readonly ResourceSet meshResourceSet;

        private readonly Pipeline pipeline;
        private readonly Camera camera;

        public BasicMeshRenderer(GraphicsDevice device)
        {
            var factory = device.ResourceFactory;

            projectionBuffer = factory.CreateBuffer(
                new BufferDescription(64, BufferUsage.UniformBuffer | BufferUsage.Dynamic));

            device.UpdateBuffer(projectionBuffer, 0, Matrix4x4.Identity);

            viewBuffer = factory.CreateBuffer(
                new BufferDescription(64, BufferUsage.UniformBuffer | BufferUsage.Dynamic));

            device.UpdateBuffer(viewBuffer, 0, Matrix4x4.Identity);

            lightInfoBuffer = factory.CreateBuffer(
                new BufferDescription(LightInfo.SizeInBytes, BufferUsage.UniformBuffer | BufferUsage.Dynamic));

            device.UpdateBuffer(lightInfoBuffer, 0, new LightInfo(Vector3.Zero));

            worldBuffer = factory.CreateBuffer(
                new BufferDescription(64, BufferUsage.UniformBuffer | BufferUsage.Dynamic));

            device.UpdateBuffer(worldBuffer, 0, Matrix4x4.Identity);

            var sceneResourceLayout = factory.CreateResourceLayout(
                new ResourceLayoutDescription(
                    new ResourceLayoutElementDescription(
                        "ProjectionBuffer",
                        ResourceKind.UniformBuffer,
                        ShaderStages.Vertex),
                    new ResourceLayoutElementDescription(
                        "ViewBuffer",
                        ResourceKind.UniformBuffer,
                        ShaderStages.Vertex),
                    new ResourceLayoutElementDescription(
                        "LightInfoBuffer",
                        ResourceKind.UniformBuffer,
                        ShaderStages.Fragment)));

            var meshResourceLayout = factory.CreateResourceLayout(
                new ResourceLayoutDescription(
                    new ResourceLayoutElementDescription(
                        "WorldBuffer",
                        ResourceKind.UniformBuffer,
                        ShaderStages.Vertex)));

            sceneResourceSet = factory.CreateResourceSet(
                new ResourceSetDescription(
                    sceneResourceLayout,
                    projectionBuffer,
                    viewBuffer,
                    lightInfoBuffer));

            meshResourceSet = factory.CreateResourceSet(
                new ResourceSetDescription(
                    meshResourceLayout,
                    worldBuffer));

            // All VertexElementSemantics must be TextureCoordinate for D3D11.
            var vertexLayoutDescription = new VertexLayoutDescription(
                new VertexElementDescription(
                    "Position",
                    VertexElementSemantic.TextureCoordinate,
                    VertexElementFormat.Float3),
                new VertexElementDescription(
                    "Normal",
                    VertexElementSemantic.TextureCoordinate,
                    VertexElementFormat.Float3));

            var vertexShaderDescription = new ShaderDescription(
                stage: ShaderStages.Vertex,
                shaderBytes: File.ReadAllBytes(Directory.GetCurrentDirectory() + "/Content/Shader/BasicMeshShader.vs"),
                entryPoint: "main");

            var fragmentShaderDescription = new ShaderDescription(
                stage: ShaderStages.Fragment,
                shaderBytes: File.ReadAllBytes(Directory.GetCurrentDirectory() + "/Content/Shader/BasicMeshShader.fs"),
                entryPoint: "main");

            var shaderSetDescription = new ShaderSetDescription(
                vertexLayouts: new[] { vertexLayoutDescription },
                shaders: factory.CreateFromSpirv(vertexShaderDescription, fragmentShaderDescription));

            var rasterizerStateDescription = new RasterizerStateDescription(
                cullMode: FaceCullMode.Back,
                fillMode: PolygonFillMode.Solid,
                frontFace: FrontFace.CounterClockwise,
                depthClipEnabled: true,
                scissorTestEnabled: false);

            pipeline = factory.CreateGraphicsPipeline(
                new GraphicsPipelineDescription(
                    blendState: BlendStateDescription.SingleOverrideBlend,
                    depthStencilStateDescription: DepthStencilStateDescription.DepthOnlyLessEqual,
                    rasterizerState: rasterizerStateDescription,
                    primitiveTopology: PrimitiveTopology.TriangleList,
                    shaderSet: shaderSetDescription,
                    resourceLayouts: new[] { sceneResourceLayout, meshResourceLayout },
                    outputs: device.SwapchainFramebuffer.OutputDescription));

            camera = new Camera(device.SwapchainFramebuffer.Width, device.SwapchainFramebuffer.Height);
        }

        public Camera Camera
        {
            get => camera;
        }

        public void Enable(CommandList list)
        {
            list.SetPipeline(pipeline);

            list.UpdateBuffer(projectionBuffer, 0, Camera.Projection);
            list.UpdateBuffer(viewBuffer, 0, camera.View);
            list.UpdateBuffer(lightInfoBuffer, 0, new LightInfo(Camera.Forward));

            list.SetGraphicsResourceSet(0, sceneResourceSet);
            list.SetGraphicsResourceSet(1, meshResourceSet);
        }

        public void Draw(CommandList list, Mesh mesh)
        {
            list.UpdateBuffer(worldBuffer, 0, mesh.Transform.World);
            list.SetVertexBuffer(0, mesh.VertexBuffer);
            list.SetIndexBuffer(mesh.IndexBuffer, Mesh.IndexFormat);
            list.DrawIndexed((uint)mesh.IndexCount);
        }

        public void Dispose()
        {
            projectionBuffer.Dispose();
            viewBuffer.Dispose();
            lightInfoBuffer.Dispose();
            worldBuffer.Dispose();
            sceneResourceSet.Dispose();
            meshResourceSet.Dispose();
            pipeline.Dispose();
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct LightInfo
        {
            public const int SizeInBytes = 16;

            public Vector3 LightDirection;
            private float padding;

            public LightInfo(Vector3 lightDirection)
            {
                LightDirection = lightDirection;
                padding = 0;
            }
        }
    }
}