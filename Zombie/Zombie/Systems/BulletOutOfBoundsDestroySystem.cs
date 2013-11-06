using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Zombie.Components;
using Zombie.Model;

namespace Zombie.Systems
{
    public class BulletOutOfBoundsDestroySystem : ComponentProcessingSystem<BulletComponent>
    {
        private World _world;
        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.PostFrame; }
        }

        protected override void Initialize()
        {
            _world = this.EntityWorld.GetService<World>();
        }

        public override void Process(UpdateContext updateContext, Entity entity, BulletComponent bullet)
        {
            RectangleF area = bullet.Area;
            if ((area.Left > _world.Width*Tile.Size + FlaiGame.Current.ScreenSize.Width) ||
                (area.Right < -FlaiGame.Current.ScreenSize.Width) ||
                (area.Top > _world.Height * Tile.Size + FlaiGame.Current.ScreenSize.Height) ||
                (area.Bottom < -FlaiGame.Current.ScreenSize.Height))
            {
                entity.Delete();
            }
        }
    }
}
