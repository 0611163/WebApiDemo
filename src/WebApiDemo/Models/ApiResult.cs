namespace Models
{
    /// <summary>
    /// 接口返回值
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 成功或失败的信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }

        public ApiResult()
        {

        }

        public ApiResult(int code, string message, object data)
        {
            Code = code;
            Message = message;
            Data = data;
        }

    }
}
