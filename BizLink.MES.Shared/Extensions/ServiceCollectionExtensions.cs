using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BizLink.MES.Shared.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 扫描并注册项目中所有的服务 (Services) 和仓储 (Repositories)
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns></returns>
        public static IServiceCollection AddProjectServices(this IServiceCollection services)
        {
            // 定义需要扫描的业务逻辑和数据访问层程序集
            // Assembly.Load 会加载指定名称的程序集
            var applicationAssembly = Assembly.Load("BizLink.MES.Application");
            var infrastructureAssembly = Assembly.Load("BizLink.MES.Infrastructure");

            // 自动注册 Application 层中所有以 "Service" 结尾的类
            RegisterServicesFromAssembly(services, applicationAssembly, "Service");

            // 自动注册 Infrastructure 层中所有以 "Repository" 结尾的类
            RegisterServicesFromAssembly(services, infrastructureAssembly, "Repository");

            RegisterServicesFromAssembly(services, applicationAssembly, "Facade");


            return services;
        }

        /// <summary>
        /// 从指定的程序集中查找并注册符合命名约定的服务
        /// </summary>
        private static void RegisterServicesFromAssembly(IServiceCollection services, Assembly assembly, string suffix)
        {
            // 获取程序集中所有满足条件的类型：
            // 1. 是一个类 (IsClass)
            // 2. 不是抽象类 (!IsAbstract)
            // 3. 类名以指定的后缀结尾 (t.Name.EndsWith(suffix))
            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith(suffix))
                .ToList();

            foreach (var type in types)
            {
                // 寻找该类实现的、且符合 "I[ClassName]" 命名约定的接口
                // 例如，对于 UserService 类，它会寻找 IUserService 接口
                var serviceInterface = type.GetInterfaces()
                    .FirstOrDefault(i => i.Name == $"I{type.Name}");

                if (serviceInterface != null)
                {
                    // 如果找到了对应的接口，则将接口和实现以 Scoped 生命周期注册到 DI 容器
                    services.AddScoped(serviceInterface, type);
                }
                else
                {
                    // 如果没有找到匹配的接口（例如一些帮助类），可以选择只注册类本身
                    services.AddScoped(type);
                }
            }
        }
    }
}
