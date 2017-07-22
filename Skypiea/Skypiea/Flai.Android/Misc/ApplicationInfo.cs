using System.Xml.Linq;
using Android.Content;
#if WINDOWS_PHONE
using System;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework.Graphics;

namespace Flai.Misc
{
    public static class ApplicationInfo
    {
        private static string _applicationTitle;
        private static string _applicationDescription;
        private static string _applicationVersion;
        private static string _applicationPublisher;
        private static string _applicationAuthor;

        private static IsolatedStorageFile _isolatedStorage;

        /*
        public static string Title
        {
            get
            {
                return _applicationTitle ?? (_applicationTitle = ApplicationInfo.GetAppAttribute("Title"));
            }
        }

        /// <summary>
        /// Gets the application description
        /// </summary>
        public static string Description
        {
            get
            {
                return _applicationDescription ?? (_applicationDescription = ApplicationInfo.GetAppAttribute("Description"));
            }
        }

        /// <summary>
        /// Gets the application version
        /// </summary>
        public static string Version
        {
            get
            {
                return _applicationVersion ?? (_applicationVersion = ApplicationInfo.GetAppAttribute("Version"));
            }
        }

            */
        public static string ShortVersion
        {
            get
            {
                try
                {
                    return global::Android.App.Application.Context.PackageManager.GetPackageInfo(global::Android.App.Application.Context.PackageName, 0).VersionName.ToString();
                }
                catch
                {
                    return "";
                }
            }
        }
        

        /*
        /// <summary>
        /// Gets the application publisher
        /// </summary>
        public static string Publisher
        {
            get
            {
                return _applicationPublisher ?? (_applicationPublisher = ApplicationInfo.GetAppAttribute("Publisher"));
            }
        }

        /// <summary>
        /// Gets the application author
        /// </summary>
        public static string Author
        {
            get
            {
                return _applicationAuthor ?? (_applicationAuthor = ApplicationInfo.GetAppAttribute("Author"));
            }
        }
        */

        /*
        /// <summary>
        /// Returns the maximum memory usage that this application can allocate in bytes
        /// </summary>
        public static long MemoryUsageLimit
        {
            get
            {
                return DeviceStatus.ApplicationMemoryUsageLimit;
            }
        }

        /// <summary>
        /// Returns how much this application has currently allocated in bytes
        /// </summary>
        public static long CurrentMemoryUsage
        {
            get
            {
                return DeviceStatus.ApplicationCurrentMemoryUsage;
            }
        }
        */

        public static IsolatedStorageFile IsolatedStorage
        {
            get { return _isolatedStorage ?? (_isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication()); }
        }

        /*
        public static ShellTile ApplicationLiveTile
        {
            get { return ShellTile.ActiveTiles.FirstOrDefault(); }
        }

        public static void UpdateTile(Texture2D texture)
        {
            const int TextureSize = 173;
            const string FilePath = "/Shared/ShellContent/LiveTile.jpg";

            // Save texture to isolated storage
            IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication();
            using (IsolatedStorageFileStream stream = isolatedStorage.CreateFile(FilePath))
            {
                texture.SaveAsPng(stream, TextureSize, TextureSize);
            }

            // Update the tile
            ShellTile tile = ApplicationInfo.ApplicationLiveTile;
            if (tile != null)
            {
                tile.Update(new StandardTileData()
                {
                    BackgroundImage = new Uri("isostore:" + FilePath, UriKind.Absolute),
                });
            }
        }

        // Not working properly?? Didnt work on boxStrike
        public static void OpenApplicationReviewPage()
        {
            MarketplaceReviewTask marketplaceReviewTask = new MarketplaceReviewTask();
            marketplaceReviewTask.Show();
        }

        public static void OpenMarketplaceApplicationPage(string applicationId)
        {
            MarketplaceDetailTask marketplaceDetailTask = new MarketplaceDetailTask()
            {
                ContentType = MarketplaceContentType.Applications,
                ContentIdentifier = applicationId,
            };
            marketplaceDetailTask.Show();
        }

        public static void OpenDeveloperApplicationList()
        {
            ApplicationInfo.OpenDeveloperApplicationList(ApplicationInfo.Publisher); // ?
        }

        public static void OpenDeveloperApplicationList(string developer)
        {
            MarketplaceSearchTask marketplaceSearchTask = new MarketplaceSearchTask { ContentType = MarketplaceContentType.Applications, SearchTerms = developer };
            marketplaceSearchTask.Show();
        }

        /// <summary> 
        /// Gets an attribute from the Windows Phone App Manifest App element 
        /// </summary> 
        /// <param name="attributeName">the attribute name</param> 
        /// <returns>the attribute value</returns> 
        private static string GetAppAttribute(string attributeName)
        {
#if WP8 
            const string AppManifestName = "WMAppManifest.xml";
            const string AppNodeName = "App";

            return XDocument.Load(AppManifestName).Root.Element(AppNodeName).Attribute(attributeName).Value;           
#endif

            const string AppManifestName = "WMAppManifest.xml";
            const string AppNodeName = "App";

            XmlReaderSettings settings = new XmlReaderSettings { XmlResolver = new XmlXapResolver() };
            using (XmlReader reader = XmlReader.Create(AppManifestName, settings))
            {
                reader.ReadToDescendant(AppNodeName);

                // Return the value of the requested XML attribute if found or NULL if the XML element with the application information was not found in the application manifest
                return !reader.IsStartElement() ? null : reader.GetAttribute(attributeName);
            }
        }
        */
    }
}

#endif