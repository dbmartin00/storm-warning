using Newtonsoft.Json;
using Splitio.Services.Client.Classes;
using Splitio.Services.Client.Interfaces;
using StormWarning.Models;

namespace StormWarning;

public class WeatherService : IDisposable
{
    private readonly ISplitClient _splitClient;
    private readonly ISplitFactory _factory;

    public WeatherService()
    {
        var sdkKey = Environment.GetEnvironmentVariable("FF_API_KEY");

        if (string.IsNullOrEmpty(sdkKey))
        {
            throw new InvalidOperationException("FF_API_KEY environment variable is not set");
        }

        var config = new ConfigurationOptions();

        _factory = new SplitFactory(sdkKey, config);
        _splitClient = _factory.Client();
    }

    public async Task InitializeAsync()
    {
        try
        {
            await Task.Run(() => _splitClient.BlockUntilReady(10000));
            Console.WriteLine("[SDK] Harness FME SDK initialized successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Failed to initialize Harness SDK: {ex.Message}");
            throw;
        }
    }

    public async Task<WeatherConfig> GetWeatherAsync()
    {
        var userId = $"user-{Guid.NewGuid()}";

        try
        {
            var result = await Task.Run(() =>
                _splitClient.GetTreatmentWithConfig(userId, "storm_warning"));

            var treatment = result.Treatment;

            if (!string.IsNullOrEmpty(result.Config))
            {
                var config = JsonConvert.DeserializeObject<WeatherConfig>(result.Config);
                if (config != null)
                {
                    return config;
                }
            }

            return new WeatherConfig
            {
                Headline = "Weather Unavailable",
                Details = $"Treatment: {treatment}, no config available"
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ERROR] Failed to evaluate flag: {ex.Message}");
            return new WeatherConfig
            {
                Headline = "Weather Unavailable",
                Details = $"Error: {ex.Message}"
            };
        }
    }

    public void Dispose()
    {
        _splitClient?.Destroy();
    }
}
