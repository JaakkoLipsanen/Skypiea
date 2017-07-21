using Flai.General;
using System.IO;

namespace Skypiea.Leaderboards
{
    public interface IHighscoreManager
    {
        int Highscore { get; }
    }

    public class HighscoreManager : PersistentManager, IHighscoreManager
    {
        private const int VersionTag = 1;
        
        private int _highscore;
        private bool _isHighscorePostedToScoreloop = true;

        public int Highscore
        {
            get { return _highscore; }
        }

        public bool IsHighscorePostedToLeaderboards
        {
            get { return _isHighscorePostedToScoreloop; }
            set { _isHighscorePostedToScoreloop = value; }
        }

        public HighscoreManager(string filePath)
            : base(filePath, LoadOption.Automatic)
        {
        }

        public virtual bool UpdateHighscore(int newScore)
        {
            if (newScore > _highscore)
            {
                _highscore = newScore;
                return true;
            }

            return false;
        }

        protected override void WriteInner(BinaryWriter writer)
        {
            writer.Write(HighscoreManager.VersionTag);
            writer.Write(_highscore);
            writer.Write(_isHighscorePostedToScoreloop);
        }

        protected override void ReadInner(BinaryReader reader)
        {
            int versionTag = reader.ReadInt32();

            _highscore = reader.ReadInt32();
            _isHighscorePostedToScoreloop = reader.ReadBoolean();
        }
    }
}
