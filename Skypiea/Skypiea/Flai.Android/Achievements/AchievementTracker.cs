using Flai.CBES;

namespace Flai.Achievements
{
    public abstract class AchievementTracker
    {
        protected readonly AchievementManager _achievementManager;
        protected AchievementTracker(AchievementManager achievementManager)
        {
            _achievementManager = achievementManager;
        }

        public virtual void Initialize() { }
        public virtual void Update(UpdateContext updateContext) { }
        public virtual void Shutdown() { }
    }

    public abstract class SingleAchievementTracker : AchievementTracker
    {
        protected readonly Achievement _achievement;
        protected SingleAchievementTracker(AchievementManager achievementManager, string achievementName)
            : base(achievementManager)
        {
            Ensure.NotNullOrWhitespace(achievementName);
            _achievement = achievementManager[achievementName];
        }
    }

    public abstract class CbesAchievementTracker : AchievementTracker
    {
        protected readonly EntityWorld _entityWorld;
        protected CbesAchievementTracker(AchievementManager achievementManager, EntityWorld entityWorld)
            : base(achievementManager)
        {
            _entityWorld = entityWorld;
        }
    }

    public abstract class CbesSingleAchievementTracker : SingleAchievementTracker
    {
        protected readonly EntityWorld _entityWorld;
        protected CbesSingleAchievementTracker(AchievementManager achievementManager, EntityWorld entityWorld, string achievementName)
            : base(achievementManager, achievementName)
        {
            _entityWorld = entityWorld;
        }
    }
}
