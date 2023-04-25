namespace Utils
{
    /// <summary>
    /// 汉字转拼音
    /// 使用TinyPinyin.Net库，性能高，内存占用低，转换后的结果不包含声调
    /// </summary>
    public class TinyPinYinUtil
    {
        /// <summary>
        /// 汉字字符串转拼音字符串
        /// </summary>
        public static string GetPinYin(string s)
        {
            return TinyPinyin.PinyinHelper.GetPinyin(s);
        }
    }
}
