using Flai;
using System.IO;
using System.IO.IsolatedStorage;

namespace Skypiea.Leaderboards
{
    public interface IHighscoreManager
    {
        int Highscore { get; }
    }

    public class HighscoreManager : IHighscoreManager
    {
        private readonly string _highscoreFilePath;
        private int _highscore;

        public int Highscore
        {
            get { return _highscore; }
        }

        public HighscoreManager(string highscoreFilePath)
        {
            Ensure.NotNullOrWhitespace(highscoreFilePath);
            Ensure.IsValidPath(highscoreFilePath);

            _highscoreFilePath = highscoreFilePath;
            this.LoadFromFile();
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

        public void SaveToFile()
        {
            using (IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (BinaryWriter writer = new BinaryWriter(isolatedStorage.OpenFile(_highscoreFilePath, FileMode.Create)))
                {
                    writer.Write(_highscore);
                    this.SaveInner(writer);
                }
            }
        }

        private void LoadFromFile()
        {
            using (IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isolatedStorage.FileExists(_highscoreFilePath))
                {
                    using (BinaryReader reader = new BinaryReader(isolatedStorage.OpenFile(_highscoreFilePath, FileMode.Open)))
                    {
                        _highscore = reader.ReadInt32();
                        this.LoadInner(reader);
                    }
                }
            }
        }

        protected virtual void SaveInner(BinaryWriter writer) { }
        protected virtual void LoadInner(BinaryReader reader) { }
    }
}
