using System;
using Microsoft.Xna.Framework.Audio;

namespace Flai.Audio
{
    public enum FadeState
    {
        None = 0,
        FadingIn = 1,
        FadingOut = 2,
    }

    // Not completed yet
    public class FlaiSoundEffectInstance : IDisposable
    {
        private readonly SoundEffectInstance _soundEffectInstance;

        private FadeState _fadeState = FadeState.None;
        private TimeSpan _fadeTime;
        private TimeSpan _timeFaded;

        private float _originalVolume;

        public float Volume
        {
            get { return _soundEffectInstance.Volume; }
            set
            {
                if (_fadeState == FadeState.FadingOut)
                {
                    return;
                }

                if (_fadeState == FadeState.None)
                {
                    _soundEffectInstance.Volume = value;
                }

                _originalVolume = value;
            }
        }

        public float Pitch
        {
            get { return _soundEffectInstance.Pitch; }
            set { _soundEffectInstance.Pitch = value; }
        }

        public float Pan
        {
            get { return _soundEffectInstance.Pan; }
            set { _soundEffectInstance.Pan = value; }
        }

        public SoundState State
        {
            get { return _soundEffectInstance.State; }
        }

        public bool IsPlaying
        {
            get { return _soundEffectInstance.State == SoundState.Playing; }
        }

        public FadeState FadeState
        {
            get { return _fadeState; }
        }

        public bool IsDisposed
        {
            get { return _soundEffectInstance.IsDisposed; }
        }

        internal FlaiSoundEffectInstance(SoundEffectInstance soundEffect, float volume, float pitch, float pan, bool isLooping)
        {
            _soundEffectInstance = soundEffect;

            _soundEffectInstance.Volume = volume;
            _soundEffectInstance.Pitch = pitch;
            _soundEffectInstance.Pan = pan;
            _soundEffectInstance.IsLooped = isLooping;

            _originalVolume = volume;
        }

        #region IDisposable Members

        internal void Dispose()
        {
            ((IDisposable)this).Dispose();
        }

        void IDisposable.Dispose()
        {
            if (_soundEffectInstance != null)
            {
                _soundEffectInstance.Dispose();
            }
        }

        #endregion

        internal void Play()
        {
            this.Play(TimeSpan.Zero);
        }

        internal void Play(TimeSpan fadeInTime)
        {
            if (this.State != SoundState.Playing)
            {             
                if (fadeInTime != TimeSpan.Zero)
                {
                    _fadeTime = fadeInTime;
                    _fadeState = FadeState.FadingIn;
                    _originalVolume = this.Volume;
                    _soundEffectInstance.Volume = 0;

                    _timeFaded = TimeSpan.Zero;
                }

                _soundEffectInstance.Play();
            }
        }

        public void Stop()
        {
            this.Stop(TimeSpan.Zero);
        }

        public void Stop(TimeSpan fadeOutTime)
        {
            if (this.State == SoundState.Playing)
            {
                if (fadeOutTime == TimeSpan.Zero)
                {
                    _soundEffectInstance.Stop();
                }
                else
                {
                    _fadeTime = fadeOutTime;
                    _fadeState = FadeState.FadingOut;

                    _timeFaded = TimeSpan.Zero;
                }
            }
            else if (this.State == SoundState.Paused)
            {
                // _soundEffectInstance.Play() ??
                _soundEffectInstance.Stop();
            }
        }

        internal void Resume()
        {
            _soundEffectInstance.Resume();
        }

        internal void Pause()
        {
            _soundEffectInstance.Pause();
        }

        internal void Update(UpdateContext updateContext)
        {
            // If the sound effect is fading
            if (_fadeState != FadeState.None)
            {
                _timeFaded += updateContext.GameTime.XnaGameTime.ElapsedGameTime;
                float progress = (float)(_timeFaded.TotalSeconds / _fadeTime.TotalSeconds);
                bool completed = progress >= 1f;
                progress = Math.Min(1f, progress);

                // If the sound effect is fading out
                if (_fadeState == FadeState.FadingOut)
                {
                    _soundEffectInstance.Volume = _originalVolume * (1f - progress);
                    if (completed)
                    {
                        // If the fading out is completed, stop the sound effect and let SoundEffectManager to remove it
                        _soundEffectInstance.Stop();
                        _fadeState = FadeState.None;                  
                    }
                }
                // If the sound is fading in
                else if (_fadeState == FadeState.FadingIn)
                {
                    _soundEffectInstance.Volume = _originalVolume * progress;
                    if (completed)
                    {
                        _fadeState = FadeState.None;
                    }
                }
            }
        }

        private void ContinuePlaying()
        {
            if (this.State != SoundState.Playing)
            {
                if (this.State == SoundState.Paused)
                {
                    _soundEffectInstance.Resume();
                }
                else if (this.State == SoundState.Stopped)
                {
                    _soundEffectInstance.Play();
                }
            }
        }
    }
}
