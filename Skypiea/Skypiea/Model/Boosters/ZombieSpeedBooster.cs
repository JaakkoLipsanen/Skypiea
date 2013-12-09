
namespace Skypiea.Model.Boosters
{
    public class ZombieSpeedBooster : Booster
    {
        public override string DisplayName
        {
            get { return "Zombies move faster"; }
        }

        public override bool IsPlayerBooster
        {
            get { return false; }
        }

        public readonly float SpeedMultiplier = 1.5f;
    }
}
