using System.Diagnostics;
using Flai.CBES.Systems;
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

            var leaderboardManager = LeaderboardHelper.CreateLeaderboardManager();
            leaderboardManager.SubmitScore(playerInfo.Score, 0, response =>
            {
                Debug.WriteLine("{0}: {1}", response.Success, response.Data.Result);
            });
        }
    }
}
