using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Sample.Playground.Display;
using MojiWeather.Sdk.Sample.Playground.Locations;

namespace MojiWeather.Sdk.Sample.Playground.Menus;

/// <summary>
/// 天气预报菜单
/// </summary>
public class ForecastMenu(IMojiWeatherClient client, LocationManager locationManager) : IMenuHandler
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
                    await GetBriefForecast3DaysAsync();
                    break;
                case "2":
                    await GetBriefForecast6DaysAsync();
                    break;
                case "3":
                    await GetForecast15DaysAsync();
                    break;
                case "4":
                    await GetForecast24HoursAsync();
                    break;
                case "5":
                    await GetShortForecastAsync();
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
        ConsoleHelper.WriteHeader("天气预报 - Forecast Service");
        ConsoleHelper.WriteInfo($"位置: {locationManager.GetCurrentLocationDisplay()}");
        Console.WriteLine();

        ConsoleHelper.WriteMenuOption("1", "3天精简预报 (3-Day Brief)");
        ConsoleHelper.WriteMenuOption("2", "6天精简预报 (6-Day Brief)");
        ConsoleHelper.WriteMenuOption("3", "15天预报 (15-Day)");
        ConsoleHelper.WriteMenuOption("4", "24小时预报 (24-Hour)");
        ConsoleHelper.WriteMenuOption("5", "短时预报 (Short-term)");

        Console.WriteLine();
        ConsoleHelper.WriteMenuOption("B", "返回主菜单 (Back)");

        ConsoleHelper.WriteSeparator();
    }

    private async Task GetBriefForecast3DaysAsync()
    {
        ConsoleHelper.WriteInfo("正在获取3天精简预报...");

        try
        {
            var response = await client.Forecast.GetBriefForecast3DaysAsync(locationManager.CurrentLocation);

            ConsoleHelper.ClearScreen();
            ConsoleHelper.WriteHeader("3天精简预报 - 3-Day Brief Forecast");

            if (response.IsSuccess && response.Data != null)
            {
                WeatherFormatter.DisplayCityInfo(response.Data.City);
                WeatherFormatter.DisplayBriefDailyForecasts(response.Data.Forecasts);
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

    private async Task GetBriefForecast6DaysAsync()
    {
        ConsoleHelper.WriteInfo("正在获取6天精简预报...");

        try
        {
            var response = await client.Forecast.GetBriefForecast6DaysAsync(locationManager.CurrentLocation);

            ConsoleHelper.ClearScreen();
            ConsoleHelper.WriteHeader("6天精简预报 - 6-Day Brief Forecast");

            if (response.IsSuccess && response.Data != null)
            {
                WeatherFormatter.DisplayCityInfo(response.Data.City);
                WeatherFormatter.DisplayBriefDailyForecasts(response.Data.Forecasts);
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

    private async Task GetForecast15DaysAsync()
    {
        ConsoleHelper.WriteInfo("正在获取15天预报...");

        try
        {
            var response = await client.Forecast.GetForecast15DaysAsync(locationManager.CurrentLocation);

            ConsoleHelper.ClearScreen();
            ConsoleHelper.WriteHeader("15天预报 - 15-Day Forecast");

            if (response.IsSuccess && response.Data != null)
            {
                WeatherFormatter.DisplayCityInfo(response.Data.City);
                WeatherFormatter.DisplayDailyForecasts(response.Data.Forecasts);
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

    private async Task GetForecast24HoursAsync()
    {
        ConsoleHelper.WriteInfo("正在获取24小时预报...");

        try
        {
            var response = await client.Forecast.GetForecast24HoursAsync(locationManager.CurrentLocation);

            ConsoleHelper.ClearScreen();
            ConsoleHelper.WriteHeader("24小时预报 - 24-Hour Forecast");

            if (response.IsSuccess && response.Data != null)
            {
                WeatherFormatter.DisplayCityInfo(response.Data.City);
                WeatherFormatter.DisplayHourlyForecasts(response.Data.Forecasts);
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

    private async Task GetShortForecastAsync()
    {
        ConsoleHelper.WriteInfo("正在获取短时预报...");

        try
        {
            var response = await client.Forecast.GetShortForecastAsync(locationManager.CurrentLocation);

            ConsoleHelper.ClearScreen();
            ConsoleHelper.WriteHeader("短时预报 - Short-term Forecast");

            if (response.IsSuccess && response.Data != null)
            {
                WeatherFormatter.DisplayCityInfo(response.Data.City);
                WeatherFormatter.DisplayShortForecast(response.Data.ShortForecast);
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
