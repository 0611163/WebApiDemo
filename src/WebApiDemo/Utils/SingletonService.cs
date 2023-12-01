namespace Utils
{
    [RegisterService(ServiceLifetime.Singleton)]
    public abstract class SingletonService : IService
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
