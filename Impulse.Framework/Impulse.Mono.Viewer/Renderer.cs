// <copyright file="Renderer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Impulse.Mono.Viewer
{
    public class Renderer
    {
        // Camera
        private readonly Camera camera;
        private readonly GraphicsDevice graphicsDevice;

        // List of Models
        private readonly Scene scene;

        // Constructor
        public Renderer(GraphicsDevice device)
        {
            graphicsDevice = device;
            camera = new Camera(new Vector2(device.Viewport.X, device.Viewport.Y));
            scene = new Scene();

            camera.CameraPosition = new Vector3(0, 0, -2f);
            camera.CameraTarget = Vector3.Zero;
        }

        public void AddItem(Model model)
        {
            scene.AddModel(model);
        }

        // Update Call
        public void Update(float deltaT)
        {
            camera.Update(deltaT);
        }

        // Draw Call
        public void Draw()
        {
            graphicsDevice.Clear(new Color(0x1e, 0x1e, 0x1e));
            foreach (var model in scene.Models)
            {
                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.AmbientLightColor = new Vector3(1f, 0, 0);
                        effect.View = camera.ViewMatrix;
                        effect.World = camera.WorldMatrix;
                        effect.Projection = camera.ProjectionMatrix;
                    }

                    mesh.Draw();
                }
            }
        }

        public void OnResize(float aspectRatio)
        {
            camera.OnResize(aspectRatio);
        }
    }
}
