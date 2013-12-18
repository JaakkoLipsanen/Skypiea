using Flai.Achievements;
using Flai.CBES.Systems;
using Skypiea.Achievements;
using Skypiea.Model;

namespace Skypiea.Systems.Player
{
    public class PlayerPassiveSystem : EntitySystem
    {
        private readonly PlayerPassiveStats _playerPassiveStats = new PlayerPassiveStats();
        protected override void Initialize()
        {
            IAchievementManager achievementManager = this.EntityWorld.Services.Get<IAchievementManager>();
            this.ProcessAchievements(achievementManager);
            achievementManager.AchievementUnlocked += this.ProcessAchievement;

            this.EntityWorld.Services.Add<IPlayerPassiveStats>(_playerPassiveStats);
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
