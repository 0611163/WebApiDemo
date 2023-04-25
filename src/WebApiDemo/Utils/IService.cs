using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    /// <summary>
    /// 服务接口
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// 服务启动
        /// </summary>
        Task OnStart();

        /// <summary>
        /// 服务停止
        /// </summary>
        Task OnStop();
    }
}
