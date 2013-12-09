
using Flai;
using Flai.CBES.Systems;
using Flai.General;
using Skypiea.Model.Boosters;

namespace Skypiea.Systems
{
    public class BoosterManagerSystem : EntitySystem
    {
        private readonly BoosterState _boosterState = new BoosterState();
        private readonly Timer _nextBoosterTimer = new Timer(float.MaxValue);

        protected override void PreInitialize()
        {
            this.EntityWorld.Services.Add<IBoosterState>(_boosterState);
            this.SetNextBoosterTimer();
        }

        protected override void Update(UpdateContext updateContext)
        {
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
                _boosterState.SetActive(null);
                this.SetNextBoosterTimer();
            }
        }

        private void SetNextBoosterTimer()
        {
            _nextBoosterTimer.SetTickPeriod(Global.Random.NextFloat(25, 60));
            _nextBoosterTimer.Restart();
        }

        private void GenerateRandomBooster()
        {
            float random = Global.Random.NextFloat();
            if (random < 0.2f)
            {
                _boosterState.SetActive(new PlayerSpeedBooster());
            }
            else if (random < 0.4f)
            {
                _boosterState.SetActive(new PlayerInvulnerabilityBooster());
            }
            else if (random < 0.6f)
            {
                _boosterState.SetActive(new PlayerAttackSpeedBooster());
            }
            else if (random < 0.8f)
            {
                _boosterState.SetActive(new ZombieSpeedBooster());
            }
            else
            {
                _boosterState.SetActive(new ZombieDamageReductionBooster());
            }
        }
    }
}
