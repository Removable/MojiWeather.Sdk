using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Sample.Playground.Display;
using MojiWeather.Sdk.Sample.Playground.Locations;

namespace MojiWeather.Sdk.Sample.Playground.Menus;

/// <summary>
/// 空气质量菜单
/// </summary>
public class AirQualityMenu(IMojiWeatherClient client, LocationManager locationManager) : IMenuHandler
{
    public async Task ShowAsync()
    {
        while (true)
        {
            ConsoleHelper.ClearScreen();
            DisplayMenu();

            var input = ConsoleHelper.ReadLine("请输入选项")?.Trim().ToUpperInvariant();

            switch (input)
            {
                case "1":
                    await GetBriefAqiAsync();
                    break;
                case "2":
                    await GetDetailedAqiAsync();
                    break;
                case "3":
                    await GetAqiForecast5DaysAsync();
                    break;
                case "B":
                    return;
                default:
                    ConsoleHelper.WriteWarning("无效选项");
                    ConsoleHelper.WaitForKey();
                    break;
            }
        }
    }

    private void DisplayMenu()
    {
        ConsoleHelper.WriteHeader("空气质量 - Air Quality Service");
        ConsoleHelper.WriteInfo($"位置: {locationManager.GetCurrentLocationDisplay()}");
        Console.WriteLine();

        ConsoleHelper.WriteMenuOption("1", "精简AQI (Brief AQI)");
        ConsoleHelper.WriteMenuOption("2", "详细AQI (Detailed AQI)");
        ConsoleHelper.WriteMenuOption("3", "5天AQI预报 (5-Day AQI Forecast)");

        Console.WriteLine();
        ConsoleHelper.WriteMenuOption("B", "返回主菜单 (Back)");

        ConsoleHelper.WriteSeparator();
    }

    private async Task GetBriefAqiAsync()
    {
        ConsoleHelper.WriteInfo("正在获取精简AQI...");

        try
        {
            var response = await client.AirQuality.GetBriefAqiAsync(locationManager.CurrentLocation);

            ConsoleHelper.ClearScreen();
            ConsoleHelper.WriteHeader("精简AQI - Brief AQI");

            if (response.IsSuccess && response.Data != null)
            {
                WeatherFormatter.DisplayCityInfo(response.Data.City);
                WeatherFormatter.DisplayBriefAqi(response.Data.Aqi);
            }
            else
            {
                ConsoleHelper.WriteError($"获取失败: {response.Message}");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"请求异常: {ex.Message}");
        }

        ConsoleHelper.WaitForKey();
    }

    private async Task GetDetailedAqiAsync()
    {
        ConsoleHelper.WriteInfo("正在获取详细AQI...");

        try
        {
            var response = await client.AirQuality.GetDetailedAqiAsync(locationManager.CurrentLocation);

            ConsoleHelper.ClearScreen();
            ConsoleHelper.WriteHeader("详细AQI - Detailed AQI");

            if (response.IsSuccess && response.Data != null)
            {
                WeatherFormatter.DisplayCityInfo(response.Data.City);
                WeatherFormatter.DisplayDetailedAqi(response.Data.Aqi);
            }
            else
            {
                ConsoleHelper.WriteError($"获取失败: {response.Message}");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"请求异常: {ex.Message}");
        }

        ConsoleHelper.WaitForKey();
    }

    private async Task GetAqiForecast5DaysAsync()
    {
        ConsoleHelper.WriteInfo("正在获取5天AQI预报...");

        try
        {
            var response = await client.AirQuality.GetAqiForecast5DaysAsync(locationManager.CurrentLocation);

            ConsoleHelper.ClearScreen();
            ConsoleHelper.WriteHeader("5天AQI预报 - 5-Day AQI Forecast");

            if (response.IsSuccess && response.Data != null)
            {
                WeatherFormatter.DisplayCityInfo(response.Data.City);
                WeatherFormatter.DisplayAqiForecasts(response.Data.Forecasts);
            }
            else
            {
                ConsoleHelper.WriteError($"获取失败: {response.Message}");
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.WriteError($"请求异常: {ex.Message}");
        }

        ConsoleHelper.WaitForKey();
    }
}
