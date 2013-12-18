
namespace Skypiea.Model.Boosters
{
    public interface IBoosterState
    {
        Booster ActiveBooster { get; }
        bool IsActive<T>() where T : Booster;
        T GetActive<T>() where T : Booster;
    }

    public class BoosterState : IBoosterState
    {
        public Booster ActiveBooster { get; set; }

        public bool IsActive<T>()
            where T : Booster
        {
            return this.ActiveBooster is T;
        }

        public T GetActive<T>()
            where T : Booster
        {
            return (T)this.ActiveBooster;
        }
    }
}
