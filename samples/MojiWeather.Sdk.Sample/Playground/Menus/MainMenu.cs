using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Sample.Playground.Display;
using MojiWeather.Sdk.Sample.Playground.Locations;

namespace MojiWeather.Sdk.Sample.Playground.Menus;

/// <summary>
/// 主菜单
/// </summary>
public class MainMenu(IMojiWeatherClient client, LocationManager locationManager)
{
    private readonly WeatherMenu _weatherMenu = new(client, locationManager);
    private readonly ForecastMenu _forecastMenu = new(client, locationManager);
    private readonly AirQualityMenu _airQualityMenu = new(client, locationManager);
    private readonly AlertMenu _alertMenu = new(client, locationManager);
    private readonly LiveIndexMenu _liveIndexMenu = new(client, locationManager);
    private readonly TrafficMenu _trafficMenu = new(client, locationManager);

    /// <summary>
    /// 显示主菜单
    /// </summary>
    /// <returns>返回 true 表示退出应用</returns>
    public async Task<bool> ShowAsync()
    {
        ConsoleHelper.ClearScreen();
        DisplayMenu();

        var input = ConsoleHelper.ReadLine("请输入选项")?.Trim().ToUpperInvariant();

        return input switch
        {
            "1" => await HandleSubMenuAsync(_weatherMenu),
            "2" => await HandleSubMenuAsync(_forecastMenu),
            "3" => await HandleSubMenuAsync(_airQualityMenu),
            "4" => await HandleSubMenuAsync(_alertMenu),
            "5" => await HandleSubMenuAsync(_liveIndexMenu),
            "6" => await HandleSubMenuAsync(_trafficMenu),
            "L" => HandleLocationChange(),
            "Q" => true,
            _ => HandleInvalidInput()
        };
    }

    private void DisplayMenu()
    {
        ConsoleHelper.WriteHeader("墨迹天气 SDK Playground");
        ConsoleHelper.WriteInfo($"当前位置: {locationManager.GetCurrentLocationDisplay()}");
        Console.WriteLine();

        ConsoleHelper.WriteMenuOption("1", "天气实况 (Weather)");
        ConsoleHelper.WriteMenuOption("2", "天气预报 (Forecast)");
        ConsoleHelper.WriteMenuOption("3", "空气质量 (Air Quality)");
        ConsoleHelper.WriteMenuOption("4", "天气预警 (Alerts)");
        ConsoleHelper.WriteMenuOption("5", "生活指数 (Live Index)");
        ConsoleHelper.WriteMenuOption("6", "限行信息 (Traffic)");

        Console.WriteLine();
        ConsoleHelper.WriteMenuOption("L", "更换位置 (Change Location)");
        ConsoleHelper.WriteMenuOption("Q", "退出 (Exit)");

        ConsoleHelper.WriteSeparator();
    }

    private async Task<bool> HandleSubMenuAsync(IMenuHandler menu)
    {
        await menu.ShowAsync();
        return false;
    }

    private bool HandleLocationChange()
    {
        ShowLocationMenu();
        return false;
    }

    private void ShowLocationMenu()
    {
        ConsoleHelper.ClearScreen();
        ConsoleHelper.WriteHeader("选择位置 - Location Selection");

        Console.WriteLine("预设位置:");
        for (var i = 0; i < LocationManager.Presets.Count; i++)
        {
            var preset = LocationManager.Presets[i];
            ConsoleHelper.WriteMenuOption((i + 1).ToString(), preset.FullDisplay);
        }

        Console.WriteLine();
        ConsoleHelper.WriteMenuOption("C", "自定义坐标 (Custom Coordinates)");
        ConsoleHelper.WriteMenuOption("B", "返回主菜单 (Back)");

        ConsoleHelper.WriteSeparator();

        var input = ConsoleHelper.ReadLine("请输入选项")?.Trim().ToUpperInvariant();

        if (input == "B")
            return;

        if (input == "C")
        {
            HandleCustomCoordinates();
            return;
        }

        if (int.TryParse(input, out var index) && index >= 1 && index <= LocationManager.Presets.Count)
        {
            if (locationManager.SelectPreset(index - 1))
            {
                ConsoleHelper.WriteSuccess($"已切换到: {locationManager.GetCurrentLocationDisplay()}");
            }
            else
            {
                ConsoleHelper.WriteError("选择失败");
            }
        }
        else
        {
            ConsoleHelper.WriteWarning("无效选项");
        }

        ConsoleHelper.WaitForKey();
    }

    private void HandleCustomCoordinates()
    {
        Console.WriteLine();
        var coords = ConsoleHelper.ReadCoordinates();
        if (coords.HasValue)
        {
            if (locationManager.SetCustomCoordinates(coords.Value.lat, coords.Value.lon))
            {
                ConsoleHelper.WriteSuccess($"已设置自定义位置: {locationManager.GetCurrentLocationDisplay()}");
            }
            else
            {
                ConsoleHelper.WriteError("坐标设置失败");
            }
        }
        ConsoleHelper.WaitForKey();
    }

    private static bool HandleInvalidInput()
    {
        ConsoleHelper.WriteWarning("无效选项，请重新输入");
        ConsoleHelper.WaitForKey();
        return false;
    }
}

/// <summary>
/// 菜单处理接口
/// </summary>
public interface IMenuHandler
{
    /// <summary>
    /// 显示菜单
    /// </summary>
    Task ShowAsync();
}
