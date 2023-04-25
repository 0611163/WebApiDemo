using Autofac;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class ServiceFactory
    {
        private static ContainerBuilder _builder;

        private static IContainer _container;

        private static bool _isRunning; //服务是否正在运行

        public static void SetBuilder(ContainerBuilder builder)
        {
            _builder = builder;
        }

        public static void SetContainer(IContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Autofac.ContainerBuilder
        /// </summary>
        public static ContainerBuilder Builder
        {
            get
            {
                return _builder;
            }
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        public static T Get<T>()
        {
            if (_isRunning)
            {
                return _container.Resolve<T>();
            }
            else
            {
                throw new Exception("服务尚未启动完成");
            }
        }

        /// <summary>
        /// 获取服务
        /// </summary>
        /// <param name="type">接口类型</param>
        public static object Get(Type type)
        {
            if (_isRunning)
            {
                return _container.Resolve(type);
            }
            else
            {
                throw new Exception("服务尚未启动完成");
            }
        }

        #region 注册程序集
        /// <summary>
        /// 注册程序集
        /// </summary>
        /// <param name="serviceAssembly">服务程序集</param>
        public static void RegisterAssembly(Assembly serviceAssembly)
        {
            Type[] typeArr = serviceAssembly.GetTypes();

            foreach (Type type in typeArr)
            {
                if (type.GetCustomAttribute<RegisterServiceAttribute>() != null && !type.IsAbstract)
                {
                    _builder.RegisterType(type).SingleInstance().AsImplementedInterfaces().AsSelf();
                }
                else
                {
                    Type[] interfaceTypes = type.GetInterfaces();
                    foreach (Type interfaceType in interfaceTypes)
                    {
                        if (interfaceType.GetCustomAttribute<RegisterServiceAttribute>() != null && !type.IsAbstract)
                        {
                            _builder.RegisterType(type).SingleInstance().AsImplementedInterfaces().AsSelf();
                            break;
                        }
                    }
                }
            }

            //_container = _builder.Build();
        }
        #endregion

        #region 启动所有服务
        /// <summary>
        /// 启动所有服务
        /// </summary>
        public static async Task StartAllService()
        {
            Type iServiceInterfaceType = typeof(IService);
            IEnumerable<Type> types = _container.ComponentRegistry.Registrations.Select(a => a.Activator.LimitType);
            await Parallel.ForEachAsync(types, async (type, c) =>
            {
                if (iServiceInterfaceType.IsAssignableFrom(type))
                {
                    object obj = _container.Resolve(type);

                    try
                    {
                        IService service = obj as IService;
                        await service.OnStart();
                        LogUtil.Info("服务 " + obj.GetType().FullName + " 已启动");
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(ex, "服务 " + obj.GetType().FullName + " 启动失败");
                    }
                }
            });
            _isRunning = true;
        }
        #endregion

        #region 停止所有服务
        /// <summary>
        /// 停止所有服务
        /// </summary>
        public static async Task StopAllService()
        {
            Type iServiceInterfaceType = typeof(IService);
            IEnumerable<Type> types = _container.ComponentRegistry.Registrations.Select(a => a.Activator.LimitType);
            await Parallel.ForEachAsync(types, async (type, c) =>
            {
                if (iServiceInterfaceType.IsAssignableFrom(type))
                {
                    object obj = _container.Resolve(type);
                    IService service = obj as IService;

                    try
                    {
                        await service.OnStop();
                        LogUtil.Info("服务 " + obj.GetType().FullName + " 已停止");
                    }
                    catch (Exception ex)
                    {
                        LogUtil.Error(ex, "服务 " + obj.GetType().FullName + " 停止失败");
                    }
                }
            });
            _isRunning = false;
        }
        #endregion

    }
}
