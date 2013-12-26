using System.Linq;
using Flai;
using Flai.CBES;
using Flai.CBES.Components;
using Flai.Content;
using Flai.Graphics;
using Flai.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Skypiea.Misc;
using Skypiea.Model;
using Skypiea.Model.Weapons;
using Skypiea.Prefabs;
using Skypiea.Prefabs.Bullets;
using Skypiea.Prefabs.Zombies;
using Skypiea.View;

namespace Skypiea.Screens
{
    public class SkypieaSplashScreen : PreloadAssetsScreen
    {
        public SkypieaSplashScreen() 
            : base(new MenuBackgroundScreen(), new MainMenuScreen())
        {
        }

        protected override void PreloadAssets()
        {
            // todo todo todo todo
            return;
            this.SimulateEntityWorld();
            this.LoadAssets();
        }

        protected override void DrawInner(GraphicsContext graphicsContext)
        {
            graphicsContext.GraphicsDevice.Clear(Color.Black);
        }

        private void SimulateEntityWorld()
        {
            World world = new World(WorldType.Grass);
            ParticleEffectRenderer particleEffectRenderer = new ParticleEffectRenderer(world.ParticleEngine);
            particleEffectRenderer.LoadContent();

            this.CreateAllPrefabs(world.EntityWorld);
            world.Initialize();

            world.Update(UpdateContext.Null);
            ZombieHelper.TriggerBloodSplatter(world.EntityWorld.FindEntityByName(EntityNames.Player).Transform);
            world.Update(UpdateContext.Null);

            foreach (Entity entity in world.EntityWorld.AllEntities.ToArray())
            {
                entity.Delete();
            }

            world.Shutdown();
        }

        private void CreateAllPrefabs(EntityWorld entityWorld)
        {
            Entity player = entityWorld.CreateEntityFromPrefab<PlayerPrefab>(EntityNames.Player, Vector2.Zero);
            entityWorld.CreateEntityFromPrefab<VirtualThumbStickPrefab>(EntityNames.RotationThumbStick, Vector2.Zero);
            entityWorld.CreateEntityFromPrefab<VirtualThumbStickPrefab>(EntityNames.MovementThumbStick, Vector2.Zero);

            // bullets
            entityWorld.CreateEntityFromPrefab<FlamethrowerBulletPrefab>(player.Transform, new Flamethrower());
            entityWorld.CreateEntityFromPrefab<NormalBulletPrefab>(player.Transform, new Flamethrower(), 0f);
            entityWorld.CreateEntityFromPrefab<RicochetBulletPrefab>(player.Transform, new RicochetGun(), 0f);
            entityWorld.CreateEntityFromPrefab<RocketLauncherBulletPrefab>(player.Transform, new RocketLauncher(), 0f);

            // zombies
            entityWorld.CreateEntityFromPrefab<BasicZombiePrefab>(Vector2.Zero);
            entityWorld.CreateEntityFromPrefab<FatZombiePrefab>(Vector2.Zero);
            entityWorld.CreateEntityFromPrefab<RusherZombiePrefab>(Vector2.Zero);

            // other
            entityWorld.CreateEntityFromPrefab<LifeDropPrefab>(Vector2.Zero);
            entityWorld.CreateEntityFromPrefab<WeaponDropPrefab>(Vector2.Zero, WeaponType.AssaultRifle);
        }

        private void LoadAssets()
        {
            WeaponType.AssaultRifle.ToChar();
            WeaponType.AssaultRifle.GetDisplayName();
        //  Common.CharacterToString('c');

            FlaiContentManager texureContentManager = this.ContentProvider.DefaultManager;

            this.FontContainer.GetFont("Minecraftia.16");
            this.FontContainer.GetFont("Minecraftia.20");
            this.FontContainer.GetFont("Minecraftia.24");
            this.FontContainer.GetFont("Minecraftia.32");
            this.FontContainer.GetFont("SegoeWP.16");
            this.FontContainer.GetFont("SegoeWP.24");

            texureContentManager.LoadTexture("Drops/Life");
            texureContentManager.LoadTexture("Map/CombineTest");
            texureContentManager.LoadTexture("Map/CornerFadeTexture");
            texureContentManager.LoadTexture("Map/DesertMap");
            texureContentManager.LoadTexture("Map/GrassMap");
            texureContentManager.LoadTexture("Map/SideFadeTexture");
            texureContentManager.LoadTexture("Map/Snow");

            texureContentManager.LoadTexture("BoosterTextBackground");
            texureContentManager.LoadTexture("Bullet");
            texureContentManager.LoadTexture("Fan");
            texureContentManager.LoadTexture("Heart");
            texureContentManager.LoadTexture("Laser");
            texureContentManager.LoadTexture("MainMenuBackground");
            texureContentManager.LoadTexture("Noise");
            texureContentManager.LoadTexture("RicochetBullet");
            texureContentManager.LoadTexture("Rocket");
            texureContentManager.LoadTexture("SpecialDropBase");
            texureContentManager.LoadTexture("Vignette");
            texureContentManager.LoadTexture("VignetteDithered");
            texureContentManager.LoadTexture("Zombie");
            texureContentManager.LoadTexture("ZombieShadow");
        }
    }
}
