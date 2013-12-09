
namespace Skypiea.Model.Boosters
{
    public static class BoosterHelper
    {
        // player
        public static float GetPlayerSpeedMultiplier(IBoosterState boosterState)
        {
            return boosterState.IsActive<PlayerSpeedBooster>() ? boosterState.GetActive<PlayerSpeedBooster>().SpeedMultiplier : 1;
        }

        public static bool IsPlayerInvulnerable(IBoosterState boosterState)
        {
            return boosterState.IsActive<PlayerInvulnerabilityBooster>();
        }

        public static float GetPlayerAttackSpeedMultiplier(IBoosterState boosterState)
        {
            return boosterState.IsActive<PlayerAttackSpeedBooster>() ? boosterState.GetActive<PlayerAttackSpeedBooster>().AttackSpeedMultiplier : 1;
        }

        // zombie
        public static float GetZombieSpeedMultiplier(IBoosterState boosterState)
        {
            return boosterState.IsActive<ZombieSpeedBooster>() ? boosterState.GetActive<ZombieSpeedBooster>().SpeedMultiplier : 1;
        }

        public static float GetZombieDamageRedcutionMultiplier(IBoosterState boosterState)
        {
            return boosterState.IsActive<ZombieDamageReductionBooster>() ? boosterState.GetActive<ZombieDamageReductionBooster>().DamageReductionMultiplier : 1;
        }
    }
}
