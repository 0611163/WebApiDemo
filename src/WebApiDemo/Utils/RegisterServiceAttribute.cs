using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Extensions.DependencyInjection;

namespace Utils
{
    /// <summary>
    /// 注册为服务
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public class RegisterServiceAttribute : Attribute
    {
        public ServiceLifetime ServiceLifetime { get; set; }

        public RegisterServiceAttribute(ServiceLifetime serviceLifetime)
        {
            ServiceLifetime = serviceLifetime;
        }
    }
}