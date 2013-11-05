using Flai;
using Flai.General;
using Microsoft.Xna.Framework;

namespace Zombie.Misc
{
    public interface IVirtualThumbStickManager
    {
        VirtualThumbStick MovementThumbStick { get; }
        VirtualThumbStick RotationThumbStick { get; }
    }

    public class VirtualThumbStickManager : IVirtualThumbStickManager
    {
        public VirtualThumbStick MovementThumbStick { get; private set; }
        public VirtualThumbStick RotationThumbStick { get; private set; }

        public VirtualThumbStickManager()
        {
            const float OffsetFromBorder = 120f;
            this.MovementThumbStick = new VirtualThumbStick(new Vector2(OffsetFromBorder, FlaiGame.Current.ScreenSize.Height - OffsetFromBorder));
            this.RotationThumbStick = new VirtualThumbStick(new Vector2(FlaiGame.Current.ScreenSize.Width - OffsetFromBorder, FlaiGame.Current.ScreenSize.Height - OffsetFromBorder));
        }

        public void Update(UpdateContext updateContext)
        {
            this.MovementThumbStick.Update(updateContext);
            this.RotationThumbStick.Update(updateContext);
        }

        // no Draw here, lets make a Renderer
    }
}
