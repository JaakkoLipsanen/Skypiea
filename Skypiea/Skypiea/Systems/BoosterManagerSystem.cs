
using Flai;
using Flai.CBES.Systems;
using Flai.General;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model.Boosters;
using System;

namespace Skypiea.Systems
{
    public class BoosterManagerSystem : EntitySystem
    { 
        private readonly BoosterState _boosterState = new BoosterState();
        private readonly Timer _nextBoosterTimer = new Timer(float.MaxValue);
        private CPlayerInfo _playerInfo;
        private int _previousBoosterIndex = -1;

        protected override void PreInitialize()
        {
            this.EntityWorld.Services.Add<IBoosterState>(_boosterState);
            this.SetNextBoosterTimer();
            _playerInfo = this.EntityWorld.FindEntityByName(EntityNames.Player).Get<CPlayerInfo>();
        }

        protected override void Update(UpdateContext updateContext)
        {
            if (!_playerInfo.IsAlive)
            {
                _boosterState.ActiveBooster = null;
                this.SetNextBoosterTimer();
                return;
            }

            if (_boosterState.ActiveBooster != null)
            {
                this.UpdateActiveBooster(updateContext);
            }
            else
            {
                _nextBoosterTimer.Update(updateContext);
                if (_nextBoosterTimer.HasFinished)
                {
                    this.GenerateRandomBooster();
                }
            }
        }

        private void UpdateActiveBooster(UpdateContext updateContext)
        {
            _boosterState.ActiveBooster.Update(updateContext);
            if (_boosterState.ActiveBooster.HasFinished)
            {
                _boosterState.ActiveBooster = null;
                this.SetNextBoosterTimer();
            }
        }

        private void SetNextBoosterTimer()
        {
            _nextBoosterTimer.SetTickTime(Global.Random.NextFloat(20, 50));
            _nextBoosterTimer.Restart();
        }

        private void GenerateRandomBooster()
        {
            const int BoosterCount = 5;

            // don't allow two same boosters consecutively
            int newIndex = _previousBoosterIndex;
            while(newIndex == _previousBoosterIndex)
            {
                newIndex = FlaiMath.Clamp((int)(Global.Random.NextFloat() * BoosterCount), 0, BoosterCount - 1);
            }

            _boosterState.ActiveBooster = this.CreateBooster(newIndex);
            _previousBoosterIndex = newIndex;
        }

        private Booster CreateBooster(int index)
        {
            switch (index)
            {
                case 0:
                    return new PlayerSpeedBooster();

                case 1:
                    return new PlayerInvulnerabilityBooster();

                case 2:
                    return new PlayerAttackSpeedBooster();

                case 3:
                    return new ZombieSpeedBooster();

                case 4:
                    return new ZombieDamageReductionBooster();

                default:
                    throw new ArgumentOutOfRangeException("index");
            }
        }
    }
}
