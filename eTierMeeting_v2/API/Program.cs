using System.Globalization;
using System.Reflection;
using API.Configurations;
using API.Helpers.Jobs;
using API.Helpers.SignalR;
using API.Helpers.Utilities;
using Machine_API.Helpers.Hubs;
using Machine_API.Helpers.Jobs;
using Machine_API.Resources;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using NLog.Web;
using Quartz;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddCors();

    // Add services to the container.

    builder.Services.AddControllers();

    // NLog: Setup NLog for Dependency injection
    // builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddSignalR();

    bool isSAP = bool.TryParse(builder.Configuration.GetValue<string>("AppSettings:IsSAP"), out bool _isSAP) && _isSAP;
    // Setting DBContexts
    builder.Services.AddDatabaseConfiguration(builder.Configuration, isSAP);

    // AutoMapper Settings
    builder.Services.AddAutoMapperConfiguration();

    builder.Services.AddLocalization(option => option.ResourcesPath = "Resources");
    builder.Services.AddMvc()
        .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
        .AddDataAnnotationsLocalization(options =>
        {
            options.DataAnnotationLocalizerProvider = (type, factory) =>
            {
                var assemblyName = new AssemblyName(typeof(Resource).GetTypeInfo().Assembly.FullName);
                return factory.Create("Resource", assemblyName.Name);
            };
        });

    builder.Services.Configure<RequestLocalizationOptions>(
        options =>
        {
            var supportedCultures = new List<CultureInfo>
            {
                new("vi-VN"),
                new("en-US"),
                new("zh-TW"),
                new("id-ID")
            };

            options.DefaultRequestCulture = new RequestCulture("vi-VN");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });
    builder.Services.AddSingleton<LocalizationService>();

    // Add Authentication
    builder.Services.AddAuthenticationConfigufation(builder.Configuration);
    if (isSAP)
    {
        builder.Services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            // Register the job, loading the schedule from configuration
            q.AddJobAndTrigger<CreateDataFromSAPJob>(builder.Configuration);
            q.AddJobAndTrigger<UpdateLocationJob>(builder.Configuration);
            q.AddJobAndTrigger<ClearLogJob>(builder.Configuration);
            q.AddJobAndTrigger<CheckWorkJob>(builder.Configuration);
        });
        builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }

    // RepositoryAccessor and Service
    builder.Services.AddDependencyInjectionConfiguration();

    // Swagger Config
    builder.Services.AddSwaggerGenConfiguration();
    // setup signalR new version available
    builder.Services.AddHostedService<StartApplicationService>();

    // Aspose Config
    AsposeUtility.Install();

    //Exception Handling Middleware Error
    builder.Services.AddTransient<ExceptionHandlingMiddleware>();

    builder.Services.AddEndpointsApiExplorer();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseCors(o => o
        .SetIsOriginAllowed(_ => true)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
    app.UseWebSockets();
    app.UseHttpsRedirection();
    app.UseRouting();

    app.UseRequestLocalization();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapHub<HitCounter>("/hitcounter");
    app.MapHub<SignalRHub>("/signalrHub");

    app.Run();
}
catch
{
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}