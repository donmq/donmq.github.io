using API.Configurations;
using Quartz;
using API.Helper.Jobs;
using API.Helper.SignalR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();

// Add services to the container.

builder.Services.AddControllers();

// Setting DBContexts
builder.Services.AddDatabaseConfiguration(builder.Configuration);

// Add Authentication
builder.Services.AddAuthenticationConfigufation(builder.Configuration);

builder.Services.AddQuartz(q =>
{
    // Cấu hình Quartz ở đây nếu cần
    q.UseMicrosoftDependencyInjectionJobFactory();
    q.AddJobAndTrigger<EmployeeTransferHistoryJobs>(builder.Configuration);
});
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

// RepositoryAccessor and Service
builder.Services.AddDependencyInjectionConfiguration(typeof(Program));

// Swagger Config
builder.Services.AddSwaggerGenConfiguration();

builder.Services.AddHostedService<StartApplicationService>();

// Aspose Config
AsposeUtility.Install();

//Exception Handling Middleware Error
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseWebSockets();
app.UseSDHttpsRedirection();

app.UseRouting();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<SignalRHub>("/signalrHub");

app.Run();