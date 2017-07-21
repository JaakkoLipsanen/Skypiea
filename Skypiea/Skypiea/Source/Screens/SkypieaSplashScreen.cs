using Flai;
using Flai.CBES;
using Flai.Content;
using Flai.Graphics;
using Flai.ScreenManagement;
using Flai.ScreenManagement.Screens;
using Microsoft.Xna.Framework;
using Skypiea.Leaderboards;
using Skypiea.Misc;
using Skypiea.Model;
using Skypiea.Model.Weapons;
using Skypiea.Prefabs;
using Skypiea.Prefabs.Bullets;
using Skypiea.Prefabs.Drops;
using Skypiea.Prefabs.Zombies;
using Skypiea.Systems;
using Skypiea.Systems.Drops;
using Skypiea.View;
using System.Linq;

namespace Skypiea.Screens
{
    public class SkypieaSplashScreen : PreloadAssetsScreen
    {
        private World _world;
        private ParticleEffectRenderer _particleEffectRenderer;
        public SkypieaSplashScreen()
            : base(new MenuBackgroundScreen(), new MainMenuScreen(FadeDirection.Down))
        {
        }

        protected override void LoadContent(bool instancePreserved)
        {
            this.SimulateEntityWorld();
            this.LoadAssets();
            
            HighscoreHelper.EnsureHighscoreIsUpToDate();
        }

        protected override void PreloadAssets()
        {
            // do the simulation in LoadContent since we aren't drawing anything while loading and we WANT to draw some stuff offscreen when the "only" draw is made
        }

        protected override void DrawInner(GraphicsContext graphicsContext)
        {
            if (_world == null)
            {
                return;
            }

            DropRenderer dropRenderer = new DropRenderer(_world.EntityWorld);
            dropRenderer.LoadContent();

            DropArrowRenderer dropArrowRenderer = new DropArrowRenderer(_world.EntityWorld);
            dropArrowRenderer.LoadContent();

            graphicsContext.SpriteBatch.Begin();
            dropRenderer.Draw(graphicsContext);
            dropArrowRenderer.Draw(graphicsContext);
            _particleEffectRenderer.Draw(graphicsContext);
            graphicsContext.SpriteBatch.End();

            graphicsContext.GraphicsDevice.Clear(Color.Black);

            foreach (Entity entity in _world.EntityWorld.AllEntities.ToArray())
            {
                entity.Delete();
            }

            _world.Shutdown();
        }

        private void SimulateEntityWorld()
        {
            _world = World.CreateSimulationWorld();

            Entity player = _world.EntityWorld.CreateEntityFromPrefab<PlayerPrefab>(EntityNames.Player, Vector2.Zero);
            _world.Initialize();

            _particleEffectRenderer = new ParticleEffectRenderer(_world.EntityWorld, _world.ParticleEngine);
            _particleEffectRenderer.LoadContent();

            this.CreatePrefabs(_world.EntityWorld, player);

            ZombieHelper.TriggerBloodSplatter(player.Transform);
            _world.Update(UpdateContext.Null);
            _world.EntityWorld.GetSystem<WeaponDropGeneratorSystem>().CreateWeaponDrop();

            this.SimulateScreen<LeaderboardScreen>();
            this.SimulateScreen<AchievementScreen>();
            this.SimulateScreen<OptionsScreen>();
        }

        private void CreatePrefabs(EntityWorld entityWorld, Entity player)
        {
            // bullets
            entityWorld.CreateEntityFromPrefab<FlamethrowerBulletPrefab>(player.Transform, new Flamethrower(1));
            entityWorld.CreateEntityFromPrefab<NormalBulletPrefab>(player.Transform, new Flamethrower(1), 0f);
            entityWorld.CreateEntityFromPrefab<BouncerBulletPrefab>(player.Transform, new Bouncer(1), 0f);
            entityWorld.CreateEntityFromPrefab<RocketLauncherBulletPrefab>(player.Transform, new RocketLauncher(1), 0f);

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

            // todo: segoes aren't probably needed in final versio, remember to remove them
            this.FontContainer.GetFont("Minecraftia.16");
            this.FontContainer.GetFont("Minecraftia.20");
            this.FontContainer.GetFont("Minecraftia.24");
            this.FontContainer.GetFont("Minecraftia.32");
            this.FontContainer.GetFont("SegoeWP.16");
            this.FontContainer.GetFont("SegoeWP.24");

            // todo: spritesheet. reduces loading time too probably by a lot
            //   texureContentManager.LoadTexture("Drops/Life");
            //   texureContentManager.LoadTexture("Map/CombineTest");
            //   texureContentManager.LoadTexture("Map/CornerFadeTexture");
            //   texureContentManager.LoadTexture("Map/DesertMap");
            //   texureContentManager.LoadTexture("Map/GrassMap");
            //   texureContentManager.LoadTexture("Map/SideFadeTexture");
            //   texureContentManager.LoadTexture("Map/Snow");

            //   texureContentManager.LoadTexture("BoosterTextBackground");
            //   texureContentManager.LoadTexture("Bullet");
            //   texureContentManager.LoadTexture("Fan");
            //   texureContentManager.LoadTexture("Heart");
            //   texureContentManager.LoadTexture("Laser");
            //   texureContentManager.LoadTexture("MainMenuBackground");
            //   texureContentManager.LoadTexture("Noise");
            //   texureContentManager.LoadTexture("RicochetBullet");
            //   texureContentManager.LoadTexture("Rocket");
            //// texureContentManager.LoadTexture("SpecialDropBase");
            //   texureContentManager.LoadTexture("Vignette");
            //   texureContentManager.LoadTexture("VignetteDithered");
            //   texureContentManager.LoadTexture("Zombie");
            //   texureContentManager.LoadTexture("ZombieShadow");
        }

        private void SimulateScreen<T>()
            where T : GameScreen, new()
        {
            T screen = new T();

            this.ScreenManager.AddScreen(screen);
            GameScreenSimulator.Update(UpdateContext.Null, screen);
            GameScreenSimulator.Draw(FlaiGame.Current.GraphicsContext, screen);
            this.ScreenManager.RemoveScreen(screen);
        }
    }
}
