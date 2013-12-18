
namespace Skypiea.Model
{
    public interface IPlayerPassiveStats
    {
        float MovementSpeedMultiplier { get; }
        float FireRateMultiplier { get; }
        float ChanceToKillEveryoneOnDeath { get; }
        float ScoreMultiplier { get; }
    }

    public class PlayerPassiveStats : IPlayerPassiveStats
    {
        public float MovementSpeedMultiplier { get; set; }
        public float FireRateMultiplier { get; set; }
        public float ChanceToKillEveryoneOnDeath { get; set; }
        public float ScoreMultiplier { get; set; }

        public PlayerPassiveStats()
        {
            this.MovementSpeedMultiplier = 1;
            this.FireRateMultiplier = 1;
            this.ChanceToKillEveryoneOnDeath = 0;
            this.ScoreMultiplier = 1;
        }
    }
}
