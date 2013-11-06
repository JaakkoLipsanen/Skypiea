using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Flai.Misc;
using Microsoft.Xna.Framework;
using Zombie.Components;
using Zombie.Misc;
using Zombie.Model;
using Zombie.Prefabs;

namespace Zombie.Systems
{
    // ZombieSpawnManagerSystem ?
    public class ZombieManagerSystem : EntitySystem
    {
        private readonly Timer _zombieTimer = new Timer(0.5f);
        protected override int ProcessOrder
        {
            get { return SystemProcessOrder.PreUpdate; }
        }

        protected override void Update(UpdateContext updateContext)
        {
            _zombieTimer.Update(updateContext);
            if (_zombieTimer.HasFinished)
            {
                this.SpawnZombie(updateContext);
                _zombieTimer.Restart();
            }
        }

        private void SpawnZombie(UpdateContext updateContext)
        {
            this.EntityWorld.AddEntity(Prefab.CreateInstance<ZombiePrefab>(this.GeneratePosition()));
        }

        private Vector2 GeneratePosition()
        {
            const float MinDistanceFromBorder = Tile.Size * 2;
            const float MinDistanceFromPlayer = Tile.Size * 4;

            World world = this.EntityWorld.GetService<World>();
            TransformComponent playerTransform = this.EntityWorld.FindEntityByName(EntityTags.Player).Get<TransformComponent>();
            while (true)
            {
                Vector2 position = new Vector2(
                    Global.Random.NextFloat(MinDistanceFromBorder, world.Width*Tile.Size - MinDistanceFromBorder),
                    Global.Random.NextFloat(MinDistanceFromBorder, world.Height*Tile.Size - MinDistanceFromBorder));

                if (Vector2.Distance(playerTransform.Position, position) > MinDistanceFromPlayer)
                {
                    return position;
                }
            }
        }
    }
}
