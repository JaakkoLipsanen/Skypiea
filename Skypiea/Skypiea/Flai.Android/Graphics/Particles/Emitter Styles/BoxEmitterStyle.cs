using Microsoft.Xna.Framework;

namespace Flai.Graphics.Particles.EmitterStyles
{
    public class BoxEmitterStyle : EmitterStyle
    {
        private SizeF _halfSize;
        private float _rotation;
        private Matrix _rotationMatrix;

        public SizeF Size
        {
            get { return _halfSize + _halfSize; }
            set { _halfSize = new SizeF(value.Width / 2f, value.Height / 2f); }
        }

        public float Rotation
        {
            get { return _rotation; }
            set
            {
                Ensure.IsValid(_rotation);
                _rotation = FlaiMath.RealModulus(_rotation, FlaiMath.TwoPi);
                if (_rotation != 0f)
                {
                    _rotationMatrix = Matrix.CreateRotationZ(_rotation);
                }
            }
        }

        public BoxEmitterStyle(SizeF size)
            : this(size, 0)      
        {    
        }

        public BoxEmitterStyle(SizeF size, float rotation)
        {
            this.Size = size;
            this.Rotation = rotation;
        }

        public override void GenerateOffsetAndForce(out Vector2 offset, out Vector2 initialForce)
        {
            initialForce = FlaiAlgorithms.GenerateRandomUnitVector2();
            offset = new Vector2 { X = Global.Random.NextFloat(-_halfSize.Width, _halfSize.Width), Y = Global.Random.NextFloat(-_halfSize.Height, _halfSize.Height) };

            if (_rotation != 0f)
            {
                Vector2.Transform(ref offset, ref _rotationMatrix, out offset);
            }
        }
    }
}
