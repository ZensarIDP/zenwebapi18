using Microsoft.EntityFrameworkCore;
using ZenHotelManagement.Contracts;
using ZenHotelManagement.LoggerService;
using ZenHotelManagement.Repository;
using ZenHotelManagement.Service;
using ZenHotelManagement.Service.Contracts;

namespace ZenHotelManagement.WebApi.Extensions
{
    public static  class ServiceExtensions
    {

        public static void ConfigureCors(this IServiceCollection services) =>
            services.AddCors(
                options =>
                {
                    options.AddPolicy("CorsPolicy", builder =>
                            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                }
            );

        public static void ConfigureIISIntegration(this IServiceCollection services) =>
             services.Configure<IISOptions>(
                     options =>
                     {

                     }
             );

        public static void ConfigureLoggerService(this IServiceCollection services) =>
        //here it tails that where ever in project there is ILogger is found then create an object of LoggerManaager class and inject it.
          services.AddSingleton<ILoggerManager, LoggerManager>();

        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            // Build MySQL connection string from environment variables
            var dbHost = Environment.GetEnvironmentVariable("MYSQL_HOST");
            var dbPort = Environment.GetEnvironmentVariable("MYSQL_PORT");
            var dbName = Environment.GetEnvironmentVariable("MYSQL_DATABASE");
            var dbUser = Environment.GetEnvironmentVariable("MYSQL_USERNAME");
            var dbPass = Environment.GetEnvironmentVariable("MYSQL_PASSWORD");

            // If environment variables are not set, fall back to appsettings
            var connectionString = !string.IsNullOrEmpty(dbHost) 
                ? $"server={dbHost};port={dbPort};database={dbName};user={dbUser};password={dbPass};AllowPublicKeyRetrieval=true;SslMode=Required;"
                : configuration.GetConnectionString("ZenHotelConnection");

            services.AddDbContext<RepositoryContext>(
                options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
            );
        }

        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
           services.AddScoped<IRepositoryManager, RepositoryManager>();

        public static void ConfigureServiceManager(this IServiceCollection services) =>
           services.AddScoped<IServiceManager, ServiceManager>();
    }
}
