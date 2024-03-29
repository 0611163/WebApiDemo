using Filters;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using Utils;
using System.Net;
using Models;
using Dapper.Lite;
using Porvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebApiDemo.Services;

var builder = WebApplication.CreateBuilder(args);

var db = new DapperLite(builder.Configuration.GetConnectionString("DefaultConnection"), new MySQLProvider());
var secondDB = new DapperLite<SecondDbFlag>(builder.Configuration.GetConnectionString("DefaultConnection"), new MySQLProvider());

// Add services to the container.
// 注册数据库IDapperLiteClient
builder.Services.AddSingleton<IDapperLite>(db);
// 注册第二个数据库IDapperLiteClient
builder.Services.AddSingleton<IDapperLite<SecondDbFlag>>(secondDB);
// 注册数据库DbSession

builder.Services.AddScoped<IDbSession>(serviceProvider =>
{
    return serviceProvider.GetService<IDapperLite>().GetSession();
});
// 注册第二个数据库DbSession
builder.Services.AddScoped<IDbSession<SecondDbFlag>>(serviceProvider =>
{
    return serviceProvider.GetService<IDapperLite<SecondDbFlag>>().GetSession();
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//swagger配置
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = ".NET Web API 示例接口",
        Version = "1.0",
        Description = ".NET Web API 示例接口"
    });
    c.AddSecurityDefinition("auth", new OpenApiSecurityScheme()
    {
        Name = "token",
        In = ParameterLocation.Header
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference() {
                    Id = "auth",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
    var path = Path.Combine(AppContext.BaseDirectory, "WebApiDemo.xml"); // xml文档绝对路径
    c.IncludeXmlComments(path, true); // 显示控制器层注释
    c.TagActionsBy(a =>
    {
        var tagAttr = a.ActionDescriptor.EndpointMetadata.OfType<TagsAttribute>().FirstOrDefault();
        if (tagAttr != null)
        {
            return tagAttr.Tags.ToList();
        }
        return new List<string>() { "未分组" };
    });
    c.DocumentFilter<TagReorderDocumentFilter>(); // Workaround: After adding XML controller descriptions, they are listed out of alphabetical order
    c.OrderActionsBy(a => a.RelativePath); // 对Action排序
    c.OperationFilter<SwaggerOperationFilter>(); //显示token输入框
});

//拦截器
builder.Services.AddMvc(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
    options.Filters.Add<ResultFilterAttribute>();
    options.Filters.Add<RateLimitFilter>();
    options.Filters.Add<ActionFilter>();
});

//启动自动验证
builder.Services.AddFluentValidationAutoValidation();
//获取当前dll中的所有Validators
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    //验证失败时触发
    options.InvalidModelStateResponseFactory = (context) =>
    {
        List<string> errList = new List<string>();
        foreach (string key in context.ModelState.Keys)
        {
            var errors = context.ModelState[key];
            foreach (var error in errors.Errors)
            {
                errList.Add($"{key}{error.ErrorMessage}");
            }
        }

        return new BadRequestObjectResult(new ApiResult
        {
            Code = (int)HttpStatusCode.BadRequest,
            Message = string.Join('|', errList),
            Data = null
        });
    };
});

// ASP.NET Core整合Autofac
/*
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());//通过工厂替换，把Autofac整合进来
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    ServiceFactory.SetBuilder(containerBuilder);
    ServiceFactory.RegisterAssembly(Assembly.GetExecutingAssembly()); //注册服务
});
*/
ServiceFactory.SetBuilder(builder.Services);
ServiceFactory.RegisterAssembly(Assembly.GetExecutingAssembly()); //注册服务

var app = builder.Build();

Task.Run(async () => await ServiceFactory.InitStaticClasses(Assembly.GetExecutingAssembly())); //初始化静态类
ServiceFactory.SetContainer(app.Services);
Task.Run(async () => await ServiceFactory.StartAllService()); //启动服务，注意：服务启动完成之前，调用接口会异常

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "版本1.0");
        c.DocExpansion(DocExpansion.None); //折叠
        c.DefaultModelsExpandDepth(-1); //隐藏models
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
