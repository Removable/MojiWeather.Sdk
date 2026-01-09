using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Sample.Playground.Display;
using MojiWeather.Sdk.Sample.Playground.Locations;

namespace MojiWeather.Sdk.Sample.Playground.Menus;

/// <summary>
/// 天气实况菜单
/// </summary>
public class WeatherMenu(IMojiWeatherClient client, LocationManager locationManager) : IMenuHandler
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
                    await GetBriefConditionAsync();
                    break;
                case "2":
                    await GetDetailedConditionAsync();
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
        ConsoleHelper.WriteHeader("天气实况 - Weather Service");
        ConsoleHelper.WriteInfo($"位置: {locationManager.GetCurrentLocationDisplay()}");
        Console.WriteLine();

        ConsoleHelper.WriteMenuOption("1", "精简实况 (Brief Condition)");
        ConsoleHelper.WriteMenuOption("2", "详细实况 (Detailed Condition)");

        Console.WriteLine();
        ConsoleHelper.WriteMenuOption("B", "返回主菜单 (Back)");

        ConsoleHelper.WriteSeparator();
    }

    private async Task GetBriefConditionAsync()
    {
        ConsoleHelper.WriteInfo("正在获取精简天气实况...");

        try
        {
            var response = await client.Weather.GetBriefConditionAsync(locationManager.CurrentLocation);

            ConsoleHelper.ClearScreen();
            ConsoleHelper.WriteHeader("精简天气实况 - Brief Condition");

            if (response.IsSuccess && response.Data != null)
            {
                WeatherFormatter.DisplayCityInfo(response.Data.City);
                WeatherFormatter.DisplayBriefCondition(response.Data.Condition);
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

    private async Task GetDetailedConditionAsync()
    {
        ConsoleHelper.WriteInfo("正在获取详细天气实况...");

        try
        {
            var response = await client.Weather.GetDetailedConditionAsync(locationManager.CurrentLocation);

            ConsoleHelper.ClearScreen();
            ConsoleHelper.WriteHeader("详细天气实况 - Detailed Condition");

            if (response.IsSuccess && response.Data != null)
            {
                WeatherFormatter.DisplayCityInfo(response.Data.City);
                WeatherFormatter.DisplayDetailedCondition(response.Data.Condition);
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
