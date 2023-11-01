using Azure.Messaging.EventGrid;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TestFunction
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
        }

        [Function(nameof(Function1))]
        public void Run([ServiceBusTrigger("%TestFeeQueue%", Connection = "ServiceBusConnection")] string message)
        {
            try
            {
                var events = EventGridEvent.ParseMany(new BinaryData(message));
                throw new NonCriticalException("Test Exception");
            }
            catch (NonCriticalException ex)
            {
                _logger.LogWarning(ex, $"TestFee: {ex.Message} - Request: {message}");
            }
            catch (TestNotFoundException ex)
            {
                _logger.LogError(ex, $"TestFee: TestNotFoundException: Error occured while processing message - {message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"TestFee: Error occured while processing message - {message}");
                throw;
            }
        }

        [Function("LoggingPOCFunction")]
          public async Task LoggingPOCFunction(
            [TimerTrigger("0 0 10 * * *", RunOnStartup = true)] TimerInfo timer)
        {
            Exception ex = new InvalidOperationException("Test Exception By Nish");
           for (var i = 0; i <= 2000; i++)
            {
                _logger.LogError("Can Be sampled Processor {i}", i);
                //Thread.Sleep(500);
                _logger.LogErrorWithNoSampling(ex, "Should Not be sampled: ex {i}", i);
                //Thread.Sleep(500);
                _logger.LogErrorWithNoSampling("Should Not be sampled {i}", i);

                ++Counter;
            }
    }
}
