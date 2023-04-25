using JsonA = Newtonsoft.Json;
using JsonB = System.Text.Json.Serialization;

namespace Utils
{
    /// <summary>
    /// 限速接口
    /// </summary>
    public interface RateLimitInterface
    {
        /// <summary>
        /// 是否限速
        /// </summary>
        [JsonA.JsonIgnore]
        [JsonB.JsonIgnore]
        bool IsLimit { get; }
    }
}
