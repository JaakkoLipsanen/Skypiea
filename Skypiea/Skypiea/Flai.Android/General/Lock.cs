
namespace Flai.General
{
    public class Lock
    {
        private int _lockCount;
        public bool IsLocked
        {
            get { return _lockCount != 0; }
        }

        public bool IsOpen
        {
            get { return _lockCount == 0; }
        }

        // can't make ++ and -- operators, since they change the variable, thus forcing the fields to be non-readonly
        public void Increase()
        {
            _lockCount++;
        }

        public void Decrease()
        {
            if (_lockCount > 0)
            {
                _lockCount--;
            }
            // else throw exception?
        }

        public void Reset()
        {
            _lockCount = 0;
        }
    }
}
