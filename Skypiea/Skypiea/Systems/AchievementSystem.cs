using Flai;
using Flai.Achievements;
using Flai.CBES.Systems;
using Flai.DataStructures;
using Skypiea.Achievements;
using Skypiea.Messages;

namespace Skypiea.Systems
{
    public interface IAchievementManager
    {
        event AchievementUnlockedDelegate AchievementUnlocked;
        ReadOnlyArray<Achievement> AllAchievements { get; }
    }

    public class AchievementSystem : EntitySystem, IAchievementManager
    {
        public event AchievementUnlockedDelegate AchievementUnlocked;

        private readonly AchievementManager _achievementManager;
        private AchievementTrackerCollection _achievementTrackerCollection;

        public ReadOnlyArray<Achievement> AllAchievements
        {
            get { return _achievementManager.Achievements; }
        }

        public AchievementSystem()
        {
            _achievementManager = AchievementHelper.CreateAchivementManager();
        }

        protected override void PreInitialize()
        {
            this.EntityWorld.Services.Add<IAchievementManager>(this);
        }

        protected override void Initialize()
        {
            this.EntityWorld.SubscribeToMessage<GameOverMessage>(message => this.OnGameOver());
            this.EntityWorld.SubscribeToMessage<GameExitMessage>(message => this.OnGameOver());

            this.CreateTrackers();
            _achievementManager.AchievementUnlocked += this.OnAchievementUnlocked;
        }

        protected override void Shutdown()
        {
            _achievementTrackerCollection.Shutdown();
            _achievementManager.SaveToFile();
        }

        protected override void Update(UpdateContext updateContext)
        {
            _achievementTrackerCollection.Update(updateContext);
        }

        private void CreateTrackers()
        {
            _achievementTrackerCollection = new AchievementTrackerCollection();
            foreach (Achievement achievement in _achievementManager.Achievements)
            {
                AchievementInfo achievementInfo = (AchievementInfo)achievement.Tag;
                if (achievementInfo.HasTracker)
                {
                    _achievementTrackerCollection.Add(achievementInfo.CreateTracker(_achievementManager, this.EntityWorld, achievement));
                }
            }
        }

        private void OnAchievementUnlocked(Achievement achievement)
        {
            this.AchievementUnlocked.InvokeIfNotNull(achievement);
        }

        private void OnGameOver()
        {
            _achievementManager.SaveToFile();
        }
    }
}
