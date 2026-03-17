using BizLink.MES.Application.ApiClient;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Mappings;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Common;
using BizLink.MES.Infrastructure.Extensions; // 确保引用了这个命名空间
using BizLink.MES.Infrastructure.Persistence.DbContext;
using BizLink.MES.Shared.Extensions;
using BizLink.MES.WinForms.Common;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using SqlSugar;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// --- ?????? ---

// 1. ?? SqlSugar
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));

builder.Services.Configure<Dictionary<string, ServiceEndpointSettings>>(builder.Configuration.GetSection("ApiSettings"));

// ★ 2. 使用扩展方法注册 SqlSugar 和 UnitOfWork
// 这行代码会自动读取 appsettings.json 中的 "Connections" 节点并注册所有数据库
// 替代了原来的: builder.Services.AddScoped<IDbClientFactory, DbClientFactory>();
// 替代了原来的: builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSqlSugarSetup(builder.Configuration);
// 2. ?? WebAPI ?????
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();


builder.Services.AddHttpClient<IMesApiClient, ApiClient>((serviceProvider, client) =>
{
    var apiSettings = serviceProvider.GetRequiredService<IOptions<Dictionary<string, ServiceEndpointSettings>>>().Value;
    client.BaseAddress = new Uri(apiSettings["MesApi"].BaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpClient<IJyApiClient, ApiClient>((serviceProvider, client) =>
{
    var apiSettings = serviceProvider.GetRequiredService<IOptions<Dictionary<string, ServiceEndpointSettings>>>().Value;
    client.BaseAddress = new Uri(apiSettings["JyApi"].BaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpClient<IBartApiClient, ApiClient>((serviceProvider, client) =>
{
    var apiSettings = serviceProvider.GetRequiredService<IOptions<Dictionary<string, ServiceEndpointSettings>>>().Value;
    client.BaseAddress = new Uri(apiSettings["BartenderApi"].BaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    // 关键设置：启用当前 Windows 用户的凭据
    UseDefaultCredentials = true,
});

// 3. ????????????????????????????
builder.Services.AddProjectServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

// 1. ??????????????
var provider = new FileExtensionContentTypeProvider();

// 2. ????? .apk ? MIME ????
provider.Mappings[".apk"] = "application/vnd.android.package-archive";

// 3. ?????????????
app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});


var resourcesPath = Path.Combine(builder.Environment.ContentRootPath, "Resources");
if (!Directory.Exists(resourcesPath))
{
    Directory.CreateDirectory(resourcesPath);
}
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(resourcesPath),
    RequestPath = "/Resources"
});
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();
