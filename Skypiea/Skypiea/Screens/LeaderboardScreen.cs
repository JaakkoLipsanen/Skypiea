using Flai;
using Flai.General;
using Flai.Graphics;
using Flai.Scoreloop;
using Flai.ScreenManagement;
using Flai.Ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Scoreloop.CoreSocial.API;
using Skypiea.Leaderboards;
using Skypiea.Ui;
using System;
using System.Collections.Generic;
using Range = Flai.Range;
using ScoreloopScore = Scoreloop.CoreSocial.API.Model.Score;

namespace Skypiea.Screens
{
    // todo: it'd be great if I'd refactor/rewrite the Flai.UI so that there can be hierarchy (and also skinning etc).
    // that way I could just create a "CameraRoot" or something like that which automatically handled the camera stuff in the GUI. now I
    // have to update the position manually...
    public class LeaderboardScreen : GameScreen
    {
        private const float ScoreSlotHeight = 32;
        private const float OffsetFromTop = 196;

        private readonly ScrollingLeaderboard _leaderboard;
        private readonly Scroller _scroller = new Scroller(Range.Zero, Alignment.Vertical);
        private readonly BasicUiContainer _uiContainer = new BasicUiContainer();
        private readonly Dictionary<LeaderboardScope, ScoreRank> _ranks = new Dictionary<LeaderboardScope, ScoreRank>();

        private ScrollerButton _dailyLeaderboardButton;
        private ScrollerButton _overallLeaderboardButton;
        private ScrollerButton _loadMoreScoresButton;

        private bool _hasFailed = false;
        private string _failMessage = "";

        public override bool IsPopup
        {
            get { return true; }
        }

        public LeaderboardScreen()
        {
            _leaderboard = new ScrollingLeaderboard(FlaiGame.Current.Services.Get<ScoreloopManager>(), 0, 25);
            if (_leaderboard.ScoreloopManager.IsNetworkAvailable)
            {
                //_failMessage = "Loading...";
                _leaderboard.GetRank(LeaderboardScope.Daily, this.OnRankLoaded);
                _leaderboard.GetRank(LeaderboardScope.AllTime, this.OnRankLoaded);
                _leaderboard.LoadMoreScores(this.OnScoresLoaded);
            }
            else
            {
                _hasFailed = true;
                _failMessage = "No network connection";
            }

            this.EnabledGestures = GestureType.Flick;
            this.TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            this.TransitionOffTime = TimeSpan.FromSeconds(0.5f);
            this.FadeType = FadeType.FadeAlpha;
            this.FadeIn = FadeDirection.Right;
            this.FadeOut = FadeDirection.Right;
            HighscoreHelper.ResubmitHighscoreIfNeeded();
        }

        protected override void LoadContent(bool instancePreserved)
        {
            if (instancePreserved)
            {
                return;
            }

            this.CreateUI();
        }

        protected override void Update(UpdateContext updateContext, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (this.IsExiting)
            {
                return;
            }

            _scroller.Update(updateContext);
            _uiContainer.Update(updateContext);
            if (updateContext.InputState.IsBackButtonPressed)
            {
                _leaderboard.CancelLastScoreRequest();
                this.ExitScreen();
                this.ScreenManager.AddScreen(new MainMenuScreen(FadeDirection.Left), new Delay(0.25f));
            }
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.SpriteBatch.Begin(_scroller.GetTransformMatrix(graphicsContext.ScreenSize));
            this.DrawScores(graphicsContext);
            graphicsContext.SpriteBatch.End();

            graphicsContext.SpriteBatch.Begin(SamplerState.LinearClamp);
            _uiContainer.Draw(graphicsContext, true);
            graphicsContext.SpriteBatch.End();
        }

        private void DrawScores(GraphicsContext graphicsContext)
        {
            SpriteFont scoreFont = graphicsContext.FontContainer["Minecraftia.20"];
            if (!string.IsNullOrWhiteSpace(_failMessage) || _leaderboard.Scores.Count == 0)
            {
                graphicsContext.SpriteBatch.DrawStringCentered(scoreFont, _failMessage ?? "Loading...", new Vector2(graphicsContext.ScreenSize.Width / 2f, 192), Color.White);
            }

            if (_ranks.ContainsKey(_leaderboard.CurrentScope))
            {
                ScoreRank rank = _ranks[_leaderboard.CurrentScope];
                if (rank.Rank == 0) // user doesn't have a rank on this leaderboard
                {
                    graphicsContext.SpriteBatch.DrawStringCentered(scoreFont, "You have no rank on this leaderboard", new Vector2(graphicsContext.ScreenSize.Width / 2f, 124), Color.White);
                }
                else // user has a rank on this leaderboard
                {
                    graphicsContext.SpriteBatch.DrawStringCentered(scoreFont, "You are " + rank.Rank, Common.GetOrdinalSuffix(rank.Rank), new Vector2(graphicsContext.ScreenSize.Width / 2f, 124), Color.White);
                    graphicsContext.SpriteBatch.DrawStringCentered(scoreFont, "with a score of ", rank.Score, new Vector2(graphicsContext.ScreenSize.Width / 2f, 156), Color.White);
                }
            }

            if (_leaderboard.Scores.Count == 0)
            {
                return;
            }

            const float HorizontalOffset = 32;
            const float ScoreHeight = 32;

            RectangleF scrollerArea = _scroller.GetArea(graphicsContext.ScreenSize);
            int topVisible = (int)FlaiMath.Clamp((scrollerArea.Top - OffsetFromTop) / ScoreHeight - 1, 0, _leaderboard.ScoresLoaded);
            int bottomVisible = (int)FlaiMath.Clamp(FlaiMath.Ceiling((scrollerArea.Bottom - OffsetFromTop) / ScoreHeight), 0, _leaderboard.ScoresLoaded);

            for (int i = topVisible; i < bottomVisible; i++)
            {
                float verticalPosition = OffsetFromTop + ScoreHeight * i;
                ScoreloopScore score = _leaderboard.Scores[i % _leaderboard.Scores.Count];
                ulong rank = (ulong)(i + 1); // score.Rank;
                int rankDigits = FlaiMath.DigitCount(rank);

                // rank
                graphicsContext.SpriteBatch.DrawString(scoreFont, rank, ".", new Vector2(HorizontalOffset, verticalPosition), Color.White);

                // user name
                float userNameHorizontalOffset = (rankDigits >= 3 ? 24 * (rankDigits - 2) : 0); // "if 3 or more digits, then add some offset to right, otherwise 0". this makes all usernames of scores 0-100 to start at same offset from left
                graphicsContext.SpriteBatch.DrawString(scoreFont, score.User.Login, new Vector2(HorizontalOffset + 72 + userNameHorizontalOffset, verticalPosition), Color.White);

                // TODO: dont call score.Result.ToString().....
                graphicsContext.SpriteBatch.DrawString(scoreFont, ((int)score.Result).ToString(), new Vector2(graphicsContext.ScreenSize.Width - HorizontalOffset, verticalPosition), Corner.TopRight, Color.White);
            }
        }

        private void CreateUI()
        {
            _uiContainer.Add(_dailyLeaderboardButton = new ScrollerButton("Daily", new Vector2(this.Game.ScreenSize.Width / 4f, 64), _scroller, this.OnDailyButtonClicked) { Font = "Minecraftia.24", Color = Color.White });
            _uiContainer.Add(_overallLeaderboardButton = new ScrollerButton("Overall", new Vector2(this.Game.ScreenSize.Width / 4f * 3f, 64), _scroller, this.OnOverallButtonClicked) { Font = "Minecraftia.24", Color = Color.DimGray });
            _uiContainer.Add(_loadMoreScoresButton = new ScrollerButton("Load more scores", new Vector2(this.Game.ScreenSize.Width / 2f, 100), _scroller, this.OnLoadMoreScoresClicked) { Font = "Minecraftia.24", Enabled = false, Visible = false });
        }

        private void OnLoadMoreScoresClicked()
        {
            _loadMoreScoresButton.Enabled = false;
            _loadMoreScoresButton.Visible = false;
            _leaderboard.LoadMoreScores(this.OnScoresLoaded);
        }

        private void OnOverallButtonClicked()
        {
            this.ChangeScope(LeaderboardScope.AllTime);
            _overallLeaderboardButton.Color = Color.White;
            _dailyLeaderboardButton.Color = Color.DimGray;
        }

        private void OnDailyButtonClicked()
        {
            this.ChangeScope(LeaderboardScope.Daily);
            _dailyLeaderboardButton.Color = Color.White;
            _overallLeaderboardButton.Color = Color.DimGray;
        }

        private void ChangeScope(LeaderboardScope scope)
        {
            if (!_hasFailed && _leaderboard.CurrentScope != scope)
            {
                _leaderboard.CurrentScope = scope;
                _leaderboard.LoadMoreScores(this.OnScoresLoaded);
            }
        }

        private void OnScoresLoaded(ScoreloopResponse<LeaderboardScoresResponse> response)
        {
            if (!response.Success)
            {
                if (response.Error == null || response.Error.Status != StatusCode.RequestCancelled)
                {
                    _hasFailed = true;
                    _failMessage = "Something went wrong. Please try again later";
                }

                return;
            }

            _scroller.ScrollingRange = new Range(0, FlaiMath.Max(0, _leaderboard.Scores.Count * ScoreSlotHeight - this.Game.ScreenSize.Height * 0.5f));
            if (_leaderboard.CanLoadMoreScores && response.Data != null && response.Data.Scores.Count != 0)
            {
                _loadMoreScoresButton.Enabled = true;
                _loadMoreScoresButton.Visible = true;
                _loadMoreScoresButton.SetVerticalPosition(OffsetFromTop + _leaderboard.Scores.Count * ScoreSlotHeight + 48); // _scoller.ScrollingRange.Max + 192 + this.Game.ScreenSize.Height*0.5f);
            }
        }

        private void OnRankLoaded(ScoreloopResponse<RankResponse> response)
        {
            if (!response.Success)
            {
                if (response.Error == null || response.Error.Status != StatusCode.RequestCancelled)
                {
                    _hasFailed = true;
                    _failMessage = "Something went wrong. Please try again later";
                }

                return;
            }

            int rank = response.Data.Rank;
            int score = (rank == 0) ? 0 : (int)response.Data.Score.Result;
            _ranks.Add(response.Data.Scope, new ScoreRank(rank, score));
        }

        private class ScoreRank
        {
            public int Rank { get; private set; }
            public int Score { get; private set; }

            public ScoreRank(int rank, int score)
            {
                this.Rank = rank;
                this.Score = score;
            }
        }
    }
}
