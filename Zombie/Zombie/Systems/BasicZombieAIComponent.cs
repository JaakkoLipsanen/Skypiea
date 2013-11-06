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
        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.Update; }
        }

        public BasicZombieAIComponent()
            : base(Aspect.All<Components.BasicZombieAIComponent, TransformComponent>())
        {
        }

        protected override void Process(UpdateContext updateContext, EntityCollection entities)
        {
            Entity player = this.EntityWorld.FindEntityByName(EntityTags.Player);
            TransformComponent playerTransform = player.Get<TransformComponent>();
            PlayerInfoComponent playerInfo = player.Get<PlayerInfoComponent>();

            foreach (Entity entity in entities)
            {
                TransformComponent transform = entity.Get<TransformComponent>();

                const float Speed = Tile.Size;
                float movement = Speed * updateContext.DeltaSeconds;

                // player alive
                if (playerInfo.IsAlive)
                {
                    transform.RotationAsVector = playerTransform.Position - transform.Position;
                    if (Vector2.Distance(transform.Position, playerTransform.Position) < movement)
                    {
                        transform.Position = playerTransform.Position;
                    }
                    else
                    {
                        transform.Position -= Vector2.Normalize(transform.Position - playerTransform.Position) * movement;
                    }
                }
                // player dead
                else
                {
                    Vector2 direction = transform.Position != playerTransform.Position ?
                        (transform.Position - playerTransform.Position) :
                        new Vector2(Global.Random.NextFloat(-1f, 1f), Global.Random.NextFloat(-1f, 1f));

                    direction.Normalize();
                    transform.RotationAsVector = direction;
                    transform.Position += direction * movement;
                }
            }
        }
    }
}
