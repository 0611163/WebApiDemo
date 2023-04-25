namespace Utils
{
    /// <summary>
    /// ServiceFactory的简写
    /// </summary>
    public class SF
    {
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        public static T Get<T>()
        {
            return ServiceFactory.Get<T>();
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="type">接口类型</param>
        public static object Get(Type type)
        {
            return ServiceFactory.Get(type);
        }
    }
}
