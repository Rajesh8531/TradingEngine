using Application;
using Application.Behaviours;
using Application.Common.Interface;
using Application.Services;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Contracts;
using Infrastructure.Persistence.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using Serilog;
using Serilog.Events;
using TradingEngine.Middlewares;

namespace TradingEngine.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
        }

        public static WebApplicationBuilder AddCustomSerilog(this WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()

                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)

                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information)

                .Enrich.FromLogContext()
                .Enrich.WithProperty("ApplicationName", "TradingEngine")

                .WriteTo.File(path: "logs/app-log-.txt",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj} {Properties:j}{NewLine}{Exception}"
                )
                .CreateLogger();

            builder.Host.UseSerilog();

            return builder;
        }

        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CorrelationIdMiddleware>();
        }

        public static void ConfigureMediatR(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });
        }

        public static void RegisterHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<OutboxRelayService>();
        }

        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();

            services.AddScoped<IBalanceRepository, BalanceRepository>();
        }

        public static void RegisterRabbitMQ(this IServiceCollection services)
        {
            services.AddSingleton(new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
            });

            services.AddSingleton<IEventPublisher, RabbitMQPublisher>();
        }
    }
}
