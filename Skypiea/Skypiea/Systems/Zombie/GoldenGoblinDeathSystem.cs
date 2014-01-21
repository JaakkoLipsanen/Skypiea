using Flai;
using Flai.CBES.Systems;
using Flai.General;
using Flai.Graphics.Particles;
using Microsoft.Xna.Framework;
using Skypiea.Components;
using Skypiea.Messages;
using Skypiea.Misc;
using Skypiea.Model.Weapons;
using Skypiea.Prefabs;
using Skypiea.Prefabs.Drops;

namespace Skypiea.Systems.Zombie
{
    public class GoldenGoblinDeathSystem : EntitySystem
    {
        protected override void Initialize()
        {
            this.EntityWorld.SubscribeToMessage<ZombieKilledMessage>(this.OnZombieKilled);
        }

        private void OnZombieKilled(ZombieKilledMessage message)
        {
            if (message.Zombie.Get<CZombieInfo>().Type == ZombieType.GoldenGoblin)
            {
                this.SpawnGoldenGoblinExplosion(message.Zombie.Transform.Position);
                this.SpawnDrops(message.Zombie.Transform.Position);
            }
        }

        private void SpawnGoldenGoblinExplosion(Vector2 position)
        {
            IParticleEngine particleEngine = this.EntityWorld.Services.Get<IParticleEngine>();
            ParticleEffect particleEffect = particleEngine[ParticleEffectID.GoldenGoblinExplosion];

            particleEffect.Trigger(position);
        }

        private void SpawnDrops(Vector2 position)
        {
            this.SpawnWeapons(position);
            this.SpawnBlackBox(position);
            this.AddScore();


        }

        private void AddScore()
        {
            const int BaseScore = 10000;
            const int MaxExtraScore = 15000;

            int score = BaseScore + Global.Random.Next(MaxExtraScore);
            this.EntityWorld.FindEntityByName(EntityNames.Player).Get<CPlayerInfo>().Score += score;
        }

        private void SpawnBlackBox(Vector2 position)
        {
            bool spawnBlackBox = Global.Random.NextFromOdds(0.2f);
            if (spawnBlackBox)
            {
                this.EntityWorld.CreateEntityFromPrefab<BlackBoxPrefab>(position + Vector2.UnitY * 32); // waterblaster IS included!
            }
        }

        private void SpawnWeapons(Vector2 position)
        {
            int weaponCount = Global.Random.Next(2, 3);

            const int SpawnSize = 56;

            float spawnCenterHorizontal = position.X;
            float spawnLeft = spawnCenterHorizontal - weaponCount / 2f * SpawnSize;

            for (int i = 0; i < weaponCount; i++)
            {
                this.EntityWorld.CreateEntityFromPrefab<WeaponDropPrefab>(new Vector2(spawnLeft + SpawnSize * i, position.Y - SpawnSize / 2f), WeaponTypeHelper.GenerateWeaponDropType(true)); // waterblaster IS included!
            }
        }
    }
}
