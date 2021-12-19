using System.Numerics;

namespace Impulse.Renderer
{
    public class Camera
    {
        private Matrix4x4 projection;
        private Matrix4x4 view;
        private Vector3 position;
        private Vector3 forward;
        private Vector3 up;
        private bool dirty;

        public Camera(float width, float height, float near = 1.0f, float far = 1000.0f)
        {
            projection = Matrix4x4.CreatePerspectiveFieldOfView(1.0f, width / height, near, far);
            view = Matrix4x4.Identity;
            position = Vector3.Zero;
            forward = Vector3.UnitZ;
            up = Vector3.UnitY;
            dirty = true;
        }

        public Matrix4x4 Projection
        {
            get => projection;
        }

        public Matrix4x4 View
        {
            get
            {
                if (dirty)
                {
                    view = Matrix4x4.CreateLookAt(position, position + forward, up);
                    dirty = false;
                }
                return view;
            }
        }

        public Vector3 Position
        {
            get => position;
            set
            {
                position = value;
                dirty = true;
            }
        }

        public Vector3 Forward
        {
            get => forward;
            set
            {
                forward = value;
                dirty = true;
            }
        }

        public Vector3 Up
        {
            get => up;
            set
            {
                up = value;
                dirty = true;
            }
        }

        public void TranslateX(float delta)
        {
            position.X += delta;
            dirty = true;
        }

        public void TranslateY(float delta)
        {
            position.Y += delta;
            dirty = true;
        }

        public void TranslateZ(float delta)
        {
            position.Z += delta;
            dirty = true;
        }

        public void LookAt(Vector3 target)
        {
            forward = Vector3.Normalize(target - position);
            dirty = true;
        }
    }
}