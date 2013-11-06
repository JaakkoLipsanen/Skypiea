using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Zombie.Components;
using Zombie.Messages;
using Zombie.Misc;

namespace Zombie.Systems
{
    public class PlayerManagerSystem : EntitySystem
    {
        private Entity _player;
        private PlayerInfoComponent _playerInfo;

        protected override void Initialize()
        {
            _player = this.EntityWorld.FindEntityByName(EntityTags.Player);
            _playerInfo = _player.Get<PlayerInfoComponent>();

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
