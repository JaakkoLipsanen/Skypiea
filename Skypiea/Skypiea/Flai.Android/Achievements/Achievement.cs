namespace Flai.Achievements
{
    public delegate void AchievementUnlockedDelegate(Achievement achievement);

    // hmm... should the Achievement even have "Progression" or should it be more abstract...? maybe it's okay..
    public class Achievement
    {
        public event GenericEvent Unlocked;
        public virtual string Name { get; protected set; }
        public virtual string Description { get; protected set; }
        public virtual string ShortDescription { get; private set; }

        // blahh..
        public AchievementProgression Progression { get; private set; }
        public int Points { get; private set; }
        public object Tag { get; set; } // misc info

        public bool IsUnlocked
        {
            get { return this.Progression.IsUnlocked; }
        }

        public Achievement(string name, string description, AchievementProgression achievementProgression)
            : this(name, description, null, achievementProgression, 0)
        {
        }

        public Achievement(string name, string description, string shortDescription, AchievementProgression achievementProgression)
            : this(name, description, shortDescription, achievementProgression, 0)
        {
        }

        public Achievement(string name, string description, AchievementProgression achievementProgression, int points)
            : this(name, description, null, achievementProgression, points)
        {
        }

        public Achievement(string name, string description, string shortDescription, AchievementProgression achievementProgression, int points)
        {
            Ensure.NotNullOrWhitespace(name);
            Ensure.NotNull(achievementProgression); // a bit meh...

            this.Name = name;
            this.Description = description ?? "";
            this.ShortDescription = shortDescription ?? description;
            this.Progression = achievementProgression;
            this.Points = points;

            this.Progression.Unlocked += () => this.Unlocked.InvokeIfNotNull();
        }

        protected void Unlock()
        {
            this.Unlocked.InvokeIfNotNull();
        }
    }

    // todo: don't force achievements to have "progression" ...
}
