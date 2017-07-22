using System;
using Flai.DataStructures;

namespace Flai.Achievements
{
    public class AchievementGroup
    {
        private readonly ReadOnlyArray<Achievement> _achievements;

        private readonly Bag<Achievement> _returnAchievements = new Bag<Achievement>();
        private readonly ReadOnlyBag<Achievement> _readOnlyReturnAchievements; 

        public ReadOnlyArray<Achievement> Achievements
        {
            get { return _achievements; }
        }

        // todo: change to ReadOnlyArray or List or Collection

        // this is pretty meh too....
        public ReadOnlyBag<Achievement> UnlockedAchievements
        {
            get
            {
                _returnAchievements.Clear();
                for (int i = 0; i < _achievements.Count; i++)
                {
                    _returnAchievements.Add(_achievements[i]);
                }

                return _readOnlyReturnAchievements;
            }
        }

        public Achievement CurrentAchievement
        {
            get
            {
                for (int i = 0; i < _achievements.Count; i++)
                {
                    if (!_achievements[i].IsUnlocked)
                    {
                        // first one that is locked
                        return _achievements[i];
                    }
                }

                return _achievements[_achievements.Count - 1]; // last one
            }
        }

        public int UnlockedCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < _achievements.Count; i++)
                {
                    if (_readOnlyReturnAchievements[i].IsUnlocked)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        public int TotalCount
        {
            get { return _achievements.Count; }
        }

        public object Tag { get; set; }

        public AchievementGroup(Achievement[] achievements)
        {
            Ensure.NotNull(achievements);
            _achievements = new ReadOnlyArray<Achievement>(achievements);
            _readOnlyReturnAchievements = new ReadOnlyBag<Achievement>(_returnAchievements);
        }

        public int IndexOf(Achievement achievement)
        {
            for (int i = 0; i < _achievements.Count; i++)
            {
                if (achievement == _achievements[i])
                {
                    return i;
                }
            }

            throw new ArgumentException("achievement");
        }
    }
}
