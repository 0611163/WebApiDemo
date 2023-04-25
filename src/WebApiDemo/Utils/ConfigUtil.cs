using System.Reflection;

namespace Utils
{
    /// <summary>
    /// 配置文件工具类
    /// </summary>
    public class ConfigUtil
    {
        /// <summary>
        /// 获取配置文件配置
        /// </summary>
        public static IConfiguration GetConfiguration()
        {
            string basePath = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory).Replace(@"\", "/");
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(Path.Combine(basePath, "appsettings.json"));
            return builder.Build();
        }
    }
}
