using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Configuration;

namespace Chess.Web.Services
{
    public class BuildNumberTelemetryInitializer : ITelemetryInitializer
    {
        private readonly IConfiguration _configuration;
        public BuildNumberTelemetryInitializer (IConfiguration configuration)
        {
          _configuration = configuration;
        }

        public void Initialize(ITelemetry telemetry)
        {
            var buildNumber = _configuration["buildNumber"];
            telemetry.Context.Properties["BuildNumber"] = buildNumber;
        }
        
    }
}
