using Microsoft.International.Converters.PinYinConverter;
using System.Text;

namespace Utils
{
    /// <summary>
    /// 汉字转拼音
    /// 使用PinYinConverterCore库，性能较差，转换后的结果包含声调
    /// </summary>
    public class PinYinUtil
    {
        /// <summary>
        /// 汉字字符串转拼音字符串
        /// </summary>
        public static string GetPinYin(string s)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var c in s)
            {
                if (ChineseChar.IsValidChar(c))
                {
                    var cc = new ChineseChar(c);
                    var pinyin = cc.Pinyins;
                    if (pinyin.Count > 0)
                    {
                        sb.Append(pinyin[0]).Append(' ');
                    }
                }
                else
                {
                    sb.Append(c).Append(' ');
                }
            }
            return sb.ToString();
        }
    }
}
