using Flai.DataStructures;
using Flai.General;
using Flai.IO;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Flai.Achievements
{
    public class AchievementManager : PersistentManager
    {
        public event AchievementUnlockedDelegate AchievementUnlocked;

        private readonly Dictionary<string, Achievement> _achievementsByName = new Dictionary<string, Achievement>();
        private readonly Dictionary<string, AchievementGroup> _achievementGroupsByName = new Dictionary<string, AchievementGroup>();
        private readonly Dictionary<Achievement, AchievementGroup> _groupsByAchievement = new Dictionary<Achievement, AchievementGroup>();
        private readonly ReadOnlyArray<Achievement> _achievements;

        public ReadOnlyArray<Achievement> Achievements
        {
            get { return _achievements; }
        }

        // todo: UnlockedAchievements

        public AchievementManager(string filePath, IEnumerable<Achievement> achievements)
            : base(filePath)
        {
            Ensure.NotNull(achievements);
            _achievements = new ReadOnlyArray<Achievement>(achievements.ToArray());
            _achievementsByName = _achievements.ToDictionary(achivement => achivement.Name);

            this.Load();
            this.HookAchievementUnlockedEvents();
        }

        #region Public methods

        public Achievement this[string name]
        {
            get { return _achievementsByName[name]; }
        }

        // meh. i mean really meh, there's no guarantee that achievement in the group contains only achievements that are in this manager. but what-fucking-ever
        public void AddGroup(string name, AchievementGroup achievementGroup)
        {
            _achievementGroupsByName.Add(name, achievementGroup);
            for (int i = 0; i < achievementGroup.Achievements.Count; i++)
            {
                _groupsByAchievement.Add(achievementGroup.Achievements[i], achievementGroup);
            }
        }

        public AchievementGroup GetGroup(string name)
        {
            return _achievementGroupsByName[name];
        }

        public bool IsPartOfAnyGroup(Achievement achievement)
        {
            return _groupsByAchievement.ContainsKey(achievement);
        }

        public AchievementGroup GetGroupOf(Achievement achievement)
        {
            return _groupsByAchievement[achievement];
        }

        // oh my fucking god this is awful name. basically get's all achievements, but if the achievement is part of any group, then return it only if
        // it is "the latest" (aka, either if all achievements are unlocked then the last one, or else it is the next one to be unlocked)
        // todo: this is generates a ton of garbage
        public Achievement[] GetAchievementsWithoutGroups()
        {
            // ..... cache these collections? no need for thread safety, it isnt anyway
            List<Achievement> achievements = new List<Achievement>(_achievements.Count);
            for (int i = 0; i < _achievements.Length; i++)
            {
                Achievement achievement = _achievements[i];
                AchievementGroup group = _groupsByAchievement.TryGetValue(achievement);

                // if the achievement is not in group OR if the achivement is the "current" achievement (the next one to be unlocked)
                if (group == null || group.CurrentAchievement == achievement)
                {
                    achievements.Add(achievement);
                }
            }

            return achievements.ToArray();
        }

        #endregion

        #region Write/Read

        protected override void WriteInner(BinaryWriter writer)
        {
            writer.Write(_achievements.Count);
            for (int i = 0; i < _achievements.Length; i++)
            {
                writer.WriteString(_achievements[i].Name);
                writer.Write(_achievements[i].Progression);

                IBinarySerializable tag = _achievements[i].Tag as IBinarySerializable;
                if (tag != null) // ... i guess this is ok
                {
                    writer.Write(tag);
                }
            }
        }

        protected override void ReadInner(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                string name = reader.ReadString(); // todo: make sure that achievment with name of 'name' exists?
                reader.Read(this[name].Progression);

                IBinarySerializable tag = _achievements[i].Tag as IBinarySerializable;
                if (tag != null) // ... i guess this is ok
                {
                    reader.Read(tag);
                }
            }
        }

        #endregion

        private void HookAchievementUnlockedEvents()
        {
            foreach (Achievement achievement in _achievements)
            {
                Achievement cachedAchievement = achievement;
                achievement.Unlocked += () => this.OnAchievementUnlocked(cachedAchievement);
            }
        }

        private void OnAchievementUnlocked(Achievement achievement)
        {
            this.AchievementUnlocked.InvokeIfNotNull(achievement);
        }
    }
}
