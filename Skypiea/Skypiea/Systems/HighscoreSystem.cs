using Flai.CBES;
using Flai.CBES.Systems;
using Skypiea.Components;
using Skypiea.Leaderboards;
using Skypiea.Messages;
using Skypiea.Misc;

namespace Skypiea.Systems
{
    public class HighscoreSystem : EntitySystem
    {
        protected override void Initialize()
        {
            this.EntityWorld.SubscribeToMessage<GameOverMessage>(this.OnGameOver);
        }

        private void OnGameOver(GameOverMessage message)
        {
            Entity player = this.EntityWorld.TryFindEntityByName(EntityNames.Player);
            if (player)
            {
                CPlayerInfo playerInfo = player.Get<CPlayerInfo>();
                HighscoreHelper.SubmitScore(playerInfo.Score);
            }
        }
    }
}
