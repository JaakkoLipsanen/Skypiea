using Flai.CBES;
using Flai.CBES.Systems;
using Skypiea.Components;
using Skypiea.Messages;
using Skypiea.Misc;

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
