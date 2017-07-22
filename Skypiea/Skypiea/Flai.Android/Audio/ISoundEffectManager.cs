using System;
namespace Flai.Audio
{
    public interface ISoundEffectManager
    {
        bool Enabled { get; set; }

        int PlayingSoundEffectCount { get; }
        float MasterVolume { get; set; }

        void LoadSound(string name);
        void LoadSound(string name, string path);

        FlaiSoundEffectInstance CreateInstance(string name);
        FlaiSoundEffectInstance CreateInstance(string name, float volume);
        FlaiSoundEffectInstance CreateInstance(string name, float volume, float pitch, float pan);
      
        bool PlaySound(FlaiSoundEffectInstance soundEffect);

        FlaiSoundEffectInstance PlaySound(string name);
        FlaiSoundEffectInstance PlaySound(string name, float volume);
        FlaiSoundEffectInstance PlaySound(string name, float volume, float pitch, float pan);
        FlaiSoundEffectInstance PlaySound(string name, float volume, float pitch, float pan, bool isLooping);
        
        FlaiSoundEffectInstance PlaySound(string name, TimeSpan fadeInTime);
        FlaiSoundEffectInstance PlaySound(string name, float volume, TimeSpan fadeInTime);
        FlaiSoundEffectInstance PlaySound(string name, float volume, float pitch, float pan, TimeSpan fadeInTime);
        FlaiSoundEffectInstance PlaySound(string name, float volume, float pitch, float pan, bool isLooping, TimeSpan fadeInTime);

        bool PlaySound(string name, out FlaiSoundEffectInstance instance); 
        bool PlaySound(string name, float volume, out FlaiSoundEffectInstance instance);       
        bool PlaySound(string name, float volume, float pitch, float pan, out FlaiSoundEffectInstance instance);     
        bool PlaySound(string name, float volume, float pitch, float pan, bool isLooping, out FlaiSoundEffectInstance instance);

        bool PlaySound(string name, TimeSpan fadeInTime, out FlaiSoundEffectInstance instance);
        bool PlaySound(string name, float volume, TimeSpan fadeInTime, out FlaiSoundEffectInstance instance);
        bool PlaySound(string name, float volume, float pitch, float pan, TimeSpan fadeInTime, out FlaiSoundEffectInstance instance); 
        bool PlaySound(string name, float volume, float pitch, float pan, bool isLooping, TimeSpan fadeInTime, out FlaiSoundEffectInstance instance);       
             
        void StopAllSounds();
        void StopAllSounds(TimeSpan fadeOutTime);     
    }
}
