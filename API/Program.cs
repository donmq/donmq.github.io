using API.Configurations;
using API.Helpers.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
// Add services to the container.

builder.Services.AddControllers();

// Setting DBContexts
builder.Services.AddDatabaseConfiguration(builder.Configuration);

// Add Authentication
builder.Services.AddAuthenticationConfigufation(builder.Configuration);

// RepositoryAccessor and Service
builder.Services.AddDependencyInjectionConfiguration();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// Swagger Config
builder.Services.AddSwaggerGenConfiguration();

//Exception Handling Middleware Error
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.UseHttpsRedirection();

app.UseRouting();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
