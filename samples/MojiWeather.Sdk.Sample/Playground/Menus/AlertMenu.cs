using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Sample.Playground.Display;
using MojiWeather.Sdk.Sample.Playground.Locations;

namespace MojiWeather.Sdk.Sample.Playground.Menus;

/// <summary>
/// 天气预警菜单
/// </summary>
public class AlertMenu(IMojiWeatherClient client, LocationManager locationManager) : IMenuHandler
{
    public async Task ShowAsync()
    {
        ConsoleHelper.ClearScreen();
        ConsoleHelper.WriteHeader("天气预警 - Weather Alerts");
        ConsoleHelper.WriteInfo($"位置: {locationManager.GetCurrentLocationDisplay()}");
        Console.WriteLine();

        ConsoleHelper.WriteInfo("正在获取天气预警...");

        try
        {
            var response = await client.Alert.GetActiveAlertsAsync(locationManager.CurrentLocation);

            ConsoleHelper.ClearScreen();
            ConsoleHelper.WriteHeader("天气预警 - Weather Alerts");

            if (response.IsSuccess && response.Data != null)
            {
                WeatherFormatter.DisplayCityInfo(response.Data.City);
                WeatherFormatter.DisplayAlerts(response.Data.Alerts);
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
