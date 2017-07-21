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
            _passiveStats = this.EntityWorld.Services.Get<IPlayerPassiveStats>();
            if (_passiveStats.SpawnWithThreeLives)
            {
                _playerInfo.AddLife();
            }
        }

        private void OnZombieKilled(ZombieKilledMessage message)
        {
            _playerInfo.Score += (int)(ZombieHelper.GetScore(message.Zombie.Get<CZombieInfo>().Type) * _passiveStats.ScoreMultiplier);
        }
    }
}
