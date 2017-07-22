
namespace Flai.Input
{
    public abstract class FlaiController
    {
        protected readonly FlaiServiceContainer _serviceContainer;
        protected FlaiController()
        {
            _serviceContainer = FlaiGame.Current.Services;
        }

        public abstract void Control(UpdateContext updateContext);
    }
}
