using System.IO;
using Flai.IO;

namespace Flai.Settings
{
    // don't use "Settings" since if you have a "Settings" namespace in the game,
    // it'd make this class and the namespace to be.. ambiguous? from each other
    public abstract class SettingsBase : IBinarySerializable
    {
        public bool IsFirstLaunch { get; internal set; }
        public bool HasChanged { get; protected internal set; }

        protected SettingsBase()
        {
            this.HasChanged = true;
        }

        protected void SetValue<T>(ref T variable, T newValue)
        {
            if (!variable.Equals(newValue))
            {
                variable = newValue;
                this.HasChanged = true;
            }
        }

        void IBinarySerializable.Write(BinaryWriter writer)
        {
            this.WriteInner(writer);
        }

        void IBinarySerializable.Read(BinaryReader reader)
        {
            this.ReadInner(reader);
        }

        protected abstract void WriteInner(BinaryWriter writer);
        protected abstract void ReadInner(BinaryReader reader);
    }
}
