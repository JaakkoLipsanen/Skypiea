
using Flai;

namespace Skypiea.Achievements
{
    // todo: some data/way to tie the achievement to the skill
    public class Achievement
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        // blahh..
        public IAchievementProgression Progression { get; private set; }
        public object Tag { get; set; } // misc info
     // public int Points { get; } ?????

        public bool IsUnlocked
        {
            get { return this.Progression.IsUnlocked; }
        }

        public Achievement(string name, string description, IAchievementProgression achievementProgression)
        {
            Ensure.NotNullOrWhitespace(name);
            Ensure.NotNull(achievementProgression);

            this.Name = name;
            this.Description = description ?? "";
            this.Progression = achievementProgression;
        }
    }
}
