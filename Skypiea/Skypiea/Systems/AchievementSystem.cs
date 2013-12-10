using Flai;
using Flai.Achievements;
using Flai.CBES.Systems;
using Skypiea.Achievements;
using Skypiea.Achievements.Trackers;

namespace Skypiea.Systems
{
    // listener...? shouldn't it be like Broadcaster or something?? dunno..
    public interface IAchievementListener
    {
        event AchievementUnlockedDelegate AchievementUnlocked;
    }

    public class AchievementSystem : EntitySystem, IAchievementListener
    {
        public event AchievementUnlockedDelegate AchievementUnlocked;

        private readonly AchievementManager _achievementManager;
        private AchievementTrackerCollection _achievementTrackerCollection;


        public AchievementSystem()
        {
            _achievementManager = AchievementHelper.CreateAchivementManager();
        }

        protected override void Initialize()
        {
            this.EntityWorld.Services.Add<IAchievementListener>(this);

            this.CreateTrackers();
            _achievementManager.AchievementUnlocked += this.OnAchievementUnlocked;
        }

        protected override void Update(UpdateContext updateContext)
        {
            _achievementTrackerCollection.Update(updateContext);
        }

        private void CreateTrackers()
        {
            _achievementTrackerCollection = new AchievementTrackerCollection
            {
                new PersistentKillZombiesTracker(_achievementManager, this.EntityWorld, "Sunday Killer"),
                new PersistentKillZombiesTracker(_achievementManager, this.EntityWorld, "Zombie-nator"),
                new PersistentKillZombiesTracker(_achievementManager, this.EntityWorld, "Jesus Fucking Christ Antti"),
                
                new PersistentRunningTracker(_achievementManager, this.EntityWorld, "Jogger"),
                new PersistentRunningTracker(_achievementManager, this.EntityWorld, "The Marathoner"),
                new PersistentRunningTracker(_achievementManager, this.EntityWorld, "The Flash"),
            };
        }

        private void OnAchievementUnlocked(Achievement achievement)
        {
            this.AchievementUnlocked.InvokeIfNotNull(achievement);
        }
    }
}
