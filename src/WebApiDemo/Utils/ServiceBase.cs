namespace Utils
{
    [RegisterService]
    public abstract class ServiceBase : IService
    {
        #region OnStart
        public virtual Task OnStart()
        {
            return Task.CompletedTask;
        }
        #endregion

        #region OnStop
        public virtual Task OnStop()
        {
            return Task.CompletedTask;
        }
        #endregion

    }
}
