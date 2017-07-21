
namespace Skypiea.Model.Boosters
{
    public class ZombieDamageReductionBooster : Booster
    {
        public override string DisplayName
        {
            get { return "Zombies take less damage"; }
        }

        public override bool IsPlayerBooster
        {
            get { return false; }
        }

        public readonly float DamageReductionMultiplier = 0.4f;
    }
}
