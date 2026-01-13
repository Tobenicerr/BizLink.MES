using AutoUpdaterDotNET;
using BizLink.MES.Application.ApiClient;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Mappings;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Common;
using BizLink.MES.Domain.Repositories;
using BizLink.MES.Infrastructure.Persistence.DbContext;
using BizLink.MES.Infrastructure.Persistence.Repositories;
using BizLink.MES.Shared.Extensions;
using BizLink.MES.WinForms.Common;
using BizLink.MES.WinForms.Infrastructure;
using ClosedXML.Parser;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SqlSugar;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using System.Windows.Forms.Design;

namespace BizLink.MES.WinForms
{
    internal static class Program
    {
        private const string AppMutexName = "Global\\91844A38-0245-4E09-A603-48C09E7081E9";
        private const string UpdateXmlUrl = "http://svcn5mesp01:8001/update.xml";
        public static IServiceProvider ServiceProvider
        {
            get; private set;
        }

        [STAThread]
        static void Main()
        {
            // AutoUpdater 配置
            AutoUpdater.Synchronous = true;
            AutoUpdater.ShowSkipButton = false;
            AutoUpdater.ShowRemindLaterButton = false;
            AutoUpdater.RunUpdateAsAdmin = false;
            AutoUpdater.Start(UpdateXmlUrl);

            using (var mutex = new Mutex(true, AppMutexName, out bool newlyCreated))
            {
                if (!newlyCreated)
                {
                    MessageBox.Show("应用程序已经在运行中。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    System.Windows.Forms.Application.Exit();
                }
                else
                {
                    ApplicationConfiguration.Initialize();

                    var host = CreateHostBuilder().Build();
                    ServiceProvider = host.Services;



                    // 启动应用程序
                    // 注意：确保 LoginForm 也继承自 MesBaseForm 或者至少注册为 Transient
                    var loginForm = ServiceProvider.GetRequiredService<LoginForm>();
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        var mainForm = ServiceProvider.GetRequiredService<MainForm>();
                        System.Windows.Forms.Application.Run(mainForm);
                    }
                }
            }
        }

        static IHostBuilder CreateHostBuilder() =>
            Host.CreateDefaultBuilder().ConfigureAppConfiguration((hostingContext, config) =>
            {
                #if DEBUG
                //config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                #else
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                #endif

            })
            .ConfigureServices((context, services) =>
            {
                // --- 1. 核心基础设施 ---

                // 注册 SqlSugar & UnitOfWork
                services.AddSingleton<IDbClientFactory, DbClientFactory>();
                services.AddTransient<IUnitOfWork, UnitOfWork>();

                // 注册 HttpClient
                services.Configure<Dictionary<string, ServiceEndpointSettings>>(context.Configuration.GetSection("ApiSettings"));
                services.AddHttpClient<IMesApiClient, ApiClient>((serviceProvider, client) =>
                {
                    var apiSettings = serviceProvider.GetRequiredService<IOptions<Dictionary<string, ServiceEndpointSettings>>>().Value;
                    client.BaseAddress = new Uri(apiSettings["MesApi"].BaseUrl);
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                });

                services.AddHttpClient<IJyApiClient, ApiClient>((serviceProvider, client) =>
                {
                    var apiSettings = serviceProvider.GetRequiredService<IOptions<Dictionary<string, ServiceEndpointSettings>>>().Value;
                    client.BaseAddress = new Uri(apiSettings["JyApi"].BaseUrl);
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                });

                // --- 2. WinForms 特有服务 ---

                services.AddSingleton<ICurrentUserService, CurrentUserService>();
                services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

                // ============================================================
                // 【核心改动】 新架构注册区
                // ============================================================

                // A. 注册窗体工厂
                // 用于管理 Scope 和窗体生命周期，必须手动注册
                services.AddSingleton<IFormFactory, ScopedFormFactory>();

                // B. 自动注册所有业务服务 (Service, Repository, Facade)
                // 这里会自动扫描 Application 层以 "Facade" 结尾的类并注册
                services.AddProjectServices();

                // ============================================================

                // --- 3. 注册所有窗体 ---

                // 自动扫描当前程序集中的所有窗体
                // MesBaseForm 是 abstract 的，会自动被过滤掉
                var formTypes = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract 
                    && typeof(Form).IsAssignableFrom(t)
                    && t.Name != "MesBaseForm"
                             // 虽然这两个是抽象的(会被!IsAbstract排除)，但加上判断更安全，防止未来改为非抽象
                    && !t.Name.StartsWith("MesListForm")
                    && !t.Name.StartsWith("MesEditForm"));

                foreach (var type in formTypes)
                {
                    // 所有窗体必须注册为 Transient (每次打开都是新实例)
                    // 这样 ScopedFormFactory 才能在新的 Scope 中解析出干净的实例
                    services.AddTransient(type);
                }
            });
    }
}