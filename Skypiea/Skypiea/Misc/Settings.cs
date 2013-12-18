using Flai;
using Skypiea.View;

namespace Skypiea.Misc
{
    public class Settings
    {
        public GraphicalQuality GraphicalQuality { get; set; }

        public Settings()
        {
            this.GraphicalQuality = GraphicalQuality.High;
        }

        #region Singleton Stuff

        private static Settings _instance = new Settings();
        public static Settings Current
        {
            get { return _instance; }
            set
            {
                Ensure.NotNull(value);
                _instance = value;
            }
        }

        #endregion
    }
}
