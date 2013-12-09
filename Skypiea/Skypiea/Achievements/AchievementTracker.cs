
using Flai;
using System.Collections.Generic;
using System.Linq;
using Flai.CBES;
using Skypiea.Messages;

namespace Skypiea.Achievements
{
    public abstract class AchievementTracker
    {
        protected readonly Achievement _achievement;
        protected AchievementTracker(Achievement achievement)
        {
            _achievement = achievement;
        }

        public virtual void Initialize() { }
        public virtual void Update(UpdateContext updateContext) { }
    }

    public interface IPlayerStats
    {
        int ZombiesKilled { get; }
    }

    public class KillAchivementTracker : AchievementTracker
    {
        public EntityWorld EntityWorld { get; set; }
        public KillAchivementTracker(Achievement achivement)
            : base(achivement)
        {
            Ensure.True(achivement.Progression is IntProgression);
        }

        public override void Initialize()
        {
            this.EntityWorld.SubscribeToMessage<ZombieKilledMessage>(this.OnZombieKilled);
        }

        public override void Update(UpdateContext updateContext)
        {
            if (!_achievement.IsUnlocked)
            {
                var stats = this.EntityWorld.Services.Get<IPlayerStats>();
                if (stats.ZombiesKilled > 1000)
                {

                }
            }
        }

        private void OnZombieKilled(ZombieKilledMessage message)
        {
            if (!_achievement.IsUnlocked)
            {
                ((IntProgression) _achievement.Progression).Current++;
            }
        }
    }

    public class AchivementCollection
    {
        private readonly Dictionary<string, Achievement> _achievements = new Dictionary<string, Achievement>();
        public AchivementCollection(IEnumerable<Achievement> achievements)
        {
            Ensure.NotNull(achievements);
            _achievements = achievements.ToDictionary(achivement => achivement.Name, achievement => achievement);
        }
    }
}
