using Flai.General;
using System.IO;

namespace Flai.Settings
{
    public class SettingsManager<T> : PersistentManager
        where T : SettingsBase, new()
    {
        public T Settings { get; private set; }

        public SettingsManager(string filePath)
            : base(filePath)
        {
            if (!this.Load())
            {
                this.Settings = new T { HasChanged = true, IsFirstLaunch = true }; // first time it is saved to a file even if nothing is actually changed
            }
        }

        protected override void WriteInner(BinaryWriter writer)
        {
            this.Settings.HasChanged = false;
            writer.Write(this.Settings);
        }

        protected override void ReadInner(BinaryReader reader)
        {
            this.Settings = reader.ReadGeneric<T>();
            this.Settings.IsFirstLaunch = false;
        }

        protected override bool NeedsSaving()
        {
            return this.Settings.HasChanged;
        }
    }
}
