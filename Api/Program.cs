using Api.DependencyInjection;
using Api.Middlewares;
using FastEndpoints;
using FastEndpoints.Swagger;
using LuxuzDev.PersonalLogger;

var builder = WebApplication.CreateBuilder(args);

#region PersonalLoggerConfiguration

PersonalLogger.Initialize();

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


var app = builder.Build();


// Revisar disponibilidad minio
await app.CheckMinioServiceAsync();

// Aplicar migraciones y seeders
await app.UseDatabaseSeederAsync();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

#region AppUse

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