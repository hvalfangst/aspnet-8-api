using Api.Configs;
using Api.Repositories;
using Api.Services;
using Api.Utils;

namespace Api;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.Configure<DatabaseConfig>(builder.Configuration.GetSection("DatabaseConfig"));
        builder.Services.AddScoped<IHeroRepository, HeroRepository>();
        builder.Services.AddScoped<DataContext>();
        builder.Services.AddScoped<HeroService>();
        builder.Services.AddSingleton<ILoggerFactory, ApiLoggerFactory>();
        
        var app = builder.Build();
        app.MapControllers();
        app.Run();
    }
}