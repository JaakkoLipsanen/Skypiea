
using Scoreloop.CoreSocial.API;

namespace Flai.Scoreloop
{
    public class ScoreloopResponse
    {
        public IRequestController RequestSource { get; private set; }
        public bool Success { get; private set; }

        public ScoreloopError Error
        {
            get { return this.RequestSource == null ? null : this.RequestSource.Error; }
        }

        internal ScoreloopResponse(IRequestController requestController, bool success)
        {
            this.RequestSource = requestController;
            this.Success = success;
        }
    }

    public class ScoreloopResponse<T> : ScoreloopResponse
    {
        public T Data { get; private set; }

        internal ScoreloopResponse(T data, IRequestController requestController, bool success)
            : base(requestController, success)
        {
            this.Data = data;
        }
    }
}
