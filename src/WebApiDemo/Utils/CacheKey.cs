using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    /// <summary>
    /// 缓存键工具类
    /// </summary>
    public static class CacheKey
    {
        #region 缓存键前缀
        /// <summary>
        /// 缓存键前缀
        /// </summary>
        public static string KeyPre
        {
            get
            {
                StackFrame stackFrame = new StackFrame(1);
                MethodBase method = stackFrame.GetMethod();

                StringBuilder result = new StringBuilder("00CE387506937BA67ADECC2736DD4AEB");

                result.AppendFormat(" {0}", method.DeclaringType.Namespace);

                result.AppendFormat(" {0}", method.DeclaringType.FullName);

                result.AppendFormat(" {0}", (method.IsGenericMethod ? "1" : "0"));

                result.AppendFormat(" {0}", method.Name);

                foreach (ParameterInfo param in method.GetParameters())
                {
                    result.AppendFormat(" {0} {1}", param.ParameterType.Name, param.Name);
                }

                return result.ToString();
            }
        }
        #endregion

    }
}
