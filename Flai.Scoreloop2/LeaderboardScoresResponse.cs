using Flai.DataStructures;
using Scoreloop.CoreSocial.API.Model;

namespace Flai.Scoreloop
{
    // formatter from IScoreController.Game.ScoreFormatter?
    public class LeaderboardScoresResponse
    {
        public ReadOnlyArray<Score> Scores { get; private set; }
        public int FirstScoreRank { get; private set; }

        internal LeaderboardScoresResponse(Score[] scores, int firstScoreRank)
        {
            this.Scores = new ReadOnlyArray<Score>(scores);
            this.FirstScoreRank = firstScoreRank;
        }
    }

    public class RankResponse
    {
        public int Rank { get; private set; }
        public LeaderboardScope Scope { get; private set; }

        internal RankResponse(int rank, LeaderboardScope scope)
        {
            this.Rank = rank;
            this.Scope = scope;
        }
    }
}
