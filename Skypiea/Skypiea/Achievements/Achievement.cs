
namespace Skypiea.Achievements
{
    // todo: some data/way to tie the achievement to the skill
    public class Achievement
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        //
        public IAchievementProgression Progression { get; private set; }
    }
}
