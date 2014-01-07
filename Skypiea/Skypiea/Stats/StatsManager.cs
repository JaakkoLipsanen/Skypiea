using Flai.General;
using System;
using System.IO;

namespace Skypiea.Stats
{
    public class StatsManager : PersistentManager
    {
        private const int VersionTag = 1;

        private int _kills;
        private float _timePlayed;
        private float _totalMovement;
        private bool _hasChanged = false;

        public int Kills
        {
            get { return _kills; }
            set { this.SetValue(ref _kills, value); }
        }

        public float TimePlayed
        {
            get { return _timePlayed; }
            set { this.SetValue(ref _timePlayed, value); }
        }

        public float TotalMovement
        {
            get { return _totalMovement; }
            set { this.SetValue(ref _totalMovement, value); }
        }

        public float TimePlayedInMinutes
        {
            get { return this.TimePlayed / 60f; }
        }

        public float TotalMovementInKilometers
        {
            get { return this.TotalMovement/1000f; }
        }

        public StatsManager(string filePath)
             : base(filePath, LoadOption.Automatic)
        {
        }

        protected override void WriteInner(BinaryWriter writer)
        {
            writer.Write(StatsManager.VersionTag);

            writer.Write(this.Kills);
            writer.Write(this.TimePlayed);
            writer.Write(this.TotalMovement);

            _hasChanged = false;
        }

        protected override void ReadInner(BinaryReader reader)
        {
            // can be used later
            int versionTag = reader.ReadInt32();

            this.Kills = reader.ReadInt32();
            this.TimePlayed = reader.ReadSingle();
            this.TotalMovement = reader.ReadSingle();
        }

        protected override bool NeedsSaving()
        {
            return _hasChanged;
        }

        private void SetValue<T>(ref T variable, T value)
            where T : IEquatable<T>
        {
            if (!variable.Equals(value))
            {
                variable = value;
                _hasChanged = true;
            }
        }
    }
}
