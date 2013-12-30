using Flai.Misc;
using System.IO;
using Skypiea.Misc;

namespace Skypiea.Settings
{
    public class SkypieaSettings : SettingsBase
    {
        public ThumbstickStyle ThumbstickStyle { get; set; }
        protected override void WriteInner(BinaryWriter writer)
        {
            writer.Write((int)this.ThumbstickStyle);
        }

        protected override void ReadInner(BinaryReader reader)
        {
            this.ThumbstickStyle = (ThumbstickStyle)reader.ReadInt32();
        }
    }
}
