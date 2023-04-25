namespace Utils
{
    /// <summary>
    /// 接口限速
    /// </summary>
    public class RateLimitAttribute : Attribute
    {
        private SemaphoreSlim _sem;

        public SemaphoreSlim Sem
        {
            get
            {
                return _sem;
            }
        }

        /// <summary>
        /// 超时时间(单位:毫秒)
        /// </summary>
        private int _timeout;

        /// <summary>
        /// 超时时间(单位:毫秒)
        /// </summary>
        public int Timeout
        {
            get
            {
                return _timeout;
            }
        }

        /// <summary>
        /// 接口限速
        /// </summary>
        /// <param name="limitCount">限制并发数量</param>
        /// <param name="timeout">超时时间(单位:秒)</param>
        public RateLimitAttribute(int limitCount = 1, int timeout = 0)
        {
            _sem = new SemaphoreSlim(limitCount, limitCount);
            _timeout = timeout * 1000;
        }
    }
}
