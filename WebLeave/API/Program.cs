using API.Configurations;
using API.Helpers.AutoMapper;
using API.Helpers.Hubs;
using API.Helpers.Jobs;
using AutoMapper;
using Microsoft.AspNetCore.HttpOverrides;
using Quartz;

AsposeUtility.Install();

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Setting DBContexts
builder.Services.AddDatabaseConfiguration(builder.Configuration);

// Add Authentication
builder.Services.AddAuthenticationConfigufation(builder.Configuration);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IMapper>(sp => new Mapper(AutoMapperConfig.RegisterMappings()));
builder.Services.AddSingleton(AutoMapperConfig.RegisterMappings());

builder.Services.AddSignalR();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Swagger Config
builder.Services.AddSwaggerGenConfiguration();

//Exception Handling Middleware Error
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    // Register the job, loading the schedule from configuration
    q.AddJobAndTrigger<LeaveLogClearJob>(builder.Configuration);
    q.AddJobAndTrigger<LoginExpriesJob>(builder.Configuration);
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.AddHostedService<StartApplicationService>();

// RepositoryAccessor and Service
builder.Services.AddDependencyInjectionConfiguration(typeof(Program));


var app = builder.Build();

app.UseForwardedHeaders();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseWebSockets();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapHub<UserCounterHub>("/UserCounter");
app.MapHub<LoginDetectHub>("/LoginDetect");
app.MapHub<HostApplicationLifetimeHub>("/HostApplicationLifetime");

app.UseStaticFiles();

app.Run();
