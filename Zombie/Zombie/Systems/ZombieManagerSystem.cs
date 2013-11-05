using Flai;
using Flai.CBES;
using Flai.CBES.Systems;
using Flai.Misc;
using Microsoft.Xna.Framework;
using Zombie.Model;
using Zombie.Prefabs;

namespace Zombie.Systems
{
    public class ZombieManagerSystem : EntitySystem
    {
        private readonly Timer _zombieTimer = new Timer(2f);

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
            const float Offset = Tile.Size * 2;
            World world = this.EntityWorld.GetService<World>();
            this.EntityWorld.AddEntity(Prefab.CreateInstance<ZombiePrefab>(new Vector2(
                Global.Random.NextFloat(Offset, world.Width * Tile.Size - Offset),
                Global.Random.NextFloat(Offset, world.Height * Tile.Size - Offset))));
        }
    }
}
