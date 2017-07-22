using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Flai.Achievements
{
    // hmm.. should this have more responsibiltiy? for example, if AchievementTrackers were more like EntitySystems, they could not take
    // AchievementManager and EntityWorld as a constructor parameter but instead expose them as internal property and have this class set them..?
    public class AchievementTrackerCollection : IEnumerable<AchievementTracker>
    {
        private readonly List<AchievementTracker> _achievementTrackers;
        public AchievementTrackerCollection()
            : this(Enumerable.Empty<AchievementTracker>())
        {
        }

        public AchievementTrackerCollection(IEnumerable<AchievementTracker> achievementTrackers)
        {
            _achievementTrackers = new List<AchievementTracker>(achievementTrackers);
        }

        public void Add(AchievementTracker achievementTracker)
        {
            _achievementTrackers.Add(achievementTracker);
        }

        public void Update(UpdateContext updateContext)
        {
            for (int i = 0; i < _achievementTrackers.Count; i++)
            {
                _achievementTrackers[i].Update(updateContext);
            }
        }

        public void Shutdown()
        {
            foreach (AchievementTracker achievementTracker in _achievementTrackers)
            {
                achievementTracker.Shutdown();
            }
        }

        #region Implemenation of IEnumerable<T>

        IEnumerator<AchievementTracker> IEnumerable<AchievementTracker>.GetEnumerator()
        {
            return _achievementTrackers.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _achievementTrackers.GetEnumerator();
        }

        #endregion
    }
}
