using System.Numerics;

namespace Impulse.Renderer
{
    public class Transform
    {
        private Matrix4x4 world;
        private Vector3 position;
        private Quaternion rotation;
        private Vector3 scale;
        private bool dirty;

        public Transform()
        {
            world = Matrix4x4.Identity;
            position = Vector3.Zero;
            rotation = Quaternion.Identity;
            scale = Vector3.One;
            dirty = false;
        }

        public Matrix4x4 World
        {
            get
            {
                if (dirty)
                {
                    world = Matrix4x4.CreateTranslation(position)
                        * Matrix4x4.CreateFromQuaternion(rotation)
                        * Matrix4x4.CreateScale(scale);

                    dirty = false;
                }

                return world;
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

        public Quaternion Rotation
        {
            get => rotation;
            set
            {
                rotation = value;
                dirty = true;
            }
        }

        public Vector3 Scale
        {
            get => scale;
            set
            {
                scale = value;
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

        public void RotateX(float delta)
        {
            rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitX, delta);
            dirty = true;
        }

        public void RotateY(float delta)
        {
            rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitY, delta);
            dirty = true;
        }

        public void RotateZ(float delta)
        {
            rotation *= Quaternion.CreateFromAxisAngle(Vector3.UnitZ, delta);
            dirty = true;
        }

        public void ScaleX(float delta)
        {
            scale.X += delta;
            dirty = true;
        }

        public void ScaleY(float delta)
        {
            scale.Y += delta;
            dirty = true;
        }

        public void ScaleZ(float delta)
        {
            scale.Z += delta;
            dirty = true;
        }
    }
}