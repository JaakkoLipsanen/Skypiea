using Flai.Achievements;
using Flai.CBES.Systems;
using Skypiea.Achievements;
using Skypiea.Model;

namespace Skypiea.Systems.Player
{
    public class PlayerPassiveSystem : EntitySystem
    {
        private readonly PlayerPassiveStats _playerPassiveStats = new PlayerPassiveStats();
        protected override void PreInitialize()
        {
            this.EntityWorld.Services.Add<IPlayerPassiveStats>(_playerPassiveStats);
            IAchievementManager achievementManager = this.EntityWorld.Services.TryGet<IAchievementManager>();
            if (achievementManager != null) // is null in simulation
            {
                this.ProcessAchievements(achievementManager);
                achievementManager.AchievementUnlocked += this.ProcessAchievement;
            }
        }

        private void ProcessAchievements(IAchievementManager achievementManager)
        {
            foreach (Achievement achievement in achievementManager.AllAchievements)
            {
                if (achievement.IsUnlocked)
                {
                    this.ProcessAchievement(achievement);
                }
            }
        }

        private void ProcessAchievement(Achievement achievement)
        {
            AchievementInfo achievementInfo = (AchievementInfo)achievement.Tag;
            achievementInfo.RewardPassiveSkill.ProcessPassives(_playerPassiveStats);
        }
    }
}
