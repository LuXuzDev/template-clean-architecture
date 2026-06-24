using Api.DependencyInjection;
using Api.Middlewares;
using Application;
using Application.Services.PersonalLoggerNotifier.Telegram;
using FastEndpoints;
using FastEndpoints.Swagger;
using Loop.PersonalLogger;

var builder = WebApplication.CreateBuilder(args);

GlobalAppInfo.Name = builder.Configuration["ApiInfo:Name"]!;
GlobalAppInfo.Version = builder.Configuration["ApiInfo:Version"]!;
GlobalAppInfo.RoutePrefix = builder.Configuration["ApiInfo:RoutePrefix"]!;
GlobalAppInfo.Description = builder.Configuration["ApiInfo:Description"]!;

#region PersonalLoggerConfiguration

PersonalLogger.Initialize();

if (builder.Environment.IsProduction())
{
    var telegramSettings = builder.Configuration
        .GetSection("PersonalLogger:Telegram")
        .Get<TelegramSettings>();

    var telegramNotifier = new TelegramNotifier(telegramSettings!.BotToken, telegramSettings.ChatIds);
    PersonalLogger.Configure(telegramNotifier);
}

#endregion


#region ServicesInjection

builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddAutoMapperServices(builder.Configuration)
    .AddExternalServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration);

builder.Services.AddFastEndpoints();

#endregion


#region OpenAPI Configuration (FastEndpoints + Swagger UI)

builder.Services.SwaggerDocument(options =>
{
    options.DocumentSettings = settings =>
    {
        settings.Title = GlobalAppInfo.Name;
        settings.Version = GlobalAppInfo.Version;
        settings.Description = GlobalAppInfo.Description;
    };

    options.EnableJWTBearerAuth = true;
});

#endregion


#region CORS

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAllOrigins", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });
}
else
{
    var allowedOrigins = builder.Configuration
        .GetSection("CorsSettings:AllowedOrigins")
        .Get<string[]>() ?? Array.Empty<string>();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("ProductionPolicy", policy =>
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
    });
}

#endregion

var app = builder.Build();

await app.CheckExternalHealthAsync();
await app.UseDatabaseSeederAsync();

PersonalLogger.Log("Inicio correctamente", LogType.Success, GlobalAppInfo.Name);

#region AppUse

if (builder.Environment.IsDevelopment())
{
    app.UseCors("AllowAllOrigins");
}
else
{
    app.UseCors("ProductionPolicy");
}

app.UseMiddleware<SwaggerAuthMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

// Generar Swagger UI de FastEndpoints
app.UseSwaggerGen();

// FastEndpoints
app.UseFastEndpoints(config =>
{
    config.Endpoints.RoutePrefix = GlobalAppInfo.RoutePrefix;
    config.Endpoints.ShortNames = false;
});

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

#endregion

app.Run();