using MojiWeather.Sdk.Models.AirQuality;
using MojiWeather.Sdk.Models.Alert;
using MojiWeather.Sdk.Models.Common;
using MojiWeather.Sdk.Models.LiveIndex;
using MojiWeather.Sdk.Models.Traffic;
using MojiWeather.Sdk.Models.Weather;

namespace MojiWeather.Sdk.Sample.Playground.Display;

/// <summary>
/// 天气数据格式化显示器
/// </summary>
public static class WeatherFormatter
{
    #region 城市信息

    /// <summary>
    /// 显示城市信息
    /// </summary>
    public static void DisplayCityInfo(CityInfo? city)
    {
        if (city == null)
        {
            ConsoleHelper.WriteWarning("无城市信息");
            return;
        }

        ConsoleHelper.WriteSubHeader("城市信息");
        ConsoleHelper.WriteKeyValue("城市", city.Name);
        ConsoleHelper.WriteKeyValue("省份", city.ProvinceName);
        ConsoleHelper.WriteKeyValue("国家", city.CountryName);
        ConsoleHelper.WriteKeyValue("城市ID", city.CityId.ToString());
        if (!string.IsNullOrEmpty(city.IanaTimezone))
            ConsoleHelper.WriteKeyValue("时区", city.IanaTimezone);
    }

    #endregion

    #region 天气实况

    /// <summary>
    /// 显示精简天气实况
    /// </summary>
    public static void DisplayBriefCondition(BriefCondition? condition)
    {
        if (condition == null)
        {
            ConsoleHelper.WriteWarning("无天气实况数据");
            return;
        }

        ConsoleHelper.WriteSubHeader("天气实况");
        ConsoleHelper.WriteKeyValue("天气", condition.Condition);
        ConsoleHelper.WriteKeyValue("温度", FormatTemperature(condition.Temperature));
        ConsoleHelper.WriteKeyValue("湿度", FormatPercent(condition.Humidity));
        ConsoleHelper.WriteKeyValue("风向", condition.WindDirection);
        ConsoleHelper.WriteKeyValue("风级", FormatWindLevel(condition.WindLevel));
        ConsoleHelper.WriteKeyValue("能见度", FormatVisibility(condition.Visibility));
        ConsoleHelper.WriteKeyValue("更新时间", condition.UpdateTime);
    }

    /// <summary>
    /// 显示详细天气实况
    /// </summary>
    public static void DisplayDetailedCondition(DetailedCondition? condition)
    {
        if (condition == null)
        {
            ConsoleHelper.WriteWarning("无天气实况数据");
            return;
        }

        ConsoleHelper.WriteSubHeader("天气实况");
        ConsoleHelper.WriteKeyValue("天气", condition.ConditionDescription);
        ConsoleHelper.WriteKeyValue("温度", FormatTemperature(condition.Temperature));
        ConsoleHelper.WriteKeyValue("体感温度", FormatTemperature(condition.RealFeel));
        ConsoleHelper.WriteKeyValue("湿度", FormatPercent(condition.Humidity));
        ConsoleHelper.WriteKeyValue("气压", FormatPressure(condition.Pressure));
        ConsoleHelper.WriteKeyValue("风向", condition.WindDirection);
        ConsoleHelper.WriteKeyValue("风级", FormatWindLevel(condition.WindLevel));
        ConsoleHelper.WriteKeyValue("风速", FormatWindSpeed(condition.WindSpeed));
        ConsoleHelper.WriteKeyValue("能见度", FormatVisibility(condition.Visibility));
        ConsoleHelper.WriteKeyValue("紫外线", condition.UvIndex);

        ConsoleHelper.WriteSubHeader("日出日落");
        ConsoleHelper.WriteKeyValue("日出", condition.SunRise);
        ConsoleHelper.WriteKeyValue("日落", condition.SunSet);

        if (!string.IsNullOrEmpty(condition.Tips))
        {
            ConsoleHelper.WriteSubHeader("温馨提示");
            ConsoleHelper.WriteInfo($"  {condition.Tips}");
        }

        ConsoleHelper.WriteKeyValue("更新时间", condition.UpdateTime);
    }

    #endregion

    #region 天气预报

    /// <summary>
    /// 显示精简日预报列表
    /// </summary>
    public static void DisplayBriefDailyForecasts(IReadOnlyList<BriefDailyForecast>? forecasts)
    {
        if (forecasts == null || forecasts.Count == 0)
        {
            ConsoleHelper.WriteWarning("无预报数据");
            return;
        }

        ConsoleHelper.WriteSubHeader("天气预报");
        Console.WriteLine();

        // 表头
        Console.WriteLine("  {0,-12} {1,-8} {2,-8} {3,-12} {4,-6} {5,-8}",
            "日期", "白天", "夜间", "温度", "湿度", "风向");
        ConsoleHelper.WriteSeparator('-', 60);

        foreach (var f in forecasts)
        {
            var tempRange = $"{f.TempDay}°C/{f.TempNight}°C";
            Console.WriteLine("  {0,-12} {1,-8} {2,-8} {3,-12} {4,-6} {5,-8}",
                f.PredictDate ?? "N/A",
                f.ConditionDay ?? "N/A",
                f.ConditionNight ?? "N/A",
                tempRange,
                FormatPercent(f.Humidity),
                f.WindDirDay ?? "N/A");
        }
    }

    /// <summary>
    /// 显示15天预报列表
    /// </summary>
    public static void DisplayDailyForecasts(IReadOnlyList<DailyForecast>? forecasts)
    {
        if (forecasts == null || forecasts.Count == 0)
        {
            ConsoleHelper.WriteWarning("无预报数据");
            return;
        }

        ConsoleHelper.WriteSubHeader("天气预报");
        Console.WriteLine();

        // 表头
        Console.WriteLine("  {0,-12} {1,-8} {2,-8} {3,-12} {4,-6} {5,-8} {6,-8}",
            "日期", "白天", "夜间", "温度", "湿度", "风向", "降水");
        ConsoleHelper.WriteSeparator('-', 70);

        foreach (var f in forecasts)
        {
            var tempRange = $"{f.TempDay}°C/{f.TempNight}°C";
            Console.WriteLine("  {0,-12} {1,-8} {2,-8} {3,-12} {4,-6} {5,-8} {6,-8}",
                f.PredictDate ?? "N/A",
                f.ConditionDay ?? "N/A",
                f.ConditionNight ?? "N/A",
                tempRange,
                FormatPercent(f.Humidity),
                f.WindDirDay ?? "N/A",
                FormatPercent(f.Pop));
        }
    }

    /// <summary>
    /// 显示24小时预报列表
    /// </summary>
    public static void DisplayHourlyForecasts(IReadOnlyList<HourlyForecast>? forecasts)
    {
        if (forecasts == null || forecasts.Count == 0)
        {
            ConsoleHelper.WriteWarning("无预报数据");
            return;
        }

        ConsoleHelper.WriteSubHeader("24小时预报");
        Console.WriteLine();

        // 表头
        Console.WriteLine("  {0,-6} {1,-8} {2,-8} {3,-6} {4,-6} {5,-8}",
            "时间", "天气", "温度", "湿度", "风向", "风级");
        ConsoleHelper.WriteSeparator('-', 50);

        foreach (var f in forecasts)
        {
            Console.WriteLine("  {0,-6} {1,-8} {2,-8} {3,-6} {4,-6} {5,-8}",
                $"{f.Hour}时",
                f.Condition ?? "N/A",
                FormatTemperature(f.Temp),
                FormatPercent(f.Humidity),
                f.WindDir ?? "N/A",
                FormatWindLevel(f.WindLevel));
        }
    }

    /// <summary>
    /// 显示短时预报
    /// </summary>
    public static void DisplayShortForecast(ShortForecast? forecast)
    {
        if (forecast == null)
        {
            ConsoleHelper.WriteWarning("无短时预报数据");
            return;
        }

        ConsoleHelper.WriteSubHeader("短时预报");

        if (!string.IsNullOrEmpty(forecast.Banner))
        {
            ConsoleHelper.WriteInfo($"  {forecast.Banner}");
        }

        if (!string.IsNullOrEmpty(forecast.Notice))
        {
            Console.WriteLine($"  {forecast.Notice}");
        }

        var rainStatus = forecast.Rain == 1 ? "有降水" : "无降水";
        ConsoleHelper.WriteKeyValue("当前状态", rainStatus);

        if (forecast.Rain == 1 && forecast.RainLastTime > 0)
        {
            ConsoleHelper.WriteKeyValue("降水持续", $"{forecast.RainLastTime}分钟");
        }

        if (!string.IsNullOrEmpty(forecast.NearRain))
        {
            ConsoleHelper.WriteKeyValue("附近降雨", forecast.NearRain);
        }

        ConsoleHelper.WriteKeyValue("更新时间", forecast.UpdateTime.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

        // 显示降水预报详情
        if (forecast.Percent.Count > 0)
        {
            ConsoleHelper.WriteSubHeader("未来2小时降水预报");
            var index = 0;
            foreach (var p in forecast.Percent.Take(12)) // 每5分钟一个数据点，显示1小时
            {
                var time = index * 5;
                var level = GetRainLevel(p.Percent);
                Console.WriteLine($"  {time,3}分钟后: {level}");
                index++;
            }
        }
    }

    private static string GetRainLevel(double percent) => percent switch
    {
        < 0.063 => "无雨",
        < 0.33 => "小雨",
        < 0.66 => "中雨",
        _ => "大雨"
    };

    #endregion

    #region 空气质量

    /// <summary>
    /// 显示精简AQI
    /// </summary>
    public static void DisplayBriefAqi(BriefAqi? aqi)
    {
        if (aqi == null)
        {
            ConsoleHelper.WriteWarning("无空气质量数据");
            return;
        }

        ConsoleHelper.WriteSubHeader("空气质量");

        if (int.TryParse(aqi.Value, out var aqiValue))
        {
            var level = ConsoleHelper.GetAqiLevel(aqiValue);
            var color = ConsoleHelper.GetAqiColor(aqiValue);
            Console.Write("  AQI值: ");
            ConsoleHelper.WriteColored($"{aqiValue} [{level}]", color);
        }
        else
        {
            ConsoleHelper.WriteKeyValue("AQI值", aqi.Value);
        }

        ConsoleHelper.WriteKeyValue("城市", aqi.CityName);
        ConsoleHelper.WriteKeyValue("更新时间", aqi.PublishTime?.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A");
    }

    /// <summary>
    /// 显示详细AQI
    /// </summary>
    public static void DisplayDetailedAqi(DetailedAqi? aqi)
    {
        if (aqi == null)
        {
            ConsoleHelper.WriteWarning("无空气质量数据");
            return;
        }

        ConsoleHelper.WriteSubHeader("空气质量");

        var level = ConsoleHelper.GetAqiLevel(int.Parse(aqi.Value));
        var color = ConsoleHelper.GetAqiColor(int.Parse(aqi.Value));
        Console.Write("  AQI值: ");
        ConsoleHelper.WriteColored($"{aqi.Value} [{level}]", color);

        ConsoleHelper.WriteKeyValue("全国排名", aqi.Rank);

        ConsoleHelper.WriteSubHeader("污染物浓度");
        ConsoleHelper.WriteKeyValue("PM2.5", $"{aqi.Pm25Concentration} μg/m³");
        ConsoleHelper.WriteKeyValue("PM10", $"{aqi.Pm10Concentration} μg/m³");
        ConsoleHelper.WriteKeyValue("CO", $"{aqi.CoConcentration} mg/m³");
        ConsoleHelper.WriteKeyValue("NO2", $"{aqi.No2Concentration} μg/m³");
        ConsoleHelper.WriteKeyValue("O3", $"{aqi.O3Concentration} μg/m³");
        ConsoleHelper.WriteKeyValue("SO2", $"{aqi.So2Concentration} μg/m³");

        ConsoleHelper.WriteKeyValue("更新时间", aqi.PublishTime?.LocalDateTime.ToString("yyyy-MM-dd HH:mm:ss") ?? "N/A");
    }

    /// <summary>
    /// 显示AQI预报
    /// </summary>
    public static void DisplayAqiForecasts(IReadOnlyList<AqiForecast>? forecasts)
    {
        if (forecasts == null || forecasts.Count == 0)
        {
            ConsoleHelper.WriteWarning("无AQI预报数据");
            return;
        }

        ConsoleHelper.WriteSubHeader("AQI预报");
        Console.WriteLine();

        foreach (var f in forecasts)
        {
            var level = ConsoleHelper.GetAqiLevel(f.Value);
            var color = ConsoleHelper.GetAqiColor(f.Value);
            Console.Write($"  {f.Date:yyyy-MM-dd}  AQI: ");
            ConsoleHelper.WriteColored($"{f.Value,3} [{level}]", color);
        }
    }

    #endregion

    #region 天气预警

    /// <summary>
    /// 显示天气预警
    /// </summary>
    public static void DisplayAlerts(IReadOnlyList<WeatherAlert>? alerts)
    {
        if (alerts == null || alerts.Count == 0)
        {
            ConsoleHelper.WriteSuccess("当前无预警信息");
            return;
        }

        ConsoleHelper.WriteSubHeader($"天气预警 ({alerts.Count}条)");

        foreach (var alert in alerts)
        {
            Console.WriteLine();
            var levelColor = GetAlertLevelColor(alert.Level);
            ConsoleHelper.WriteColored($"  【{alert.Level}】{alert.Name}", levelColor);

            if (!string.IsNullOrEmpty(alert.Title))
            {
                Console.WriteLine($"  标题: {alert.Title}");
            }

            if (!string.IsNullOrEmpty(alert.Content))
            {
                Console.WriteLine($"  内容: {alert.Content}");
            }

            if (alert.LandDefenseDescription?.Length > 0)
            {
                Console.WriteLine($"  陆地防御: {string.Join("、", alert.LandDefenseDescription)}");
            }

            if (alert.PortDefenseDescription?.Length > 0)
            {
                Console.WriteLine($"  港口防御: {string.Join("、", alert.PortDefenseDescription)}");
            }

            if (alert.PublishTime.HasValue)
            {
                Console.WriteLine($"  发布时间: {alert.PublishTime:yyyy-MM-dd HH:mm:ss}");
            }

            ConsoleHelper.WriteSeparator('-', 40);
        }
    }

    private static ConsoleColor GetAlertLevelColor(string? level) => level switch
    {
        "红色" => ConsoleColor.Red,
        "橙色" => ConsoleColor.DarkYellow,
        "黄色" => ConsoleColor.Yellow,
        "蓝色" => ConsoleColor.Blue,
        _ => ConsoleColor.White
    };

    #endregion

    #region 生活指数

    /// <summary>
    /// 显示生活指数
    /// </summary>
    public static void DisplayLiveIndexes(IReadOnlyDictionary<string, IReadOnlyList<LiveIndex>>? indexesByDate)
    {
        if (indexesByDate == null || indexesByDate.Count == 0)
        {
            ConsoleHelper.WriteWarning("无生活指数数据");
            return;
        }

        ConsoleHelper.WriteSubHeader("生活指数");
        Console.WriteLine();

        foreach (var (date, indexes) in indexesByDate)
        {
            ConsoleHelper.WriteSubHeader($"日期: {date}");

            foreach (var index in indexes)
            {
                Console.Write($"  {index.Name,-12}: ");
                ConsoleHelper.WriteColoredInline($"{index.Status}", GetIndexColor(index.Level));
                Console.WriteLine($" ({index.Level})");

                if (!string.IsNullOrEmpty(index.Description))
                {
                    ConsoleHelper.WriteColored($"    {index.Description}", ConsoleColor.DarkGray);
                }
            }

            Console.WriteLine();
        }
    }

    private static ConsoleColor GetIndexColor(int level) => level switch
    {
        1 => ConsoleColor.Green,
        2 => ConsoleColor.Cyan,
        3 => ConsoleColor.Yellow,
        4 or 5 or 6 or 7 => ConsoleColor.DarkYellow,
        >= 8 => ConsoleColor.Red,
        _ => ConsoleColor.White
    };

    #endregion

    #region 限行信息

    /// <summary>
    /// 显示限行信息
    /// </summary>
    public static void DisplayTrafficRestrictions(IReadOnlyList<TrafficRestriction>? restrictions)
    {
        if (restrictions == null || restrictions.Count == 0)
        {
            ConsoleHelper.WriteWarning("无限行信息");
            return;
        }

        ConsoleHelper.WriteSubHeader("限行信息");
        Console.WriteLine();

        foreach (var r in restrictions)
        {
            var dateStr = r.Date?.ToString("yyyy-MM-dd (ddd)") ?? "N/A";
            var color = r.Prompt == "W" ? ConsoleColor.Green : ConsoleColor.Yellow;

            Console.Write($"  {dateStr}: ");
            ConsoleHelper.WriteColored(r.PromptDescription ?? "未知", color);
        }
    }

    #endregion

    #region 格式化辅助方法

    private static string FormatTemperature(string? temp)
    {
        return string.IsNullOrEmpty(temp) ? "N/A" : $"{temp}°C";
    }

    private static string FormatPercent(string? value)
    {
        return string.IsNullOrEmpty(value) ? "N/A" : $"{value}%";
    }

    private static string FormatVisibility(string? vis)
    {
        if (string.IsNullOrEmpty(vis))
            return "N/A";
        if (int.TryParse(vis, out var m))
        {
            return m >= 1000 ? $"{m / 1000.0:F1}km" : $"{m}m";
        }
        return $"{vis}m";
    }

    private static string FormatPressure(string? pressure)
    {
        return string.IsNullOrEmpty(pressure) ? "N/A" : $"{pressure}hPa";
    }

    private static string FormatWindLevel(string? level)
    {
        return string.IsNullOrEmpty(level) ? "N/A" : $"{level}级";
    }

    private static string FormatWindSpeed(string? speed)
    {
        return string.IsNullOrEmpty(speed) ? "N/A" : $"{speed}m/s";
    }

    #endregion
}
