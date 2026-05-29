# Storm Warning Console Application

A .NET 8.0 console application that displays weather conditions from a Harness FME feature flag.

## Features

- Animated welcome screen with ASCII art clouds and snow
- Clouds and snow drift across the screen every 5 seconds
- Weather conditions display from Harness FME feature flag `storm_warning`
- Dynamic ASCII art visualizations for different weather conditions
- Interactive refresh on Enter key press
- Random user targeting for each evaluation
- Graceful error handling with fallback defaults

## Prerequisites

- .NET 8.0 SDK
- Harness Feature Flags account with access to the `storm_warning` flag
- Harness Server SDK API key

## Setup

1. Set your Harness API key as an environment variable:
   ```bash
   export FF_API_KEY="your-api-key-here"
   ```

2. Build the application:
   ```bash
   dotnet build
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

## Feature Flag Configuration

The `storm_warning` flag should be configured in Harness with JSON variations containing:

```json
{
  "headline": "Weather Condition Title",
  "details": "Detailed weather description",
  "weatherType": "calm"
}
```

### Example Treatments

- **calm**: `{"headline": "Clear Skies", "details": "Perfect weather conditions", "weatherType": "calm"}`
- **fog**: `{"headline": "Foggy Conditions", "details": "Reduced visibility expected", "weatherType": "fog"}`
- **rain**: `{"headline": "Rainy Day", "details": "Bring an umbrella", "weatherType": "rain"}`
- **sleet**: `{"headline": "Sleet Warning", "details": "Icy precipitation possible", "weatherType": "sleet"}`
- **snow**: `{"headline": "Snow Alert", "details": "Winter weather advisory", "weatherType": "snow"}`
- **stormy**: `{"headline": "Severe Storm Warning", "details": "Thunderstorms and lightning expected", "weatherType": "stormy"}`
- **sunny**: `{"headline": "Sunny Day", "details": "Beautiful clear skies ahead", "weatherType": "sunny"}`
- **cloudy**: `{"headline": "Overcast", "details": "Mostly cloudy throughout the day", "weatherType": "cloudy"}`
- **windy**: `{"headline": "High Winds", "details": "Gusts up to 40mph expected", "weatherType": "windy"}`

### Supported Weather Types

The application includes ASCII art visualizations for the following weather types:
- `calm` - Peaceful lighthouse scene
- `rain` - Heavy rainfall pattern
- `fog` - Low visibility fog cloud
- `snow` - Winter snowflake tree pattern
- `sleet` - Mixed precipitation (uses stormy visualization)
- `stormy` - Thunderstorm with lightning bolts
- `sunny` - Bright sun with rays
- `cloudy` - Overcast cloud layers
- `windy` - Wind pattern visualization

If `weatherType` is omitted or unrecognized, the application defaults to "calm".

## Usage

1. Launch the application
2. View the welcome screen and press Enter
3. View the current weather conditions
4. Press Enter to refresh and fetch new weather data
5. Each refresh generates a new random user identifier for flag evaluation

## Dependencies

- `Splitio-net-core` (v6.2.3) - Split.io SDK (used by Harness FME)
- `Spectre.Console` (v0.55.2) - Rich console UI framework
- `Newtonsoft.Json` (v13.0.4) - JSON handling

## Architecture

- **Program.cs** - Application entry point and main loop
- **WeatherService.cs** - Split.io SDK integration and flag evaluation using `GetTreatmentWithConfig`
- **ConsoleUI.cs** - Spectre.Console UI rendering
- **Models/WeatherConfig.cs** - Weather data model

## Technical Details

The application uses the Split.io .NET SDK which powers Harness Feature Management & Experimentation. The SDK:
- Initializes with `BlockUntilReady(10000)` for a 10-second timeout
- Evaluates feature flags using `GetTreatmentWithConfig()` which returns both treatment name and configuration JSON
- Uses CDN-based evaluation (default configuration)
- Generates a random user ID for each evaluation to test different targeting rules
