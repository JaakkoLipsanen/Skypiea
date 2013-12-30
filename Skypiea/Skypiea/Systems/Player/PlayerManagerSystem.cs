using Flai.CBES;
using Flai.CBES.Systems;
using Skypiea.Components;
using Skypiea.Messages;
using Skypiea.Misc;
using Skypiea.Model;

namespace Skypiea.Systems.Player
{
    public class PlayerManagerSystem : EntitySystem
    {
        private Entity _player;
        private CPlayerInfo _playerInfo;

        protected override void Initialize()
        {
            _player = this.EntityWorld.FindEntityByName(EntityNames.Player);
            _playerInfo = _player.Get<CPlayerInfo>();

            this.EntityWorld.SubscribeToMessage<ZombieKilledMessage>(this.OnZombieKilled);
            this.EntityWorld.SubscribeToMessage<PlayerKilledMessage>(this.OnPlayerKilled);

            if (this.EntityWorld.Services.Get<IPlayerPassiveStats>().SpawnWithThreeLives)
            {
                _playerInfo.AddLife();
            }
        }

        private void OnZombieKilled(ZombieKilledMessage message)
        {
            _playerInfo.Score += 100; // message.Score; or message.Zombie.Score/whatevr
        }

        private void OnPlayerKilled(PlayerKilledMessage message)
        {
            // blaah
        }
    }
}
