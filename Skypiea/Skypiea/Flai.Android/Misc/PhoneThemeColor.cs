#if WINDOWS_PHONE
using System;
using Microsoft.Xna.Framework;

namespace Flai.Misc
{
    public enum PhoneThemeColor
    {
        Dark = 1,
        Light = 2,
    }

    public static class PhoneThemeColorExtensions
    {
        public static Color ToColor(this PhoneThemeColor phoneThemeColor)
        {
            switch (phoneThemeColor)
            {
                case PhoneThemeColor.Dark:
                    return Color.Black;

                case PhoneThemeColor.Light:
                    return Color.White;

                default:
                    throw new ArgumentException("Value \"" + phoneThemeColor + "\" not recognized");
            }
        }
    }
}

#endif
