using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Chess.Web.Services;

namespace Chess.Web
{
    public static class StartupExtensions
    {
        public static IServiceCollection UseInMemoryDb(this IServiceCollection services)
        {
            return services.AddSingleton<Services.IGameStore, Services.InMemoryGameStore>(); // singleton for in-memory store :-)
        }

        public static IServiceCollection UseMongoDb(this IServiceCollection services)
        {
            services.AddSingleton<MongoClient>(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var mongoConnectionString = configuration["mongodbConnectionString"];
                return new MongoClient(mongoConnectionString);
            });
            return services.AddTransient<Services.IGameStore, Services.MongoDBGameStore>();
        }

        public static IApplicationBuilder ConfigureAppInsights(this IApplicationBuilder app, IConfiguration configuration)
        {
            var telemetryConfig = Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active;
            telemetryConfig.TelemetryInitializers.Add(new BuildNumberTelemetryInitializer(configuration));

            var telemetryClient = new Microsoft.ApplicationInsights.TelemetryClient();
            telemetryClient.TrackEvent("InstanceStart");

            return app;
        }
    }
}
