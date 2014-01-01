using Scoreloop.CoreSocial.API;
using Scoreloop.CoreSocial.API.Model;
using System;
using ScoreloopRange = Scoreloop.CoreSocial.API.Model.Range;

namespace Flai.Scoreloop
{
    public delegate void ScoreloopCallback(ScoreloopResponse response);
    public delegate void ScoreloopCallback<T>(ScoreloopResponse<T> response);

    public interface IScoreloopManager
    {
        ScoreloopClient Client { get; }
        bool IsNetworkAvailable { get; }
        IRequestToken SubmitScore(double score, uint mode, ScoreloopCallback<Score> callback);
        IRequestToken GetRank(LeaderboardScope leaderboardScope, uint mode, ScoreloopCallback<RankResponse> callback);
        IRequestToken GetRank(LeaderboardScope leaderboardScope, Score score, ScoreloopCallback<RankResponse> callback);
        IRequestToken LoadScores(LeaderboardScope leaderboardScope, RangeInt scoreRange, uint mode, ScoreloopCallback<LeaderboardScoresResponse> callback);
        IRequestToken RenameUser(string newName, ScoreloopCallback callback);

        void Close();
    }

    // todo: move this to Flai and make it a "ICancellationToken"? okay not doable any more, but still could make interface without the RequestSource
    public interface IRequestToken
    {
        bool HasFinished { get; }
        IRequestController RequestSource { get; }
        void Cancel();
    }

    internal class RequestToken : IRequestToken
    {
        private readonly IRequestController _requestController;
        public bool HasFinished
        {
            get { return !_requestController.IsProcessing; }
        }

        public IRequestController RequestSource
        {
            get { return _requestController; }
        }

        public RequestToken(IRequestController requestController)
        {
            _requestController = requestController;
        }

        public void Cancel()
        {
            if (!this.HasFinished)
            {
                _requestController.Cancel();
            }
        }
    }

    // todo: ScoreloopMode? Or make this a super-low level Scoreloop manager and create higher level stuff also?
    // >> or maybe don't even use the word "Mode" in the API but instead use "leaderboard" (and do some kind of string -> uint conversion?)
    public class ScoreloopManager : IScoreloopManager
    {
        private readonly ScoreloopClient _client;
        public ScoreloopClient Client
        {
            get { return _client; }
        }

        public bool IsNetworkAvailable
        {
            get { return _client.Status.IsNetworkAvailable; }
        }

        public ScoreloopManager(string gameID, string gameSecret, string currency)
            : this(new ScoreloopClient(new Version(1, 0), gameID, gameSecret, currency))
        {     
        }

        public ScoreloopManager(ConfigurationBuilder configurationBuilder)
            : this(new ScoreloopClient(configurationBuilder))
        {
        }

        private ScoreloopManager(ScoreloopClient client)
        {
            _client = client;
        }

        // todo: return rank?
        public IRequestToken SubmitScore(double score, uint mode, ScoreloopCallback<Score> callback)
        {
            if (!this.IsNetworkAvailable)
            {
                callback.InvokeIfNotNull(new ScoreloopResponse<Score>(null, null, false));
                return null;
            }

            IScoreController scoreController = _client.CreateScoreController();
            scoreController.RequestFailed += (o, e) => callback.InvokeIfNotNull(new ScoreloopResponse<Score>(null, scoreController, false));
            scoreController.RequestCancelled += (o, e) => callback.InvokeIfNotNull(new ScoreloopResponse<Score>(null, scoreController, false));
            scoreController.ScoreSubmitted += (o, e) => callback.InvokeIfNotNull(new ScoreloopResponse<Score>(scoreController.Score, scoreController, true));

            scoreController.Submit(scoreController.CreateScore(score, mode));

            return new RequestToken(scoreController);
        }

        public IRequestToken GetRank(LeaderboardScope leaderboardScope, uint mode, ScoreloopCallback<RankResponse> callback)
        {
            if (!this.IsNetworkAvailable)
            {
                callback.InvokeIfNotNull(new ScoreloopResponse<RankResponse>(new RankResponse(-1, leaderboardScope), null, false));
                return null;
            }

            IRankingController rankingController = _client.CreateRankingController();
            rankingController.RequestFailed += (o, e) => callback.InvokeIfNotNull(new ScoreloopResponse<RankResponse>(new RankResponse(-1, leaderboardScope), rankingController, false));
            rankingController.RequestCancelled += (o, e) => callback.InvokeIfNotNull(new ScoreloopResponse<RankResponse>(new RankResponse(-1, leaderboardScope), rankingController, false));
            rankingController.RankingLoaded += (o, e) => callback.InvokeIfNotNull(new ScoreloopResponse<RankResponse>(new RankResponse((int)e.Controller.Rank, leaderboardScope), rankingController, true));

            rankingController.LoadRanking(ScoreloopHelper.GetSearchList(leaderboardScope, rankingController), mode);
            return new RequestToken(rankingController);
        }

        public IRequestToken GetRank(LeaderboardScope leaderboardScope, Score score, ScoreloopCallback<RankResponse> callback)
        {
            if (!this.IsNetworkAvailable)
            {
                callback.InvokeIfNotNull(new ScoreloopResponse<RankResponse>(new RankResponse(-1, leaderboardScope), null, false));
                return null;
            }

            IRankingController rankingController = _client.CreateRankingController();
            rankingController.RequestFailed += (o, e) => callback.InvokeIfNotNull(new ScoreloopResponse<RankResponse>(new RankResponse(-1, leaderboardScope), rankingController, false));
            rankingController.RequestCancelled += (o, e) => callback.InvokeIfNotNull(new ScoreloopResponse<RankResponse>(new RankResponse(-1, leaderboardScope), rankingController, false));
            rankingController.RankingLoaded += (o, e) => callback.InvokeIfNotNull(new ScoreloopResponse<RankResponse>(new RankResponse((int)e.Controller.Rank, leaderboardScope), rankingController, true));

            rankingController.LoadRanking(ScoreloopHelper.GetSearchList(leaderboardScope, rankingController), score);
            return new RequestToken(rankingController);
        }

        public IRequestToken LoadScores(LeaderboardScope leaderboardScope, RangeInt scoreRange, uint mode, ScoreloopCallback<LeaderboardScoresResponse> callback)
        {
            if (!this.IsNetworkAvailable)
            {
                callback.InvokeIfNotNull(new ScoreloopResponse<LeaderboardScoresResponse>(null, null, false));
                return null;
            }

            Ensure.True(scoreRange.Length > 0);

            IScoresController scoresController = _client.CreateScoresController();
            scoresController.RequestCancelled += (o, e) => callback.InvokeIfNotNull(new ScoreloopResponse<LeaderboardScoresResponse>(null, scoresController, false));
            scoresController.RequestFailed += (o, e) => callback.InvokeIfNotNull(new ScoreloopResponse<LeaderboardScoresResponse>(null, scoresController, false));
            scoresController.ScoresLoaded += (o, e) => callback.InvokeIfNotNull(new ScoreloopResponse<LeaderboardScoresResponse>(new LeaderboardScoresResponse(e.Controller.Scores, scoreRange.Min), scoresController, true));

            scoresController.LoadScores(ScoreloopHelper.GetSearchList(leaderboardScope, scoresController), new ScoreloopRange(scoreRange.Min, (uint)scoreRange.Length), mode);
            return new RequestToken(scoresController);
        }

        public IRequestToken RenameUser(string newName, ScoreloopCallback callback)
        {
            if (!this.IsNetworkAvailable)
            {
                callback.InvokeIfNotNull(new ScoreloopResponse(null, false));
                return null;
            }

            Ensure.NotNull(newName);

            IUserController userController = _client.CreateUserController();
            userController.RequestFailed += (o, e) => callback.InvokeIfNotNull(new ScoreloopResponse(userController, false));
            userController.RequestCancelled += (o, e) => callback.InvokeIfNotNull(new ScoreloopResponse(userController, false));
            userController.UserUpdated += (o, e) => callback.InvokeIfNotNull(new ScoreloopResponse(userController, true));

            userController.Update(newName, "");
            return new RequestToken(userController);
        }

        public void Close()
        {
            _client.Close();
        }
    }
}
