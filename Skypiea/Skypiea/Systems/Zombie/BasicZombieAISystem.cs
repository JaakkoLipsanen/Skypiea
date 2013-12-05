using System.Collections.Generic;
using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Flai.CBES.Systems;
using Flai.DataStructures;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Misc;
using Skypiea.Model;

namespace Skypiea.Systems.Zombie
{
    public class BasicZombieAISystem : ProcessingSystem
    {
        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.Update; }
        }

        public BasicZombieAISystem()
            : base(Aspect.All<CBasicZombieAI>())
        {
        }

        protected override void Process(UpdateContext updateContext, ReadOnlyBag<Entity> entities)
        {
            Entity player = this.EntityWorld.FindEntityByName(EntityNames.Player);
            CPlayerInfo playerInfo = player.Get<CPlayerInfo>();

            bool isFleeing = !playerInfo.IsAlive;

            const float Speed = Tile.Size * 2f;
            float movement = Speed * updateContext.DeltaSeconds;
            for(int i = 0; i < entities.Count; i++)
            {
                Entity entity = entities[i];

                // todo: if playerTransform.Position == transform.position, then targetVelocity will be NaN
                Vector2 targetVelocity = Vector2.Normalize(player.Transform.Position - entity.Transform.Position) * (isFleeing ? -1 : 1);

                // todo: this is really jittery/stuttering. one fix could be that basically don't normalize the separation/target vectors, and instead allow movements that are less/smaller than the "movement"/Speed
                // todo: >> also maybe the rotation could depend also on separationVelocity, but be smoothed so that the rotation isn't that jittery... also not 100% sure if GetSeparationVector works correctly (the smoothing far away -> close)
                Vector2 separationVelocity = this.GetSeparationVector(entity, entities);

                Vector2 velocity = Vector2.Normalize(targetVelocity + separationVelocity * 0.5f) * movement;
                entity.Transform.Position += velocity;
                entity.Transform.RotationVector = targetVelocity; //(transform.Position + targetVelocity);
            }
        }

        // todo: this is ultra slow. i mean really really ultra slow. do some kind of quad-tree/grid thingy
        private Vector2 GetSeparationVector(Entity entity, ReadOnlyBag<Entity> entities)
        {
            const float SeparationDistance = 32;

            Vector2 v = Vector2.Zero;
            int count = 0;
            for (int i = 0; i < entities.Count; i++)
            {
                Entity other = entities[i];
                if (other == entity)
                {
                    continue;
                }

                float distance = Vector2.Distance(entity.Transform.Position, other.Transform.Position);
                if (distance < SeparationDistance)
                {
                    v += Vector2.Normalize(entity.Transform.Position - other.Transform.Position) * FlaiMath.Pow(1 - distance / SeparationDistance, 2);
                    count++;
                }
            }

            return (v == Vector2.Zero) ? v : Vector2.Normalize(v / count);
        }
    }
}
