using Autofac.Extensions.DependencyInjection;
using Autofac;
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
using Dapper.LiteSql;
using Porvider;

var builder = WebApplication.CreateBuilder(args);

var db = new LiteSqlClient(builder.Configuration.GetConnectionString("DefaultConnection"), DBType.MySQL, new MySQLProvider());
var secondDB = new LiteSqlClient(builder.Configuration.GetConnectionString("DefaultConnection"), DBType.MySQL, new MySQLProvider());

// Add services to the container.
// ע�����ݿ�DBSession
builder.Services.AddScoped<IDBSession, IDBSession>(serviceProvider =>
{
    return db.GetSession();
});
// ע��ڶ������ݿ�DBSession
builder.Services.AddScoped<SecondDBSession, SecondDBSession>(serviceProvider =>
{
    return new SecondDBSession(secondDB.GetSession());
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
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());//ͨ�������滻����Autofac���Ͻ���
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    ServiceFactory.SetBuilder(containerBuilder);
    ServiceFactory.RegisterAssembly(Assembly.GetExecutingAssembly()); //ע�����
});

var app = builder.Build();

ServiceFactory.SetContainer((app.Services as AutofacServiceProvider).LifetimeScope as IContainer);
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
