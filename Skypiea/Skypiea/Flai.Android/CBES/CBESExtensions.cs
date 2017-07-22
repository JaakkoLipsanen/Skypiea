
namespace Flai.CBES
{
    public static class CBESExtensions
    {
        public static void InvokeIfNotNull(this EntityDelegate entityDelegate, Entity entity)
        {
            if (entityDelegate != null)
            {
                entityDelegate(entity);
            }
        }
    }
}
