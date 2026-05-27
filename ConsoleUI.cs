using Spectre.Console;
using StormWarning.Models;

namespace StormWarning;

public static class ConsoleUI
{
    private static int _cloudOffset = 0;
    private static int _snowOffset = 0;

    public static async Task ShowWelcomeScreenAsync(CancellationToken cancellationToken)
    {
        var animationTask = Task.Run(async () =>
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                AnsiConsole.Clear();
                RenderWelcomeScreen();

                _cloudOffset = (_cloudOffset + 2) % 60;
                _snowOffset = (_snowOffset + 1) % 40;

                await Task.Delay(5000, cancellationToken);
            }
        }, cancellationToken);

        try
        {
            await animationTask;
        }
        catch (TaskCanceledException)
        {
        }
    }

    private static void RenderWelcomeScreen()
    {
        var panel = new Panel(
            new Markup("[bold cyan]Welcome to Storm Warning[/]\n\n" +
                      "Press [green]Enter[/] to continue...")
        )
        {
            Border = BoxBorder.Rounded,
            Padding = new Padding(2, 1),
            Expand = false
        };

        panel.Header = new PanelHeader("[bold yellow]Storm Warning App[/]", Justify.Center);

        var layout = new Layout()
            .SplitRows(
                new Layout("spacer-top").Size(3),
                new Layout("content").Size(7),
                new Layout("spacer-bottom")
            );

        layout["spacer-top"].Update(new Markup(GetClouds()));

        layout["content"].Update(
            Align.Center(panel, VerticalAlignment.Middle)
        );

        layout["spacer-bottom"].Update(new Markup(GetSnow()));

        AnsiConsole.Write(layout);
    }

    private static string GetClouds()
    {
        var clouds = new[]
        {
            "     .-~~~-.        .-~~~-.       .-~~~-.     ",
            " .- ~ ~-(     )_ _  .- ~ ~-(   )_ .- ~ ~-()_  ",
            "/            ~ -. /          ~ -./        ~ -."
        };

        var width = Console.WindowWidth;
        var lines = new string[3];

        for (int i = 0; i < 3; i++)
        {
            var cloudLine = clouds[i];
            var repeats = (width / cloudLine.Length) + 2;
            var fullLine = string.Concat(Enumerable.Repeat(cloudLine, repeats));

            var startPos = _cloudOffset % cloudLine.Length;
            lines[i] = fullLine.Substring(startPos, Math.Min(width, fullLine.Length - startPos));

            if (lines[i].Length < width)
            {
                lines[i] += fullLine.Substring(0, width - lines[i].Length);
            }
        }

        return $"[cyan]{lines[0]}[/]\n[cyan]{lines[1]}[/]\n[cyan]{lines[2]}[/]";
    }

    private static string GetSnow()
    {
        var width = Console.WindowWidth;
        var snowPattern = " *  * *   *    *  *   * *    ";
        var repeats = (width / snowPattern.Length) + 2;
        var fullSnow = string.Concat(Enumerable.Repeat(snowPattern, repeats));

        var startPos = _snowOffset % snowPattern.Length;
        var snowLine = fullSnow.Substring(startPos, Math.Min(width, fullSnow.Length - startPos));

        if (snowLine.Length < width)
        {
            snowLine += fullSnow.Substring(0, width - snowLine.Length);
        }

        return $"[white]{snowLine}[/]";
    }

    public static void ShowWeatherDisplay(WeatherConfig weather)
    {
        AnsiConsole.Clear();

        var mainLayout = new Layout()
            .SplitRows(
                new Layout("title").Size(3),
                new Layout("content"),
                new Layout("footer").Size(3)
            );

        var titlePanel = new Panel(
            new Markup("[bold cyan]Weather Dashboard[/]")
        )
        {
            Border = BoxBorder.None,
            Padding = new Padding(0)
        };

        mainLayout["title"].Update(Align.Center(titlePanel));

        var weatherPanel = new Panel(
            new Markup($"[bold yellow]{weather.Headline}[/]\n\n{weather.Details}")
        )
        {
            Border = BoxBorder.Rounded,
            Padding = new Padding(2, 1),
            Expand = true
        };

        mainLayout["content"].Update(
            Align.Center(weatherPanel, VerticalAlignment.Middle)
        );

        var footerLayout = new Layout()
            .SplitColumns(
                new Layout("left"),
                new Layout("right")
            );

        footerLayout["left"].Update(
            new Markup("[dim]Press [green]Enter[/] to refresh...[/]")
        );

        footerLayout["right"].Update(
            Align.Right(
                new Markup("[dim]Harness FME - storm_warning[/]"),
                VerticalAlignment.Bottom
            )
        );

        mainLayout["footer"].Update(footerLayout);

        AnsiConsole.Write(mainLayout);
    }
}
