using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Microsoft.Xna.Framework;
using Zombie.Components;
using Zombie.Misc;
using Zombie.Model;

namespace Zombie.Systems
{
    public class BasicZombieAIComponent : ProcessingSystem
    {
        public BasicZombieAIComponent()
            : base(Aspect.All<Components.BasicZombieAIComponent, TransformComponent>())
        {
        }

        protected override void Process(UpdateContext updateContext, EntityCollection entities)
        {
            Entity player = this.EntityWorld.FindEntityByName(EntityTags.Player);
            TransformComponent playerTransform = player.Get<TransformComponent>();
            foreach (Entity entity in entities)
            {
                const float Speed = Tile.Size;
                TransformComponent transform = entity.Get<TransformComponent>();
                transform.RotationAsVector = playerTransform.Position -  transform.Position;

                float distance = Speed * updateContext.DeltaSeconds;
                if (Vector2.Distance(transform.Position, playerTransform.Position) < distance)
                {
                    transform.Position = playerTransform.Position;
                }
                else
                {
                    transform.Position -= Vector2.Normalize(transform.Position - playerTransform.Position) * distance;
                }
            }
        }
    }
}
