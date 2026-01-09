using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Sample.Playground.Display;
using MojiWeather.Sdk.Sample.Playground.Locations;

namespace MojiWeather.Sdk.Sample.Playground.Menus;

/// <summary>
/// 限行信息菜单
/// </summary>
public class TrafficMenu(IMojiWeatherClient client, LocationManager locationManager) : IMenuHandler
{
    public async Task ShowAsync()
    {
        ConsoleHelper.ClearScreen();
        ConsoleHelper.WriteHeader("限行信息 - Traffic Restriction");
        ConsoleHelper.WriteInfo($"位置: {locationManager.GetCurrentLocationDisplay()}");
        Console.WriteLine();

        ConsoleHelper.WriteInfo("正在获取限行信息...");

        try
        {
            var response = await client.Traffic.GetRestrictionAsync(locationManager.CurrentLocation);

            ConsoleHelper.ClearScreen();
            ConsoleHelper.WriteHeader("限行信息 - Traffic Restriction");

            if (response.IsSuccess && response.Data != null)
            {
                WeatherFormatter.DisplayCityInfo(response.Data.City);
                WeatherFormatter.DisplayTrafficRestrictions(response.Data.Limit);
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
