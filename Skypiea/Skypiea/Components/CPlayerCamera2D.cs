using Flai;
using Flai.CBES.Components;
using Microsoft.Xna.Framework;

namespace Skypiea.Components
{
    public class CPlayerCamera2D : CCamera2D
    {
        protected override void Initialize()
        {
            this.Position = this.Entity.Transform.Position;
        }

        protected override void PreUpdate(UpdateContext updateContext)
        {
            this.Position = Vector2.Lerp(this.Position, this.Entity.Transform.Position, updateContext.DeltaSeconds * 5f);
        }
    }
}
