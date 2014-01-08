using Flai.CBES;
using Flai.CBES.Systems;
using Skypiea.Components;
using Skypiea.Messages;
using Skypiea.Misc;
using Skypiea.Model;
using Skypiea.Prefabs;

namespace Skypiea.Systems.Player
{
    public class PlayerManagerSystem : EntitySystem
    {
        private Entity _player;
        private CPlayerInfo _playerInfo;
        private IPlayerPassiveStats _passiveStats;

        protected override void Initialize()
        {
            _player = this.EntityWorld.FindEntityByName(EntityNames.Player);
            _playerInfo = _player.Get<CPlayerInfo>();

            this.EntityWorld.SubscribeToMessage<ZombieKilledMessage>(this.OnZombieKilled);
            this.EntityWorld.SubscribeToMessage<PlayerKilledMessage>(this.OnPlayerKilled);

            _passiveStats = this.EntityWorld.Services.Get<IPlayerPassiveStats>();
            if (_passiveStats.SpawnWithThreeLives)
            {
                _playerInfo.AddLife();
            }
        }

        private void OnZombieKilled(ZombieKilledMessage message)
        {
            _playerInfo.Score += (int)(this.GetScore(message.Zombie.Get<CZombieInfo>().Type) * _passiveStats.ScoreMultiplier); // message.Score; or message.Zombie.Score/whatevr
        }

        private void OnPlayerKilled(PlayerKilledMessage message)
        {
            // blaah
        }

        private int GetScore(ZombieType type)
        {
            switch (type)
            {
                case ZombieType.Normal:
                    return 100;
                case ZombieType.Fat:
                    return 200;

                case ZombieType.Rusher:
                    return 150;

                default:
                    return 100;
            }
        }
    }
}
