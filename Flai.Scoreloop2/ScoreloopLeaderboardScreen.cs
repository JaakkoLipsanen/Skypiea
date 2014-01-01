using Flai.ScreenManagement;

namespace Flai.Scoreloop
{
    internal class ScoreloopLeaderboardScreen : GameScreen
    { 
        protected readonly IScoreloopManager _scoreloopManager;
        protected ScoreloopLeaderboardScreen(IScoreloopManager scoreloopManager)
        {
            _scoreloopManager = scoreloopManager;
        }

        protected ScoreloopLeaderboardScreen()
        {
            _scoreloopManager = FlaiGame.Current.Services.Get<IScoreloopManager>();
        }
    }
}
