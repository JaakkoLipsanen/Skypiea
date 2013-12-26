using System;
using System.Collections.Generic;
using Flai;
using Flai.General;
using Flai.Graphics;
using Flai.Scoreloop;
using Flai.ScreenManagement;
using Flai.Ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Skypiea.Leaderboards;
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

        private readonly ScrollingLeaderboard _leaderboard;
        private readonly Scroller _scoller = new Scroller(Range.Zero, Alignment.Vertical);
        private readonly BasicUiContainer _uiContainer = new BasicUiContainer();

        private LeaderboardButton _dailyLeaderboardButton;
        private LeaderboardButton _overallLeaderboardButton;

        private readonly Dictionary<LeaderboardScope, int> _ranks = new Dictionary<LeaderboardScope, int>();
        private bool _hasFailed = false;

        public override bool IsPopup
        {
            get { return true; }
        }

        public LeaderboardScreen()
        {
            _leaderboard = new ScrollingLeaderboard(LeaderboardHelper.CreateLeaderboardManager(), 0, 25);
            _leaderboard.LoadMoreScores(this.OnScoresLoaded);
            _leaderboard.GetRank(LeaderboardScope.Daily, this.OnRankLoaded);
            _leaderboard.GetRank(LeaderboardScope.AllTime, this.OnRankLoaded);

            this.EnabledGestures = GestureType.Flick;
            this.TransitionOnTime = TimeSpan.FromSeconds(0.5f);
            this.TransitionOffTime = TimeSpan.FromSeconds(0.5f);
            this.FadeBackBufferToBlack = false;
        }

        protected override void LoadContent(bool instancePreserved)
        {
            if (instancePreserved)
            {
                return;
            }

            this.CreateUI();
        }

        protected override void UnloadContent()
        {
            _leaderboard.ScoreloopManager.Close();
        }

        protected override void Update(UpdateContext updateContext, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (this.IsExiting)
            {
                return;
            }

            _scoller.Update(updateContext);
            _uiContainer.Update(updateContext);
            if (updateContext.InputState.IsBackButtonPressed)
            {
                this.Exited += () => this.ScreenManager.AddScreen(new MainMenuScreen());
                this.ExitScreen();
            }
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.SpriteBatch.GlobalAlpha.Push(this.TransitionAlpha);
            graphicsContext.SpriteBatch.Begin(_scoller.GetTransformMatrix(graphicsContext.ScreenSize));
            this.DrawScores(graphicsContext);
            graphicsContext.SpriteBatch.End();

            _uiContainer.Draw(graphicsContext, false);
            graphicsContext.SpriteBatch.GlobalAlpha.Pop();
        }

        private void DrawScores(GraphicsContext graphicsContext)
        {
            SpriteFont scoreFont = graphicsContext.FontContainer["Minecraftia.20"];
            if (_hasFailed)
            {
                graphicsContext.SpriteBatch.DrawStringCentered(scoreFont, "Something went wrong. Please try again later", new Vector2(graphicsContext.ScreenSize.Width / 2f, 192), Color.White);
            }

            if (_ranks.ContainsKey(_leaderboard.CurrentScope))
            {
                int rank = _ranks[_leaderboard.CurrentScope];
                if (rank == 0) // user doesn't have a rank on this leaderboard
                {
                    graphicsContext.SpriteBatch.DrawStringCentered(scoreFont, "You have no rank on this leaderboard", new Vector2(graphicsContext.ScreenSize.Width / 2f, 124), Color.White);
                }
                else // user has a rank on this leaderboard
                {
                    graphicsContext.SpriteBatch.DrawStringCentered(scoreFont, "You are " + rank, Common.GetOrdinalSuffix(rank), new Vector2(graphicsContext.ScreenSize.Width / 2f, 124), Color.White);
                }
            }

            if (_leaderboard.Scores.Count == 0)
            {
                return;
            }

            const float HorizontalOffset = 8;
            const float OffsetFromTop = 164;
            const float ScoreHeight = 32;

            RectangleF scrollerArea = _scoller.GetArea(graphicsContext.ScreenSize);
            int topVisible = (int)FlaiMath.Clamp((scrollerArea.Top - OffsetFromTop) / ScoreHeight, 0, _leaderboard.Scores.Count);
            int bottomVisible = (int)FlaiMath.Clamp(FlaiMath.Ceiling((scrollerArea.Bottom - OffsetFromTop) / ScoreHeight), 0, _leaderboard.Scores.Count);

            for (int i = 0; i < 105; i++)
            {
                float verticalPosition = OffsetFromTop + ScoreHeight * i;
                ScoreloopScore score = _leaderboard.Scores[i % _leaderboard.Scores.Count];
                ulong rank = (ulong)i; // score.Rank;
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
            _uiContainer.Add(_dailyLeaderboardButton = new LeaderboardButton("Daily", new Vector2(this.Game.ScreenSize.Width / 4f, 64), _scoller, this.OnDailyButtonClicked) { Font = "Minecraftia.24", Color = Color.White });
            _uiContainer.Add(_overallLeaderboardButton = new LeaderboardButton("Overall", new Vector2(this.Game.ScreenSize.Width / 4f * 3f, 64), _scoller, this.OnOverallButtonClicked) { Font = "Minecraftia.24", Color = Color.DimGray });
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
                _hasFailed = true;
                return;
            }

            _scoller.ScrollingRange = new Range(0, _leaderboard.Scores.Count * ScoreSlotHeight + ScoreSlotHeight * 100);
        }

        private void OnRankLoaded(ScoreloopResponse<RankResponse> response)
        {
            if (!response.Success)
            {
                _hasFailed = true;
                return;
            }

            _ranks.Add(response.Data.Scope, response.Data.Rank);
        }

        #region Leaderboard Button

        private class LeaderboardButton : TextButton
        {
            private readonly float _initialVerticalPosition;
            private readonly Scroller _scroller;
            public LeaderboardButton(string text, Vector2 position, Scroller scroller, GenericEvent clickFunction)
                : base(text, position, clickFunction)
            {
                _initialVerticalPosition = position.Y;
                _scroller = scroller;
            }

            public override void Update(UpdateContext updateContext)
            {
                base.Update(updateContext);
                this.SetVerticalOffset(_scroller.ScrollValue);
            }

            private void SetVerticalOffset(float verticalOffset)
            {
                _centerPosition.Y = _initialVerticalPosition - verticalOffset;
                this.UpdateArea();
            }
        }

        #endregion
    }
}
