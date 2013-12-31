using System.Diagnostics;
using Flai;
using Flai.CBES.Systems;
using Flai.Scoreloop;
using Skypiea.Components;
using Skypiea.Leaderboards;
using Skypiea.Messages;
using Skypiea.Misc;

namespace Skypiea.Systems
{
    public class HighscoreSystem : EntitySystem
    {
        private readonly HighscoreManager _highscoreManager = HighscoreHelper.CreateHighscoreManager();
        protected override void Initialize()
        {
            this.EntityWorld.Services.Add<IHighscoreManager>(_highscoreManager);
            this.EntityWorld.SubscribeToMessage<GameOverMessage>(this.OnGameOver);
        }

        private void OnGameOver(GameOverMessage message)
        {
            CPlayerInfo playerInfo = this.EntityWorld.FindEntityByName(EntityNames.Player).Get<CPlayerInfo>();
            if (_highscoreManager.UpdateHighscore(playerInfo.Score))
            {
                _highscoreManager.SaveToFile();
            }

            var leaderboardManager = FlaiGame.Current.Services.Get<IScoreloopManager>(); // todo todo todo jos submission ei onnistu niin myöhemmin
            leaderboardManager.SubmitScore(playerInfo.Score, 0, response => Debug.WriteLine("HIGHSCORE: Success?: {0}. Result: {1}", response.Success, response.Data.Result));
        }
    }
}
