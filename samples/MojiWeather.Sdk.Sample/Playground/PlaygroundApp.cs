using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Sample.Playground.Display;
using MojiWeather.Sdk.Sample.Playground.Locations;
using MojiWeather.Sdk.Sample.Playground.Menus;

namespace MojiWeather.Sdk.Sample.Playground;

/// <summary>
/// Playground 主应用
/// </summary>
public class PlaygroundApp
{
    private readonly MainMenu _mainMenu;

    public PlaygroundApp(IMojiWeatherClient client)
    {
        var locationManager = new LocationManager();
        _mainMenu = new MainMenu(client, locationManager);
    }

    /// <summary>
    /// 运行应用
    /// </summary>
    public async Task RunAsync()
    {
        // 显示欢迎信息
        ConsoleHelper.ClearScreen();
        ConsoleHelper.WriteHeader("欢迎使用墨迹天气 SDK Playground");
        ConsoleHelper.WriteInfo("按任意键开始...");
        Console.ReadKey(true);

        // 主循环
        while (true)
        {
            try
            {
                var shouldExit = await _mainMenu.ShowAsync();
                if (shouldExit)
                {
                    break;
                }
            }
            catch (Exception ex)
            {
                ConsoleHelper.WriteError($"发生错误: {ex.Message}");
                ConsoleHelper.WaitForKey();
            }
        }

        // 退出信息
        ConsoleHelper.ClearScreen();
        ConsoleHelper.WriteSuccess("感谢使用墨迹天气 SDK Playground!");
        ConsoleHelper.WriteInfo("Goodbye!");
    }
}
