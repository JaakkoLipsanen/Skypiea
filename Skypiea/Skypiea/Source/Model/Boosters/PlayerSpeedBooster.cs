
namespace Skypiea.Model.Boosters
{
    public class PlayerSpeedBooster : Booster
    {
        public override string DisplayName
        {
            get { return "Player runs faster"; }
        }

        public override bool IsPlayerBooster
        {
            get { return true; }
        }

        public readonly float SpeedMultiplier = 1.4f;
    }
}
