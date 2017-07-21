using Flai;
using Flai.General;

namespace Skypiea.Model.Boosters
{
    public abstract class Booster
    {
        private const float DefaultBoosterLength = 15;
        private float _timeRemaining = Booster.DefaultBoosterLength;

        public abstract string DisplayName { get; }
        public abstract bool IsPlayerBooster { get; }

        public int TimeRemaining
        {
            get { return (int)FlaiMath.Ceiling(_timeRemaining); }
        }

        public bool HasFinished
        {
            get { return _timeRemaining <= 0; }
        }

        public void Update(UpdateContext updateContext)
        {
            _timeRemaining -= updateContext.DeltaSeconds;
            if (_timeRemaining < 0)
            {
                _timeRemaining = 0;
            }
        }
    }

    public static class Booster<T>
        where T : Booster
    {
        public static readonly int ID = (int)TypeID<Booster>.GetID<T>();
    }
}
