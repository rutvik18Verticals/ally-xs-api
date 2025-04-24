using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;

namespace Theta.XSPOC.Apex.Api.WellControl
{
    /// <summary>
    /// The custom telemetry initializer for Application Insights.
    /// </summary>
    public class CustomTelemetryInitializer : ITelemetryInitializer
    {

        #region Private Fields

        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTelemetryInitializer"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public CustomTelemetryInitializer(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Initializes the specified telemetry.
        /// </summary>
        /// <param name="telemetry">The telemetry to initialize.</param>
        public void Initialize(ITelemetry telemetry)
        {
            if (_httpContextAccessor?.HttpContext == null)
            {
                return;
            }

            if (telemetry is not RequestTelemetry requestTelemetry)
            {
                return;
            }

            if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue(Common.CustomHeaders.XSPOC_UI_TRACKING_ID,
                    out var value))
            {
                requestTelemetry.Properties[Common.CustomHeaders.XSPOC_UI_TRACKING_ID] = value;
            }

            requestTelemetry.Properties["TraceIdentifier"] = _httpContextAccessor.HttpContext.TraceIdentifier;
        }

    }
}
