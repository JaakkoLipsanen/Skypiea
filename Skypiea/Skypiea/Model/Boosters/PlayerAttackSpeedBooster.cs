
namespace Skypiea.Model.Boosters
{
    public class PlayerAttackSpeedBooster : Booster
    {
        public override string DisplayName
        {
            get { return "Player fires faster"; }
        }

        public override bool IsPlayerBooster
        {
            get { return true; }
        }

        public readonly float AttackSpeedMultiplier = 1.5f;
    }
}
