using Flai.Misc;
using System.IO;
using Flai.Settings;
using Skypiea.View;

namespace Skypiea.Settings
{
    public class SkypieaSettings : SettingsBase
    {
        private GraphicalQuality _graphicalQuality = GraphicalQuality.High;
        private ThumbstickStyle _thumbstickStyle;
        private bool _isRateWindowShown;
        private bool _isFirstHelpShown;

        public ThumbstickStyle ThumbstickStyle
        {
            get { return _thumbstickStyle; }
            set { this.SetValue(ref _thumbstickStyle, value); }
        }

        public GraphicalQuality GraphicalQuality
        {
            get { return _graphicalQuality; }
            set { this.SetValue(ref _graphicalQuality, value); }
        }

        public bool IsRateWindowShown
        {
            get { return _isRateWindowShown; }
            set { this.SetValue(ref _isRateWindowShown, value); }
        }

        public bool IsFirstHelpShown
        {
            get { return _isFirstHelpShown; }
            set { this.SetValue(ref _isFirstHelpShown, value); }           
        }

        public int LaunchCounts { get; private set; }

        protected override void WriteInner(BinaryWriter writer)
        {
            writer.Write((int)this.ThumbstickStyle);
            writer.Write((int)this.GraphicalQuality);
            writer.Write(this.LaunchCounts);
            writer.Write(this.IsRateWindowShown);
            writer.Write(this.IsFirstHelpShown);
        }

        protected override void ReadInner(BinaryReader reader)
        {
            this.ThumbstickStyle = (ThumbstickStyle)reader.ReadInt32();
            this.GraphicalQuality = (GraphicalQuality) reader.ReadInt32();
            this.LaunchCounts = reader.ReadInt32() + 1;
            this.IsRateWindowShown = reader.ReadBoolean();
            this.IsFirstHelpShown = reader.ReadBoolean();

            // LaunchCounts has changed
            this.HasChanged = true;
        }
    }
}
