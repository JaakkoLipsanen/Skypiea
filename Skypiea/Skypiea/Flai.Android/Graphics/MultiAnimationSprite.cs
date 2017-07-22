using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Graphics
{
    public class MultiAnimationSprite : AnimatedSprite
    {
        private readonly Dictionary<string, AnimationInfo> _animations = new Dictionary<string, AnimationInfo>();

        private AnimationInfo _currentAnimationRange;
        private string _currentAnimationName = "";

        private bool _isAnimationLooping = true;
        private bool _hasAnimationEnded = false;

        public event EventHandler AnimationEnded;

        public string CurrentAnimation
        {
            get { return _currentAnimationName; }
        }

        public MultiAnimationSprite(Texture2D spriteSheet, int sheetWidth, int sheetHeight)
            : base(spriteSheet, sheetWidth, sheetHeight, 0d) // Temporary frame time is set to zero. Every animation in MultiAnimation can have it's own frame time
        {
        }

        protected override void UpdateFrame()
        {
            if (_hasAnimationEnded)
            {
                return;
            }

            if (_framePosition == _currentAnimationRange.End)
            {
                _framePosition = _currentAnimationRange.Start;
                if (!_isAnimationLooping)
                {
                    _hasAnimationEnded = true;
                    this.OnAnimationEnded();
                }
            }
            else
            {
                _framePosition.X++;
                if (_framePosition.X >= _sheetSize.Width)
                {
                    _framePosition.X = 0;

                    _framePosition.Y++;
                    if (_framePosition.Y >= _sheetSize.Height)
                    {
                        _framePosition.Y = 0;
                    }
                }
            }

            base.UpdateSourceRectangle();
        }

        public void AddAnimation(string name, Vector2i start, int lengthInFrames, double time)
        {
            Vector2i end = start;
            while (lengthInFrames-- != 0)
            {
                end.X++;
                if (end.X >= _sheetSize.Width)
                {
                    end.X = 0;
                    end.Y++;
                    if (end.Y >= _sheetSize.Height)
                    {
                        end.Y = 0;
                    }
                }
            }

            this.AddAnimation(name, start, end, time);
        }

        public void AddAnimation(string animationName, Vector2i start, Vector2i end, double time)
        {
            Ensure.False(start.X >= _sheetSize.Width || end.X >= _sheetSize.Width || start.Y >= _sheetSize.Height || end.Y >= _sheetSize.Height, "The start or end frame of animation is out of range");
            Ensure.True(time >= 0, "Time between frames must be positive!");

            _animations.Add(animationName, new AnimationInfo(start, end, time));
        }

        public void SetAnimation(string animationName, bool isLooping)
        {
            _isAnimationLooping = isLooping;
            _hasAnimationEnded = false;

            AnimationInfo animationInfo;
            if (!_animations.TryGetValue(animationName, out animationInfo))
            {
                throw new ArgumentException(string.Format("Animation named \"{0}\" can't be found", animationName));
            }
            _currentAnimationName = animationName;
            _currentAnimationRange = animationInfo;

            _framePosition = animationInfo.Start;

            _frameTime = animationInfo.Time;
            base.UpdateSourceRectangle();
        }

        private void OnAnimationEnded()
        {
            this.AnimationEnded.InvokeIfNotNull(this);
        }

        private struct AnimationInfo
        {
            public readonly Vector2i Start;
            public readonly Vector2i End;
            public readonly double Time;

            public AnimationInfo(Vector2i start, Vector2i end, double time)
                : this()
            {
                this.Start = start;
                this.End = end;
                this.Time = time;
            }
        }
    }
}
