using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class ServiceFactory
    {
        private static IServiceCollection _builder;

        private static IServiceProvider _container;

        private static bool _isRunning; //服务是否正在运行

        private static HashSet<Type> _services = new HashSet<Type>();

        public static void SetBuilder(IServiceCollection builder)
        {
            _builder = builder;
        }

        public static void SetContainer(IServiceProvider container)
        {
            _container = container;
        }

        /// <summary>
        /// Autofac.ContainerBuilder
        /// </summary>
        public static IServiceCollection Builder
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
                using (var scope = _container.CreateScope())
                {
                    return scope.ServiceProvider.GetService<T>();
                }
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
                using (var scope = _container.CreateScope())
                {
                    return scope.ServiceProvider.GetService(type);
                }
            }
            else
            {
                throw new Exception("服务尚未启动完成");
            }
        }

        #region 初始化静态类
        /// <summary>
        /// 初始化静态类
        /// </summary>
        /// <param name="serviceAssembly">服务程序集</param>
        public static async Task InitStaticClasses(Assembly serviceAssembly)
        {
            Type[] typeArr = serviceAssembly.GetTypes();

            await Parallel.ForEachAsync(typeArr, async (type, c) =>
            {
                if (type.GetCustomAttribute<StaticClassAttribute>() != null)
                {
                    try
                    {
                        RuntimeHelpers.RunClassConstructor(type.TypeHandle);
                        LogUtil.Info($"初始化静态类 {type.FullName} 成功");
                        await Task.CompletedTask;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"初始化静态类 {type.FullName} 失败：{ex}");
                        LogUtil.Error(ex, $"初始化静态类 {type.FullName} 失败");
                    }
                }
            });
        }
        #endregion

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
                var registerServiceAttribute = type.GetCustomAttribute<RegisterServiceAttribute>();
                if (registerServiceAttribute != null && !type.IsAbstract)
                {
                    if (registerServiceAttribute.ServiceLifetime == ServiceLifetime.Singleton)
                    {
                        _builder.AddSingleton(type);
                    }
                    else
                    {
                        _builder.AddScoped(type);
                    }
                    _services.Add(type);
                }
                else
                {
                    Type[] interfaceTypes = type.GetInterfaces();
                    foreach (Type interfaceType in interfaceTypes)
                    {
                        registerServiceAttribute = interfaceType.GetCustomAttribute<RegisterServiceAttribute>();
                        if (registerServiceAttribute != null && !type.IsAbstract)
                        {
                            if (registerServiceAttribute.ServiceLifetime == ServiceLifetime.Singleton)
                            {
                                _builder.AddSingleton(type);
                            }
                            else
                            {
                                _builder.AddScoped(type, interfaceType);
                            }
                            _services.Add(type);
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
            IEnumerable<Type> types = _services;
            await Parallel.ForEachAsync(types, async (type, c) =>
            {
                if (iServiceInterfaceType.IsAssignableFrom(type))
                {
                    object obj = _container.GetService(type);

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
            IEnumerable<Type> types = _services;
            await Parallel.ForEachAsync(types, async (type, c) =>
            {
                if (iServiceInterfaceType.IsAssignableFrom(type))
                {
                    object obj = _container.GetService(type);
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
