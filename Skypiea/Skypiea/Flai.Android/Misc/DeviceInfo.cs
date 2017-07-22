#if WINDOWS_PHONE

using System;
using System.Net.NetworkInformation;
using System.Windows;
using XnaColor = Microsoft.Xna.Framework.Color;

namespace Flai.Misc
{
    public static class DeviceInfo
    {
        /*
        /// <summary>
        /// Returns the phone's theme color ( black or white )
        /// </summary>
        public static XnaColor PhoneThemeColor
        {
            get
            {
                SolidColorBrush themeBrush = Application.Current.Resources["PhoneBackgroundBrush"] as SolidColorBrush;
                if (themeBrush == null)
                {
                    return XnaColor.Black;
                }
              
                return new XnaColor(themeBrush.Color.R, themeBrush.Color.G, themeBrush.Color.B);
            }
        }

        /// <summary>
        /// Returns the phone's accent color
        /// </summary>
        public static XnaColor PhoneAccentColor
        {
            get
            {
                WindowsColor accentBrush = (WindowsColor)Application.Current.Resources["PhoneAccentColor"];
                if (accentBrush == null)
                {
                    // Blue is default color
                    return new XnaColor(27, 161, 226);
                }

                return new XnaColor(accentBrush.R, accentBrush.G, accentBrush.B);
            }
        }

        /// <summary>
        /// Returns true if the device has 256MB of RAM
        /// </summary>
        public static bool IsLowMemoryDevice
        {
            get
            {
                const long MagicNumber = 94371840L;
              
                object result;
                if (DeviceExtendedProperties.TryGetValue("ApplicationWorkingSetLimit", out result))
                {
                    long memory = (long)result;
                    if (memory < MagicNumber)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

    */
        /// <summary>
        /// Note: This does not actually tell if the device has access to internet or if the connection is actually working.
        /// </summary>
        public static bool IsNetworkAvailable
        {
            get { return NetworkInterface.GetIsNetworkAvailable(); }
          //  NetworkInterface.NetworkInterfaceType != NetworkInterfaceType.None; } // this is slow, blocking call, but a more "correct" one
        }

        /*
        public static event EventHandler<NetworkNotificationEventArgs> NetworkAvailabilityChanged
        {
            add { DeviceNetworkInformation.NetworkAvailabilityChanged += value; }
            remove { DeviceNetworkInformation.NetworkAvailabilityChanged -= value; }
        }
        */
    }
}

#endif