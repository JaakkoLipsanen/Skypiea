using Flai;
using Flai.CBES;
using Flai.Misc;
using Microsoft.Xna.Framework;

namespace Skypiea.Components
{
    // todo: move to Flai.CBES.Components?
    public class CVirtualThumbstick : Component
    {
        public VirtualThumbStick ThumbStick { get; private set; }
        public CVirtualThumbstick(Vector2 centerPosition) // todo: use Parent.Transform instead?
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
