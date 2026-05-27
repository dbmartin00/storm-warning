using StormWarning;

var cts = new CancellationTokenSource();

var welcomeTask = ConsoleUI.ShowWelcomeScreenAsync(cts.Token);

await Task.Run(() => Console.ReadLine());
cts.Cancel();

try
{
    await welcomeTask;
}
catch (TaskCanceledException)
{
}

WeatherService? weatherService = null;

try
{
    weatherService = new WeatherService();
    await weatherService.InitializeAsync();

    while (true)
    {
        var weather = await weatherService.GetWeatherAsync();
        ConsoleUI.ShowWeatherDisplay(weather);
        Console.ReadLine();
    }
}
catch (Exception ex)
{
    Console.WriteLine($"\n[FATAL ERROR] {ex.Message}");
    Console.WriteLine("\nPress Enter to exit...");
    Console.ReadLine();
}
finally
{
    weatherService?.Dispose();
}
