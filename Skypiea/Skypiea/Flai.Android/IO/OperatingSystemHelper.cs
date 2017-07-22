using System;

namespace Flai.IO
{
#if WINDOWS_PHONE
    public enum WindowsPhoneVersion
    {
        WP7,
        WP8, // future versions probably too
    }
#endif

    public static class OperatingSystemHelper
    {
        private static OperatingSystem _operatingSystem;
        public static OperatingSystem OperatingSystem
        {
            get { return _operatingSystem ?? (_operatingSystem = Environment.OSVersion); }
        }

        public static WindowsPhoneVersion Version
        {
            get { return OperatingSystemHelper.OperatingSystem.Version.Major == 7 ? WindowsPhoneVersion.WP7 : WindowsPhoneVersion.WP8; }
        }
    }
}
