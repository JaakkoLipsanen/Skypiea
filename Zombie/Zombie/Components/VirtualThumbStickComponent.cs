using Flai;
using Flai.CBES;
using Flai.General;
using Microsoft.Xna.Framework;

namespace Zombie.Components
{
    public class VirtualThumbStickComponent : Component
    {
        public VirtualThumbStick ThumbStick { get; private set; }
        public VirtualThumbStickComponent(Vector2 centerPosition)
        {
            // instead of taking centerPosition as a variable, should this component depend on TransformComponent and get the position from it?
            this.ThumbStick = new VirtualThumbStick(centerPosition);
        }

        protected override void PreUpdate(UpdateContext updateContext)
        {
            this.ThumbStick.Update(updateContext);
        }
    }
}
