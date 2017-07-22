using System;
using System.Collections.Generic;
using System.Linq;
using Flai.Content;
using Microsoft.Xna.Framework.Audio;

namespace Flai.Audio
{
    // When fading is completed, what should happen? Shitty name
    public enum FadeAction
    {
        Stop,
        Pause,
    }

    public class SoundEffectManager : FlaiGameComponent, ISoundEffectManager
    {
#if WINDOWS
        private const int MaximumSoundEffectCount = 1024;
#elif WINDOWS_PHONE
        private const int MaximumSoundEffectCount = 64;
#endif

        private readonly FlaiContentManager _contentManager;

        private readonly Dictionary<string, SoundEffect> _soundEffects = new Dictionary<string, SoundEffect>();
        private readonly FlaiSoundEffectInstance[] _playingSoundEffects = new FlaiSoundEffectInstance[MaximumSoundEffectCount];

        public float MasterVolume
        {
            get { return SoundEffect.MasterVolume; }
            set { SoundEffect.MasterVolume = value; }
        }

        /// <summary>
        /// WARNING! This is O(n) method
        /// </summary>
        public int PlayingSoundEffectCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < _playingSoundEffects.Length; i++)
                {
                    if (_playingSoundEffects[i] != null && _playingSoundEffects[i].IsPlaying)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        internal SoundEffectManager(FlaiServiceContainer services)
            : base(services)
        {
            _contentManager = _services.Get<IContentProvider>().GetManager("Audio");
            _services.Add<ISoundEffectManager>(this);
        }

        public void LoadSound(string name)
        {
            this.LoadSound(name, name);
        }

        public void LoadSound(string name, string path)
        {
            _soundEffects[name] = _contentManager.LoadSoundEffect(path);
        }

        public void Unload()
        {
            this.StopAllSounds();
            _contentManager.Unload();
        }

        public FlaiSoundEffectInstance CreateInstance(string name)
        {
            return this.CreateInstance(name, 1f, 0f, 0f);
        }

        public FlaiSoundEffectInstance CreateInstance(string name, float volume)
        {
            return this.CreateInstance(name, volume, 0f, 0f);
        }

        public FlaiSoundEffectInstance CreateInstance(string name, float volume, float pitch, float pan)
        {
            return new FlaiSoundEffectInstance(this.GetSoundEffect(name).CreateInstance(), volume, pitch, pan, false);
        }

        #region Play Sound

        public bool PlaySound(FlaiSoundEffectInstance soundEffect)
        {
            int index = this.GetAvailableSoundIndex();
            if (index != -1)
            {
                _playingSoundEffects[index] = soundEffect;

                if (_enabled)
                {
                    soundEffect.Play();
                }

                return true;
            }

            return false;
        }


        public FlaiSoundEffectInstance PlaySound(string name)
        {
            return this.InnerPlaySound(name, 1f, 0f, 0f, false, TimeSpan.Zero);
        }

        public FlaiSoundEffectInstance PlaySound(string name, TimeSpan fadeInTime)
        {
            return this.InnerPlaySound(name, 1f, 0f, 0f, false, fadeInTime);
        }

        public FlaiSoundEffectInstance PlaySound(string name, float volume)
        {
            return this.InnerPlaySound(name, volume, 0f, 0f, false, TimeSpan.Zero);
        }

        public FlaiSoundEffectInstance PlaySound(string name, float volume, TimeSpan fadeInTime)
        {
            return this.InnerPlaySound(name, volume, 0f, 0f, false, fadeInTime);
        }

        public FlaiSoundEffectInstance PlaySound(string name, float volume, float pitch, float pan)
        {
            return this.InnerPlaySound(name, volume, pitch, pan, false, TimeSpan.Zero);
        }

        public FlaiSoundEffectInstance PlaySound(string name, float volume, float pitch, float pan, TimeSpan fadeInTime)
        {
            return this.InnerPlaySound(name, volume, pitch, pan, false, fadeInTime);
        }

        public FlaiSoundEffectInstance PlaySound(string name, float volume, float pitch, float pan, bool isLooping)
        {
            return this.InnerPlaySound(name, volume, pitch, pan, isLooping, TimeSpan.Zero);
        }

        public FlaiSoundEffectInstance PlaySound(string name, float volume, float pitch, float pan, bool isLooping, TimeSpan fadeInTime)
        {
            return this.InnerPlaySound(name, volume, pitch, pan, isLooping, fadeInTime);
        }

        public bool PlaySound(string name, out FlaiSoundEffectInstance instance)
        {
            return (instance = this.InnerPlaySound(name, 1f, 0f, 0f, false, TimeSpan.Zero)) != null;
        }

        public bool PlaySound(string name, TimeSpan fadeInTime, out FlaiSoundEffectInstance instance)
        {
            return (instance = this.InnerPlaySound(name, 1f, 0f, 0f, false, fadeInTime)) != null;
        }

        public bool PlaySound(string name, float volume, out FlaiSoundEffectInstance instance)
        {
            return (instance = this.InnerPlaySound(name, volume, 0f, 0f, false, TimeSpan.Zero)) != null;
        }

        public bool PlaySound(string name, float volume, TimeSpan fadeInTime, out FlaiSoundEffectInstance instance)
        {
            return (instance = this.InnerPlaySound(name, volume, 0f, 0f, false, fadeInTime)) != null;
        }

        public bool PlaySound(string name, float volume, float pitch, float pan, out FlaiSoundEffectInstance instance)
        {
            return (instance = this.InnerPlaySound(name, volume, pitch, pan, false, TimeSpan.Zero)) != null;
        }

        public bool PlaySound(string name, float volume, float pitch, float pan, TimeSpan fadeInTime, out FlaiSoundEffectInstance instance)
        {
            return (instance = this.InnerPlaySound(name, volume, pitch, pan, false, fadeInTime)) != null;
        }

        public bool PlaySound(string name, float volume, float pitch, float pan, bool isLooping, out FlaiSoundEffectInstance instance)
        {
            return (instance = this.InnerPlaySound(name, volume, pitch, pan, isLooping, TimeSpan.Zero)) != null;
        }

        public bool PlaySound(string name, float volume, float pitch, float pan, bool isLooping, TimeSpan fadeInTime, out FlaiSoundEffectInstance instance)
        {
            return (instance = this.InnerPlaySound(name, volume, pitch, pan, isLooping, fadeInTime)) != null;
        }

        private FlaiSoundEffectInstance InnerPlaySound(string name, float volume, float pitch, float pan, bool isLooping, TimeSpan fadeInTime)
        {
            int index = this.GetAvailableSoundIndex();
            if (index != -1)
            {
                SoundEffect soundEffect = this.GetSoundEffect(name);
                FlaiSoundEffectInstance instance = new FlaiSoundEffectInstance(soundEffect.CreateInstance(), volume, pitch, pan, isLooping);
                _playingSoundEffects[index] = instance;

                if (_enabled)
                {
                    instance.Play(fadeInTime);
                }
                // If manager is not enabled, pause the sound effect. By default sound effect is stopped and to get it paused, it has to be played first. Thus, always start playing the instance and pause if manager is not enabled
                else
                {
                    instance.Volume = 0;
                    instance.Play();
                    instance.Pause();
                    instance.Volume = volume;
                }

                return instance;
            }

            return null;
        }

        #endregion

        public void StopAllSounds()
        {
            this.StopAllSounds(TimeSpan.Zero);
        }

        public void StopAllSounds(TimeSpan fadeOutTime)
        {
            foreach (FlaiSoundEffectInstance soundEffect in _playingSoundEffects.Where(soundEffect => soundEffect != null))
            {
                soundEffect.Stop(fadeOutTime);
            }
        }

        public override void Update(UpdateContext updateContext)
        {
            for (int i = 0; i < _playingSoundEffects.Length; i++)
            {
                if (_playingSoundEffects[i] != null)
                {
                    _playingSoundEffects[i].Update(updateContext);

                    // If the sound effect is stopped, remove it
                    if (_playingSoundEffects[i].State == SoundState.Stopped)
                    {
                        _playingSoundEffects[i].Dispose();
                        _playingSoundEffects[i] = null;
                    }
                }
            }
        }

        private int GetAvailableSoundIndex()
        {
            for (int i = 0; i < _playingSoundEffects.Length; i++)
            {
                if (_playingSoundEffects[i] == null)
                {
                    return i;
                }
            }

            return -1;
        }

        protected override void OnEnabledChanged()
        {
            if (_enabled)
            {
                foreach (FlaiSoundEffectInstance soundEffect in _playingSoundEffects)
                {
                    if (soundEffect != null && soundEffect.State == SoundState.Paused)
                    {
                        soundEffect.Play();
                    }
                }
            }
            else
            {
                foreach (FlaiSoundEffectInstance soundEffect in _playingSoundEffects)
                {
                    if (soundEffect != null && soundEffect.State == SoundState.Playing)
                    {
                        soundEffect.Pause();
                    }
                }
            }
        }

        private SoundEffect GetSoundEffect(string name)
        {
            SoundEffect soundEffect;
            if (!_soundEffects.TryGetValue(name, out soundEffect))
            {
                soundEffect = _contentManager.LoadSoundEffect(name);
            }

            return soundEffect;
        }
    }
}
