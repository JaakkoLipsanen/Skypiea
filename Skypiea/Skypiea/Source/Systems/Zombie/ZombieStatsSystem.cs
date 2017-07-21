using Flai;
using Flai.CBES.Systems;
using Skypiea.Components;
using Skypiea.Misc;

namespace Skypiea.Systems.Zombie
{
    public interface IZombieStatsProvider
    {
        float SpawnRate { get; }
        float SpeedMultiplier { get; }

        // todo: temporary!!
        float TotalTime { get; }
    }

    public class ZombieStatsSystem : EntitySystem, IZombieStatsProvider
    {
        private const float ZombieStartSpawnTime = 0.85f;
        private const float UpdateOnlySpawnRateEnd = 120;
        private const float UpdateOnlySpeedEnd = ZombieStatsSystem.UpdateOnlySpawnRateEnd + 100;

        private CPlayerInfo _playerInfo;
        private float _spawnRate = 1;
        private float _speedMultiplier = 1;

        private float _totalPlayTime = 0f;
        private float TotalSpawnRateUpdateTime
        {
            get
            {
                if (_totalPlayTime < ZombieStatsSystem.UpdateOnlySpawnRateEnd)
                {
                    return _totalPlayTime;
                }
                else if (_totalPlayTime < ZombieStatsSystem.UpdateOnlySpeedEnd)
                {
                    return ZombieStatsSystem.UpdateOnlySpawnRateEnd + (_totalPlayTime - ZombieStatsSystem.UpdateOnlySpawnRateEnd) / 2f;
                }
                else
                {
                    return ZombieStatsSystem.UpdateOnlySpawnRateEnd + (_totalPlayTime - ZombieStatsSystem.UpdateOnlySpawnRateEnd) / 2f + (_totalPlayTime - ZombieStatsSystem.UpdateOnlySpeedEnd);
                }
            }
        }

        private float TotalSpeedUpdateTime
        {
            get { return (_totalPlayTime < ZombieStatsSystem.UpdateOnlySpawnRateEnd) ? 0 : _totalPlayTime - ZombieStatsSystem.UpdateOnlySpawnRateEnd; }
        }

        public float SpawnRate
        {
            get { return _spawnRate; }
        }

        public float SpeedMultiplier
        {
            get { return _speedMultiplier; }
        }

        public float TotalTime
        {
            get { return _totalPlayTime; }
        }

        protected override void PreInitialize()
        {
            this.EntityWorld.Services.Add<IZombieStatsProvider>(this);
        }

        protected override void Initialize()
        {
            _playerInfo = this.EntityWorld.FindEntityByName(EntityNames.Player).Get<CPlayerInfo>();
        }

        protected override void Update(UpdateContext updateContext)
        {
            if (!_playerInfo.IsAlive)
            {
                return;
            }

            _totalPlayTime += updateContext.DeltaSeconds;

            _spawnRate = ZombieStatsSystem.ZombieStartSpawnTime / FlaiMath.Max(0.001f, FlaiMath.Pow(this.TotalSpawnRateUpdateTime / 2f, 0.25f));
            _speedMultiplier = FlaiMath.Max(1, FlaiMath.Pow(this.TotalSpeedUpdateTime / 4f, 1 / 10f));
        }
    }
}
