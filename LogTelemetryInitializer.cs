using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

public class LogTelemetryInitializer : ITelemetryInitializer
{
    public void Initialize(ITelemetry telemetry)
    {
        if (telemetry is ISupportProperties propItem && propItem.Properties.ContainsKey("NoSample") && propItem.Properties["NoSample"] == "True")
        {
            if (telemetry is TraceTelemetry)
            {
                var trace = telemetry as TraceTelemetry;
                trace.ProactiveSamplingDecision = SamplingDecision.SampledOut;
                ((ISupportSampling)trace).SamplingPercentage = 100;
            }
            else if (telemetry is ExceptionTelemetry)
            {
                var trace = telemetry as ExceptionTelemetry;
                trace.ProactiveSamplingDecision = SamplingDecision.SampledOut;
                ((ISupportSampling)trace).SamplingPercentage = 100;
            }
        }
    }
}
