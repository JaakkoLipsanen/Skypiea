using Flai.DataStructures;
using Scoreloop.CoreSocial.API.Model;
using System.Collections.Generic;

namespace Flai.Scoreloop
{
    // awful name... possibility => make ScoreloopLeaderboard non-abstract and create enum "LeaderboardType" which can be Scrolling or Page.
    // then don't create two separate leaderboards, but instead just the one which implements both Scrolling and Page behaviors.
    public class ScrollingLeaderboard : ScoreloopLeaderboard
    {
        private readonly int _scoresToLoad;
        public ScrollingLeaderboard(IScoreloopManager scoreloopManager, uint mode, int scoresToLoad)
            : base(scoreloopManager, mode)
        {
            Ensure.True(scoresToLoad > 0);
            _scoresToLoad = scoresToLoad;
        }

        public void LoadMoreScores()
        {
            this.LoadMoreScores(null);
        }

        public void LoadMoreScores(ScoreloopCallback<LeaderboardScoresResponse> callback)
        {
            this.LoadScores(new RangeInt(this.ScoresLoaded, _scoresToLoad), callback);
        }

        new public void GetRank(ScoreloopCallback<RankResponse> callback)
        {
            _scoreloopManager.GetRank(this.CurrentScope, _mode, callback);
        }

        public void GetRank(LeaderboardScope scope, ScoreloopCallback<RankResponse> callback)
        {
            _scoreloopManager.GetRank(scope, _mode, callback);
        }

        public void Reset()
        {
            this.ClearScores();
        }

        public void CancelLastScoreRequest()
        {
            this.CancelLatestScoreRequest();
        }
    }

    public abstract class ScoreloopLeaderboard
    {
        private readonly List<Score> _scores;
        private readonly ReadOnlyList<Score> _readOnlyScores;
        private LeaderboardScope _leaderboardScope;
        private IRequestToken _latestScoreRequestToken;
        private bool _canLoadMoreScores = true;

        protected readonly IScoreloopManager _scoreloopManager;
        protected readonly uint _mode;

        public IScoreloopManager ScoreloopManager
        {
            get { return _scoreloopManager; }
        }

        public LeaderboardScope CurrentScope
        {
            get { return _leaderboardScope; }
            set
            {
                if (_leaderboardScope != value)
                {
                    _leaderboardScope = value;
                    _scores.Clear();

                    this.CancelLatestScoreRequest();
                }
            }
        }

        public ReadOnlyList<Score> Scores
        {
            get { return _readOnlyScores; }
        }

        public int ScoresLoaded
        {
            get { return this.Scores.Count; }
        }

        public bool CanLoadMoreScores
        {
            get { return _canLoadMoreScores; }
        }

        protected ScoreloopLeaderboard(IScoreloopManager scoreloopManager, uint mode)
        {
            Ensure.NotNull(scoreloopManager);

            _scoreloopManager = scoreloopManager;
            _mode = mode;

            _scores = new List<Score>();
            _readOnlyScores = new ReadOnlyList<Score>(_scores);
        }

        protected IRequestToken GetRank(ScoreloopCallback<RankResponse> callback)
        {
            return _scoreloopManager.GetRank(_leaderboardScope, _mode, callback);
        }

        protected IRequestToken GetRank(Score score, ScoreloopCallback<RankResponse> callback)
        {
            return _scoreloopManager.GetRank(_leaderboardScope, score, callback);
        }

        protected IRequestToken LoadScores(RangeInt scoreRange, ScoreloopCallback<LeaderboardScoresResponse> callback)
        {
            ScoreloopCallback<LeaderboardScoresResponse> innerCallback = response =>
            {
                if (_latestScoreRequestToken != null && response.RequestSource == _latestScoreRequestToken.RequestSource)
                {
                    _latestScoreRequestToken = null;
                }

                // if less scores than the requested amount was loaded, that *should* mean that there are no more scores to load
                if (response.Success && response.Data.Scores.Count != scoreRange.Length)
                {
                    _canLoadMoreScores = false;
                }

                this.OnScoresLoaded(response);
                if (callback != null)
                {
                    callback(response);
                }
            };

            this.CancelLatestScoreRequest();
            _latestScoreRequestToken = _scoreloopManager.LoadScores(_leaderboardScope, scoreRange, _mode, innerCallback);
            return _latestScoreRequestToken;
        }

        protected void ClearScores()
        {
            this.CancelLatestScoreRequest();
            _scores.Clear();
            _canLoadMoreScores = true;
        }

        private void OnScoresLoaded(ScoreloopResponse<LeaderboardScoresResponse> response)
        {
            if (!response.Success)
            {
                // todo: "error message" tms
                return;
            }

            foreach (Score score in response.Data.Scores)
            {
                _scores.Add(score);
            }
        }

        protected void CancelLatestScoreRequest()
        {
            if (_latestScoreRequestToken != null)
            {
                _latestScoreRequestToken.Cancel();
                _latestScoreRequestToken = null;
            }
        }
    }
}
