using Flai;
using Flai.Graphics;
using Flai.Scoreloop;
using Flai.ScreenManagement;
using Microsoft.Xna.Framework;
using Scoreloop.CoreSocial.API.Model;
using Skypiea.Leaderboards;

namespace Skypiea.Screens
{
    public class LeaderboardScreen : GameScreen
    {
        private readonly ScrollingScoreloopLeaderboard _leaderboard;
        public LeaderboardScreen()
        {
            _leaderboard = new ScrollingScoreloopLeaderboard(LeaderboardHelper.CreateLeaderboardManager(), 0, 25);
            _leaderboard.LoadMoreScores();
        }

        protected override void Update(UpdateContext updateContext, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (updateContext.InputState.IsBackButtonPressed)
            {
                this.ScreenManager.LoadScreen(new MainMenuScreen());
            }
        }

        protected override void Draw(GraphicsContext graphicsContext)
        {
            graphicsContext.GraphicsDevice.Clear(Color.Black);

            graphicsContext.SpriteBatch.Begin();
            this.DrawScores(graphicsContext);           
            graphicsContext.SpriteBatch.End();
        }

        private void DrawScores(GraphicsContext graphicsContext)
        {
            for (int i = 0; i < _leaderboard.Scores.Count; i++)
            {
                const float HorizontalOffset = 8;
                const float OffsetFromTop = 32;
                const float ScoreHeight = 32;

                float verticalPosition = OffsetFromTop + ScoreHeight * i;
                Score score = _leaderboard.Scores[i];
                graphicsContext.SpriteBatch.DrawString(graphicsContext.FontContainer["Minecraftia.20"], score.User.Login, new Vector2(HorizontalOffset, verticalPosition), Color.White);

                // TODO: dont call score.Result.ToString().....
                graphicsContext.SpriteBatch.DrawString(graphicsContext.FontContainer["Minecraftia.20"], score.Result.ToString(), new Vector2(graphicsContext.ScreenSize.Width - HorizontalOffset, verticalPosition), Corner.TopRight, Color.White);
            }
        }
    }
}
