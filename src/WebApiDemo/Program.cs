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
// ע�����ݿ�IDapperLiteClient
builder.Services.AddSingleton<IDapperLite>(db);
// ע��ڶ������ݿ�IDapperLiteClient
builder.Services.AddSingleton<IDapperLite<SecondDbFlag>>(secondDB);
// ע�����ݿ�DbSession

builder.Services.AddScoped<IDbSession>(serviceProvider =>
{
    return serviceProvider.GetService<IDapperLite>().GetSession();
});
// ע��ڶ������ݿ�DbSession
builder.Services.AddScoped<IDbSession<SecondDbFlag>>(serviceProvider =>
{
    return serviceProvider.GetService<IDapperLite<SecondDbFlag>>().GetSession();
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//swagger����
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = ".NET Web API ʾ���ӿ�",
        Version = "1.0",
        Description = ".NET Web API ʾ���ӿ�"
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
    var path = Path.Combine(AppContext.BaseDirectory, "WebApiDemo.xml"); // xml�ĵ�����·��
    c.IncludeXmlComments(path, true); // ��ʾ��������ע��
    c.TagActionsBy(a =>
    {
        var tagAttr = a.ActionDescriptor.EndpointMetadata.OfType<TagsAttribute>().FirstOrDefault();
        if (tagAttr != null)
        {
            return tagAttr.Tags.ToList();
        }
        return new List<string>() { "δ����" };
    });
    c.DocumentFilter<TagReorderDocumentFilter>(); // Workaround: After adding XML controller descriptions, they are listed out of alphabetical order
    c.OrderActionsBy(a => a.RelativePath); // ��Action����
    c.OperationFilter<SwaggerOperationFilter>(); //��ʾtoken�����
});

//������
builder.Services.AddMvc(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
    options.Filters.Add<ResultFilterAttribute>();
    options.Filters.Add<RateLimitFilter>();
    options.Filters.Add<ActionFilter>();
});

//�����Զ���֤
builder.Services.AddFluentValidationAutoValidation();
//��ȡ��ǰdll�е�����Validators
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    //��֤ʧ��ʱ����
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

// ASP.NET Core����Autofac
/*
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());//ͨ�������滻����Autofac���Ͻ���
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    ServiceFactory.SetBuilder(containerBuilder);
    ServiceFactory.RegisterAssembly(Assembly.GetExecutingAssembly()); //ע�����
});
*/
ServiceFactory.SetBuilder(builder.Services);
ServiceFactory.RegisterAssembly(Assembly.GetExecutingAssembly()); //ע�����

var app = builder.Build();

Task.Run(async () => await ServiceFactory.InitStaticClasses(Assembly.GetExecutingAssembly())); //��ʼ����̬��
ServiceFactory.SetContainer(app.Services);
Task.Run(async () => await ServiceFactory.StartAllService()); //��������ע�⣺�����������֮ǰ�����ýӿڻ��쳣

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "�汾1.0");
        c.DocExpansion(DocExpansion.None); //�۵�
        c.DefaultModelsExpandDepth(-1); //����models
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
