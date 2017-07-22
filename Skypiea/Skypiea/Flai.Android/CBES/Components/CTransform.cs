
using System;
using Microsoft.Xna.Framework;

namespace Flai.CBES.Components
{
    #region 2D

    // todo: when entity detach from parent, the transform isn't currently updated correctly (the Position and Rotation values changes)
    public class CTransform2D : PoolableComponent, IModifiableTransform2D
    {
        public Vector2 LocalPosition;
        public float LocalRotation;

        public Vector2 Position
        {
            get
            {
                // dunno if works
                if (this.Entity.Parent == null)
                {
                    return this.LocalPosition;
                }
                else if (this.LocalPosition == Vector2.Zero)
                {
                    return this.Entity.Parent.Transform.Position;
                }

                return this.Entity.Parent.Transform.Position + this.LocalPosition.Rotate(this.Entity.Parent.Transform.Rotation);
            }
            set
            {
                if (this.Entity.Parent == null)
                {
                    this.LocalPosition = value;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public float Rotation
        {
            get
            {
                // dunno if works
                if (this.Entity.Parent == null)
                {
                    return this.LocalRotation;
                }

                return this.Entity.Parent.Transform.Rotation + this.LocalRotation;
            }
            set
            {
                if (this.Entity.Parent == null)
                {
                    this.LocalRotation = value;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public Vector2 LocalRotationVector
        {
            get { return FlaiMath.GetAngleVector(this.LocalRotation); }
            set
            {
                this.LocalRotation = FlaiMath.GetAngle(FlaiMath.NormalizeOrZero(value));
            }
        }

        public Vector2 RotationVector
        {
            get { return FlaiMath.GetAngleVector(this.Rotation); }
            set
            {
                this.Rotation = FlaiMath.GetAngle(FlaiMath.NormalizeOrZero(value));
            }
        }

        public CTransform2D()
        {
        }

        public void LookAt(Vector2 position)
        {
            this.RotationVector = position - this.Position;
        }

        protected internal override void Initialize()
        {
            this.LocalPosition = Vector2.Zero;
            this.LocalRotation = 0f;
        }

        protected internal override void Cleanup()
        {
            // fix for skypiea.. probably will break zombiegrid with this enabled. but maybe it's good idea to "keep" the transform data even after it's killed
            //this.LocalPosition = Vector2.Zero;
            //this.LocalRotation = 0f;
        }
    }

    #endregion

    #region 3D

    public class CTransform3D : PoolableComponent
    {
        public Vector3 Position { get; set; }
        // Vector GlobalPosition

        //     Quaternion Rotation ???
        // scale???? to 2d also?
    }

    #endregion
}
