
namespace Skypiea.Model
{
    public interface IPlayerPassiveStats
    {
        float MovementSpeedMultiplier { get; }
        float FireRateMultiplier { get; }
        float ScoreMultiplier { get; }
        float AmmoMultiplier { get; }
        float DropIncreaseMultiplier { get; }
        float ChanceToKillEveryoneOnDeath { get; }
        float ChanceForZombieExplodeOnDeath { get; }
        float ChanceToGetTwoLivesOnDrop { get; }

        bool SpawnWithThreeLives { get; }
        bool SpawnWithRandomWeapon { get; }
        bool ZombieBirthdayParty { get; }
        bool ChanceForWaterBlaster { get; }
    }

    public class PlayerPassiveStats : IPlayerPassiveStats
    {
        public float MovementSpeedMultiplier { get; set; }
        public float FireRateMultiplier { get; set; }
        public float ScoreMultiplier { get; set; }
        public float AmmoMultiplier { get; set; }
        public float DropIncreaseMultiplier { get; set; }
        public float ChanceToKillEveryoneOnDeath { get; set; }
        public float ChanceForZombieExplodeOnDeath { get; set; }
        public float ChanceToGetTwoLivesOnDrop { get; set; }

        public bool SpawnWithThreeLives { get; set; }
        public bool SpawnWithRandomWeapon { get; set; }
        public bool ZombieBirthdayParty { get; set; }
        public bool ChanceForWaterBlaster { get; set; }

        public PlayerPassiveStats()
        {
            this.MovementSpeedMultiplier = 1;
            this.FireRateMultiplier = 1;
            this.ChanceToKillEveryoneOnDeath = 0;
            this.ScoreMultiplier = 1;
            this.AmmoMultiplier = 1;
            this.DropIncreaseMultiplier = 1;
            this.ChanceForZombieExplodeOnDeath = 0;

            this.SpawnWithThreeLives = false;
            this.SpawnWithRandomWeapon = false;
            this.ZombieBirthdayParty = false;
            this.ChanceForWaterBlaster = false;
        }
    }
}
