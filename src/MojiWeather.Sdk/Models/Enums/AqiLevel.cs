namespace MojiWeather.Sdk.Models.Enums;

/// <summary>
/// AQI空气质量等级
/// </summary>
public enum AqiLevel
{
    /// <summary>
    /// 优 (0-50)
    /// </summary>
    Excellent = 1,

    /// <summary>
    /// 良 (51-100)
    /// </summary>
    Good = 2,

    /// <summary>
    /// 轻度污染 (101-150)
    /// </summary>
    LightPollution = 3,

    /// <summary>
    /// 中度污染 (151-200)
    /// </summary>
    ModeratePollution = 4,

    /// <summary>
    /// 重度污染 (201-300)
    /// </summary>
    HeavyPollution = 5,

    /// <summary>
    /// 严重污染 (301-500)
    /// </summary>
    SeverePollution = 6,

    /// <summary>
    /// 爆表 (>500)
    /// </summary>
    Hazardous = 7
}

/// <summary>
/// AQI等级扩展方法
/// </summary>
public static class AqiLevelExtensions
{
    /// <summary>
    /// 根据AQI值获取等级
    /// </summary>
    public static AqiLevel FromValue(int aqi) => aqi switch
    {
        <= 50 => AqiLevel.Excellent,
        <= 100 => AqiLevel.Good,
        <= 150 => AqiLevel.LightPollution,
        <= 200 => AqiLevel.ModeratePollution,
        <= 300 => AqiLevel.HeavyPollution,
        <= 500 => AqiLevel.SeverePollution,
        _ => AqiLevel.Hazardous
    };
}
