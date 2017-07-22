
namespace Flai
{
    public abstract class FlaiService
    {
        protected readonly FlaiServiceContainer _services;

        protected FlaiService(FlaiServiceContainer services)
        {
            _services = services;
        }
    }
}
