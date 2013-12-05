using Flai;

namespace Skypiea.Model
{
    public class Level
    {
        private readonly World _world;

        public World World
        {
            get { return _world; }
        }

        private Level(World world)
        {
            _world = world;
        }

        public void Initialize()
        {
            _world.Initialize();
        }

        public void Update(UpdateContext updateContext)
        {
            _world.Update(updateContext);
        }

        public static Level Generate()
        {
            return new Level(WorldGenerator.Generate());
        }
    }
}
