using System;
using Flai;

namespace Skypiea
{
    public enum LeaderboardScope
    {
        Daily,
        AllTime
    }

    public class ScoreloopManager
    {
        private string gameID;
        private string secret;
        private string currency;

        public bool IsNetworkAvailable { get; internal set; }

        public ScoreloopManager(string gameID, string secret, string currency)
        {
            this.gameID = gameID;
            this.secret = secret;
            this.currency = currency;
        }

        internal void Close()
        {
          //  throw new NotImplementedException();
        }

        internal void RenameUser(string input, Action<object> p)
        {
         //   throw new NotImplementedException();
        }

        internal void GetUserScore(object allTime, int v, Action<object> p)
        {
        //    throw new NotImplementedException();
        }

        internal void SubmitScore(int score, int v, Action<object> p)
        {
         //   throw new NotImplementedException();
        }
    }
}