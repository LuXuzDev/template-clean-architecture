using Api.DependencyInjection;
using Api.Middlewares;
using Application.Services.PersonalLoggerNotifier;
using Application.Services.PersonalLoggerNotifier.Telegram;
using FastEndpoints;
using FastEndpoints.Swagger;
using Loop.PersonalLogger;

var builder = WebApplication.CreateBuilder(args);

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

#endregion


#region SwaggerConfiguration

builder.Services.SwaggerDocument(o =>
    o.DocumentSettings = s =>
    {
        s.Title = "My API";
        s.Version = "v1";
        s.Description = "My API description";
    }
);

#endregion


#region Configuración de CORS para DESARROLLO

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
// Configuración de CORS para PRODUCCIÓN
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



// Revisar disponibilidad de servicios Externos
await app.CheckExternalHealthAsync();

// Aplicar migraciones y seeders
await app.UseDatabaseSeederAsync();

PersonalLogger.Log("Inicio correctamente", LogType.Success, PersonalLoggerName.Name);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

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

app.UseFastEndpoints(config =>
{
    config.Endpoints.ShortNames = false;
    config.Endpoints.RoutePrefix = "my-api";
})
.UseSwaggerGen(uiConfig: ui =>
{
    ui.DocumentTitle = "Template API";
});

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();

#endregion


app.Run();