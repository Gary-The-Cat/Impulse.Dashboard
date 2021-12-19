// <copyright file="Camera.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using Microsoft.Xna.Framework;

namespace Impulse.Mono.Viewer
{
    public class Camera
    {
        private readonly float cameraMovementSpeedPerSecond = 10;

        private Vector3 cameraPosition;
        private Vector3 cameraTarget;
        private bool orbit;

        public Camera(Vector2 screenSize)
        {
            WorldMatrix = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up);

            OnResize(screenSize.X / screenSize.Y);
        }

        public Camera(Matrix projectionMatrix)
        {
            this.ProjectionMatrix = projectionMatrix;
        }

        public Matrix ProjectionMatrix { get; private set; }

        public Matrix ViewMatrix { get; private set; }

        public Matrix WorldMatrix { get; private set; }

        public Vector3 CameraPosition
        {
            get => cameraPosition;
            set
            {
                cameraPosition = value;
                ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
            }
        }

        public Vector3 CameraTarget
        {
            get => cameraTarget;
            set
            {
                cameraTarget = value;
                ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
            }
        }

        public void Update(float deltaT)
        {
            var step = cameraMovementSpeedPerSecond * deltaT;

            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.A))
            {
                cameraPosition.X -= step;
                cameraTarget.X -= step;
            }

            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.D))
            {
                cameraPosition.X += step;
                cameraTarget.X += step;
            }

            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.Up))
            {
                cameraPosition.Y -= step;
                cameraTarget.Y -= step;
            }

            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.W))
            {
                cameraPosition.Z += step;
            }

            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.S))
            {
                cameraPosition.Z -= step;
            }

            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.Down))
            {
                cameraPosition.Y += step;
                cameraTarget.Y += step;
            }

            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.Space))
            {
                orbit = !orbit;
            }

            if (orbit)
            {
                Matrix rotationMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(step));
                cameraPosition = Vector3.Transform(cameraPosition, rotationMatrix);
            }

            ViewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
        }

        public void OnResize(float aspectRatio)
        {
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                               MathHelper.ToRadians(50f),
                               aspectRatio,
                               1f,
                               1000f);
        }
    }
}
