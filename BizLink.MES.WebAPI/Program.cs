using BizLink.MES.Application.ApiClient;
using BizLink.MES.Application.DTOs;
using BizLink.MES.Application.Mappings;
using BizLink.MES.Application.Services;
using BizLink.MES.Domain.Common;
using BizLink.MES.Infrastructure.Persistence.DbContext;
using BizLink.MES.Shared.Extensions;
using BizLink.MES.WinForms.Common;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Options;
using SqlSugar;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// --- ?????? ---

// 1. ?? SqlSugar
builder.Services.AddAutoMapper(cfg => { }, typeof(MappingProfile));
builder.Services.AddScoped<IDbClientFactory, DbClientFactory>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// 2. ?? WebAPI ?????
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.Configure<Dictionary<string, ServiceEndpointSettings>>(builder.Configuration.GetSection("ApiSettings"));

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
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();
