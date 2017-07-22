using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics.Effects
{
    #region Source

//    // Okay, fuck! This doesn't currently work properly on ellipses. The stroke is messed not "scaled" properly.. not sure how to fix it

//// Color of the inner are in the circle
//float4 FillColor = float4(0, 0, 0.25, 0.75);

///* STORKE */
//float2 StrokeSize = 0.01; // Stroke size in TEXTURE COORDINATES! That means, if StrokeSize = 0.1, then the stroke will be 10% of the circle size
//float4 StrokeColor = float4(1, 1, 1, 1);
//bool SmoothStroke = false;

//float4 PS(float2 texCoord: TEXCOORD0) : COLOR
//{
//    // trigonometry: if the point is outside the circle, then return transparent color
//    float distanceFromCenter = sqrt(pow(abs(0.5 - texCoord.x), 2) + pow(abs(0.5 - texCoord.y), 2));
//    if(distanceFromCenter > 0.5)
//    {
//        return float4(0, 0, 0, 0);
//    }
//    else if(distanceFromCenter > 0.5 - StrokeSize.x || distanceFromCenter > 0.5 - StrokeSize.y)
//    {
//        if(SmoothStroke)
//        {
//            float centerStroke = 0.5 - StrokeSize / 2;
//            float alpha = pow(lerp(1, 0, abs(centerStroke - distanceFromCenter) / StrokeSize * 2), 2);

//            if(distanceFromCenter < centerStroke)
//            {
//                return float4(StrokeColor.rgb * alpha + FillColor * (1-alpha), 1);
//            }
			
//            return float4(StrokeColor.rgb * alpha, alpha); 
//        }
//        else
//        {
//            return StrokeColor;
//        }
//    }
		
//    return FillColor;
//}

//technique
//{
//    pass P0
//    {
//        PixelShader = compile ps_2_0 PS();
//    }
//}


    #endregion

    public class CircleEffect : Effect
    {
        private readonly EffectParameter _fillColorParameter;
        private readonly EffectParameter _strokeColorParameter;
        private readonly EffectParameter _strokeSizeParameter;
        private readonly EffectParameter _useSmoothStrokeParameter;

        private Color _fillColor = Color.Blue * 0.5f;
        private Color _strokeColor = Color.White;
        private Vector2 _strokeSize = new Vector2(4, 4);
        private bool _useSmoothStroke = false;

        public Color FillColor
        {
            get { return _fillColor; }
            set 
            {
                if(_fillColor != value)
                {
                    _fillColor = value;
                    _fillColorParameter.SetValue(_fillColor.ToVector4());
                }
            }
        }

        public Color StrokeColor
        {
            get { return _strokeColor; }
            set 
            {
                if(_strokeColor != value)
                {
                    _strokeColor = value;
                    _strokeColorParameter.SetValue(_strokeColor.ToVector4());
                }
            }
        }

        public Vector2 StrokeSize
        {
            get { return _strokeSize; }
            set 
            {
                if(_strokeSize != value)
                {
                    _strokeSize = value;
                    _strokeSizeParameter.SetValue(_strokeSize);
                }
            }
        }

          public bool UseStrokeSmoothing  
          {
              get { return _useSmoothStroke; }
              set 
              {
                  if(_useSmoothStroke != value)
                  {
                      _useSmoothStroke = value;
                      _useSmoothStrokeParameter.SetValue(_useSmoothStroke);
                  }
              }
          }

        public CircleEffect(GraphicsDevice graphicsDevice)
            : base(FlaiGame.Current.Content.LoadEffect("CircleEffect")) // //graphicsDevice, CompiledEffects.GetEffectBytes(CompiledEffect.CircleEffect))
        {
            _fillColorParameter = base.Parameters["FillColor"];
            _strokeColorParameter = base.Parameters["StrokeColor"];
            _strokeSizeParameter = base.Parameters["StrokeSize"];
            _useSmoothStrokeParameter = base.Parameters["SmoothStroke"];

            _fillColorParameter.SetValue(_fillColor.ToVector4());
            _strokeColorParameter.SetValue(_strokeColor.ToVector4());
            _strokeSizeParameter.SetValue(_strokeSize);
            _useSmoothStrokeParameter.SetValue(_useSmoothStroke);
        }
    }
}
