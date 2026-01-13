using BizLink.MES.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace BizLink.MES.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });


            // 1. 加载嵌入的 appsettings.json
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("BizLink.MES.MAUI.appsettings.json");
            var config = new ConfigurationBuilder().AddJsonStream(stream).Build();
            builder.Configuration.AddConfiguration(config);

            // 2. 将配置节 "ApiSettings:MesApi" 绑定到强类型类 MesApiSettings
            builder.Services.Configure<MesApiSettings>(builder.Configuration.GetSection("ApiSettings:MesApi"));

            builder.Services.AddHttpClient<IMesApiClient, ApiClient>((serviceProvider, client) =>
            {
                // 从 DI 容器中获取已注册的强类型配置
                var apiSettings = serviceProvider.GetRequiredService<IOptions<MesApiSettings>>().Value;
                client.BaseAddress = new Uri(apiSettings.BaseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            });

            foreach (var type in assembly.GetTypes())
            {
                // 注册所有以 "ViewModel" 结尾的类 (约定优于配置)
                if (type.IsClass && !type.IsAbstract && type.Name.EndsWith("ViewModel"))
                {
                    builder.Services.AddTransient(type);
                }
                // 注册所有继承自 ContentPage 的类
                else if (type.IsClass && !type.IsAbstract && typeof(ContentPage).IsAssignableFrom(type))
                {
                    builder.Services.AddTransient(type);
                }
            }

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
