
namespace Flai.CBES.Systems
{
    // okay, if somebody changes the tag of a entity after adding to world, then this thing breaks. so umm.. yeah :P
    public abstract class TagProcessingSystem : ProcessingSystem
    {
        protected TagProcessingSystem(uint tag)
            : base(Aspect.WithTag(tag))
        {
        }

        protected TagProcessingSystem(Aspect aspect, uint tag)
            : base(Aspect.Combine(Aspect.WithTag(tag), aspect))
        {
        }
    }
}