using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RegionalContacts.Api;
using RegionalContacts.Domain.Interfaces.Repositories;
using RegionalContacts.Infrastructure.Repositories.SqlServer;
using RegionalContacts.Infrastructure.Repositories.SqlServer.Configurations;
using RegionalContacts.Service;
using RegionalContacts.Service.Services.Interfaces;
using System.Reflection;
using System.Text.Json.Serialization;
using RegionalContacts.Infrastructure.Repositories.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
      .AddJsonOptions(options =>
      {
          options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
      });

var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IPhoneRegionRepository, PhoneRegionRepository>();
builder.Services.AddScoped<IContactService, ContactService>();

builder.Services.AddRedis(configuration);
builder.Services.AddLogging(builder => builder.AddConsole());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Contatos", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
    {
        options.UseSqlServer(configuration.GetConnectionString("ConnectionString"));
    }, ServiceLifetime.Scoped);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Contatos API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseLoggingApi();

app.MapControllers();

app.Run();
