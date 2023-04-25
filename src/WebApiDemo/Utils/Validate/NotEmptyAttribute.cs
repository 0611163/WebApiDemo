using System.ComponentModel.DataAnnotations;
using System.DirectoryServices.ActiveDirectory;

namespace Utils
{
    /// <summary>
    /// 不能为null、空字符串或空白字符串
    /// </summary>
    public class NotEmptyAttribute : ValidationAttribute
    {
        public NotEmptyAttribute()
        {
            ErrorMessage = "不能为空";
        }

        public override bool IsValid(object value)
        {
            return !string.IsNullOrWhiteSpace((string)value);
        }
    }
}
