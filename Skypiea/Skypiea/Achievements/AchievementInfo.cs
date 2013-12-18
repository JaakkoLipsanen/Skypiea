using Flai;
using Flai.Achievements;
using Flai.CBES;
using Skypiea.Model;

namespace Skypiea.Achievements
{
    public delegate AchievementTracker CreateAchievementTrackerDelegate(AchievementManager achievementManager, EntityWorld entityWorld, Achievement achievement);
    public delegate void ProcessPassiveStatsDelegate(PlayerPassiveStats playerPassiveStats);

    public class AchievementInfo
    {
        private readonly CreateAchievementTrackerDelegate _createTrackerFunction;
        public readonly PassiveSkill RewardPassiveSkill;

        public AchievementInfo(CreateAchievementTrackerDelegate createTrackerFunction, PassiveSkill rewardPassiveSkill)
        {
            Ensure.NotNull(createTrackerFunction);
            Ensure.NotNull(rewardPassiveSkill);

            _createTrackerFunction = createTrackerFunction;
            this.RewardPassiveSkill = rewardPassiveSkill;
        }

        public AchievementTracker CreateTracker(AchievementManager achievementManager, EntityWorld entityWorld, Achievement achievement)
        {
            return _createTrackerFunction(achievementManager, entityWorld, achievement);
        }
    }

    public class PassiveSkill
    {
        private readonly ProcessPassiveStatsDelegate _processPassiveStatsAction;
        public string Description { get; private set; }

        public PassiveSkill(string description, ProcessPassiveStatsDelegate processPassiveStatsAction)
        {
            Ensure.NotNull(description);
            Ensure.NotNull(processPassiveStatsAction);

            this.Description = description;
            _processPassiveStatsAction = processPassiveStatsAction;
        }

        public void ProcessPassives(PlayerPassiveStats passiveStats)
        {
            _processPassiveStatsAction(passiveStats);
        }
    }
}
