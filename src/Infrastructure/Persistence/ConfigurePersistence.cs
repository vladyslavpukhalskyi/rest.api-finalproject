using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Infrastructure.Persistence;

public static class ConfigurePersistence
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        // Створення підключення до PostgreSQL
        var dataSourceBuild = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("Default"));
        dataSourceBuild.EnableDynamicJson();
        var dataSource = dataSourceBuild.Build();

        // Налаштування ApplicationDbContext для роботи з PostgreSQL
        services.AddDbContext<ApplicationDbContext>(
            options => options
                .UseNpgsql(
                    dataSource,
                    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                .UseSnakeCaseNamingConvention() // Для автоматичної конвертації назв таблиць та стовпців у snake_case
                .ConfigureWarnings(w => w.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning))); // Ігнорування попереджень

        // Додавання ініціалізатора контексту бази даних
        services.AddScoped<ApplicationDbContextInitialiser>();

        // Додавання репозиторіїв
        services.AddRepositories();
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        // Додавання репозиторіїв для акторів, режисерів, фільмів і жанрів
        services.AddScoped<ActorRepository>();
        services.AddScoped<IActorRepository>(provider => provider.GetRequiredService<ActorRepository>());
        services.AddScoped<IActorQueries>(provider => provider.GetRequiredService<ActorRepository>());

        services.AddScoped<DirectorRepository>();
        services.AddScoped<IDirectorRepository>(provider => provider.GetRequiredService<DirectorRepository>());
        services.AddScoped<IDirectorQueries>(provider => provider.GetRequiredService<DirectorRepository>());

        services.AddScoped<MovieRepository>();
        services.AddScoped<IMovieRepository>(provider => provider.GetRequiredService<MovieRepository>());
        services.AddScoped<IMovieQueries>(provider => provider.GetRequiredService<MovieRepository>());

        services.AddScoped<GenreRepository>();
        services.AddScoped<IGenreRepository>(provider => provider.GetRequiredService<GenreRepository>());
        services.AddScoped<IGenreQueries>(provider => provider.GetRequiredService<GenreRepository>());
    }
}
