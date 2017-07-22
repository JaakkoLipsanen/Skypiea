#if WINDOWS_PHONE
using Flai.Misc;
using Microsoft.Xna.Framework;

namespace Flai.CBES.Components
{
    // todo: move to Flai.CBES.Components?
    public class CVirtualThumbstick : Component
    {
        public VirtualThumbstick Thumbstick { get; private set; }

        public CVirtualThumbstick(Vector2 centerPosition) // todo: use Parent.Transform instead?
        {
            // instead of taking centerPosition as a variable, should this component depend on TransformComponent and get the position from it?
            this.Thumbstick = VirtualThumbstick.CreateFixed(centerPosition);
        }

        public CVirtualThumbstick(VirtualThumbstick virtualThumbstick) // todo: use Parent.Transform instead?
        {
            // instead of taking centerPosition as a variable, should this component depend on TransformComponent and get the position from it?
            this.Thumbstick = virtualThumbstick;
        }
    }
}
#endif