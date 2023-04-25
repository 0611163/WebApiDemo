using System.ComponentModel.DataAnnotations;

namespace Utils
{
    /// <summary>
    /// 不能为null、空字符串或空白字符串，且可以转换成DateTime
    /// </summary>
    public class DateTimeAttribute : ValidationAttribute
    {
        public string Format { get; set; }

        public DateTimeAttribute(string format)
        {
            Format = format;
        }

        public override bool IsValid(object value)
        {
            if (!string.IsNullOrWhiteSpace((string)value))
            {
                if (!DateUtil.Validate((string)value, Format))
                {
                    ErrorMessage = $"时间格式{Format}";
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                ErrorMessage = $"不能为空";
                return false;
            }
        }
    }
}
