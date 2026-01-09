namespace MojiWeather.Sdk.Sample.Playground.Display;

/// <summary>
/// 控制台辅助工具类
/// </summary>
public static class ConsoleHelper
{
    private const int DefaultWidth = 50;

    #region 颜色输出

    /// <summary>
    /// 输出成功信息（绿色）
    /// </summary>
    public static void WriteSuccess(string message)
    {
        WriteColored(message, ConsoleColor.Green);
    }

    /// <summary>
    /// 输出错误信息（红色）
    /// </summary>
    public static void WriteError(string message)
    {
        WriteColored(message, ConsoleColor.Red);
    }

    /// <summary>
    /// 输出警告信息（黄色）
    /// </summary>
    public static void WriteWarning(string message)
    {
        WriteColored(message, ConsoleColor.Yellow);
    }

    /// <summary>
    /// 输出提示信息（青色）
    /// </summary>
    public static void WriteInfo(string message)
    {
        WriteColored(message, ConsoleColor.Cyan);
    }

    /// <summary>
    /// 输出指定颜色的文本
    /// </summary>
    public static void WriteColored(string message, ConsoleColor color)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = originalColor;
    }

    /// <summary>
    /// 输出指定颜色的文本（不换行）
    /// </summary>
    public static void WriteColoredInline(string message, ConsoleColor color)
    {
        var originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.Write(message);
        Console.ForegroundColor = originalColor;
    }

    #endregion

    #region 格式化输出

    /// <summary>
    /// 输出标题
    /// </summary>
    public static void WriteHeader(string title, int width = DefaultWidth)
    {
        WriteSeparator('=', width);
        var padding = (width - title.Length) / 2;
        Console.WriteLine($"{new string(' ', Math.Max(0, padding))}{title}");
        WriteSeparator('=', width);
    }

    /// <summary>
    /// 输出子标题
    /// </summary>
    public static void WriteSubHeader(string title)
    {
        Console.WriteLine();
        WriteColored($"【{title}】", ConsoleColor.White);
    }

    /// <summary>
    /// 输出分隔线
    /// </summary>
    public static void WriteSeparator(char c = '=', int width = DefaultWidth)
    {
        Console.WriteLine(new string(c, width));
    }

    /// <summary>
    /// 输出键值对
    /// </summary>
    public static void WriteKeyValue(string key, string? value, int keyWidth = 16)
    {
        Console.Write($"  {key.PadRight(keyWidth)}: ");
        if (string.IsNullOrEmpty(value))
        {
            WriteColored("N/A", ConsoleColor.DarkGray);
        }
        else
        {
            Console.WriteLine(value);
        }
    }

    /// <summary>
    /// 输出键值对（带颜色的值）
    /// </summary>
    public static void WriteKeyValueColored(string key, string? value, ConsoleColor valueColor, int keyWidth = 16)
    {
        Console.Write($"  {key.PadRight(keyWidth)}: ");
        if (string.IsNullOrEmpty(value))
        {
            WriteColored("N/A", ConsoleColor.DarkGray);
        }
        else
        {
            WriteColored(value, valueColor);
        }
    }

    /// <summary>
    /// 输出菜单选项
    /// </summary>
    public static void WriteMenuOption(string key, string description)
    {
        Console.Write("  [");
        WriteColoredInline(key, ConsoleColor.Yellow);
        Console.WriteLine($"] {description}");
    }

    #endregion

    #region 输入辅助

    /// <summary>
    /// 读取用户输入
    /// </summary>
    public static string? ReadLine(string prompt)
    {
        Console.Write($"{prompt}: ");
        return Console.ReadLine();
    }

    /// <summary>
    /// 读取坐标输入
    /// </summary>
    public static (double lat, double lon)? ReadCoordinates()
    {
        Console.Write("请输入纬度 (Latitude): ");
        var latInput = Console.ReadLine();
        if (!double.TryParse(latInput, out var lat) || lat < -90 || lat > 90)
        {
            WriteError("无效的纬度值，纬度范围为 -90 到 90");
            return null;
        }

        Console.Write("请输入经度 (Longitude): ");
        var lonInput = Console.ReadLine();
        if (!double.TryParse(lonInput, out var lon) || lon < -180 || lon > 180)
        {
            WriteError("无效的经度值，经度范围为 -180 到 180");
            return null;
        }

        return (lat, lon);
    }

    /// <summary>
    /// 等待用户按键
    /// </summary>
    public static void WaitForKey(string message = "按任意键继续...")
    {
        Console.WriteLine();
        WriteColored(message, ConsoleColor.DarkGray);
        Console.ReadKey(true);
    }

    /// <summary>
    /// 清屏
    /// </summary>
    public static void ClearScreen()
    {
        Console.Clear();
    }

    #endregion

    #region AQI 辅助

    /// <summary>
    /// 获取AQI等级描述
    /// </summary>
    public static string GetAqiLevel(int value) => value switch
    {
        <= 50 => "优",
        <= 100 => "良",
        <= 150 => "轻度污染",
        <= 200 => "中度污染",
        <= 300 => "重度污染",
        _ => "严重污染"
    };

    /// <summary>
    /// 获取AQI对应的颜色
    /// </summary>
    public static ConsoleColor GetAqiColor(int value) => value switch
    {
        <= 50 => ConsoleColor.Green,
        <= 100 => ConsoleColor.Yellow,
        <= 150 => ConsoleColor.DarkYellow,
        <= 200 => ConsoleColor.Red,
        <= 300 => ConsoleColor.Magenta,
        _ => ConsoleColor.DarkMagenta
    };

    #endregion
}
