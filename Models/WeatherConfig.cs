namespace StormWarning.Models;

public class WeatherConfig
{
    public string Headline { get; set; } = "Weather Unavailable";
    public string Details { get; set; } = "Unable to fetch weather data";
    public string Treatment { get; set; } = "calm";
}
