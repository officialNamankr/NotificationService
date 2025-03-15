using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using NotificationService.Middlewares;
using NotificationService.Services;
using NotificationService.Services.IServices;
using NotificationService.Validator;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "http://localhost:61339/";

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
// Add services to the container.
builder.Services.AddTransient<EmailNotificationService>();
builder.Services.AddSingleton<NotificationServiceFactory>();
builder.Services.AddTransient<SMSNotificationService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Notification Service API",
        Version = "v1",
        Description = "An API for sending notifications via Email and SMS."
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<NotificationMessageValidator>();
builder.Services.AddSingleton<ILogService, LogService>();
var app = builder.Build();



// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Notification API v1");
    options.RoutePrefix = string.Empty;  // Makes Swagger UI launch at root URL
});

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthorization();


app.UseMiddleware<LoggingMiddleware>();
app.MapControllers();
app.Run();


