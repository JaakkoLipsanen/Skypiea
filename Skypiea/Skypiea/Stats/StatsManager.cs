using Flai;
using System.IO;
using System.IO.IsolatedStorage;

namespace Skypiea.Stats
{
    public class StatsManager
    {
        private const int VersionTag = 1;
        private readonly string _filePath;

        public int Kills { get; set; }
        public float TimePlayed { get; set; }
        public float TotalMovement { get; set; }

        public float TimePlayedInMinutes
        {
            get { return this.TimePlayed / 60f; }
        }

        public float TotalMovementInKilometers
        {
            get { return this.TotalMovement/1000f; }
        }

        public StatsManager(string filePath)
        {
            Ensure.NotNullOrWhitespace(filePath);
            Ensure.IsValidPath(filePath);

            _filePath = filePath;
            this.LoadFromFile();
        }

        public void SaveToFile()
        {
            using (IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (BinaryWriter writer = new BinaryWriter(isolatedStorage.OpenFile(_filePath, FileMode.Create)))
                {
                    writer.Write(StatsManager.VersionTag);

                    writer.Write(this.Kills);
                    writer.Write(this.TimePlayed);
                    writer.Write(this.TotalMovement);
                }
            }
        }

        private void LoadFromFile()
        {
            using (IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isolatedStorage.FileExists(_filePath))
                {
                    using (BinaryReader reader = new BinaryReader(isolatedStorage.OpenFile(_filePath, FileMode.Open)))
                    {
                        // can be used later
                        int versionTag = reader.ReadInt32();

                        this.Kills = reader.ReadInt32();
                        this.TimePlayed = reader.ReadSingle();
                        this.TotalMovement = reader.ReadSingle();
                    }
                }
            }
        }
    }
}
