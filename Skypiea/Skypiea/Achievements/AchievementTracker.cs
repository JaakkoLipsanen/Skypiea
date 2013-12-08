
using Flai;
using Flai.CBES;

namespace Skypiea.Achievements
{
    public abstract class AchievementTracker
    {
        protected readonly Achievement _achievement;
        protected readonly EntityWorld _entityWorld;
        protected AchievementTracker(Achievement achievement, EntityWorld entityWorld)
        {
            
        }

        public virtual void Initialize() { }
        public virtual void Update(UpdateContext updateContext) { }
    }
}
