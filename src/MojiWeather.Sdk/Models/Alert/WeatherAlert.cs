using System.Globalization;
using System.Text.Json.Serialization;
using MojiWeather.Sdk.Models.Common;

namespace MojiWeather.Sdk.Models.Alert;

/// <summary>
/// 天气预警数据
/// </summary>
public sealed record WeatherAlertData
{
    /// <summary>
    /// 城市信息
    /// </summary>
    [JsonPropertyName("city")]
    public CityInfo? City { get; init; }

    /// <summary>
    /// 预警列表
    /// <para>晴天等无预警天气时为空</para>
    /// </summary>
    [JsonPropertyName("alert")]
    public IReadOnlyList<WeatherAlert>? Alerts { get; init; }
}

/// <summary>
/// 天气预警
/// </summary>
public sealed record WeatherAlert
{
    /// <summary>
    /// 防御指南映射表
    /// </summary>
    private static readonly IReadOnlyDictionary<int, string> _defenseGuideMap = new Dictionary<int, string>
    {
        [0] = "无",
        [1] = "室内活动",
        [2] = "多做运动",
        [3] = "准备药品",
        [4] = "准备药品",
        [5] = "注意防晒",
        [6] = "防暑降温",
        [7] = "防寒保暖",
        [8] = "节约用水",
        [9] = "小心坠物",
        [10] = "关闭门窗",
        [11] = "加固门窗",
        [12] = "切断电源",
        [13] = "注意防火",
        [14] = "不用手机",
        [15] = "不戴耳机",
        [16] = "注意防护",
        [17] = "携带雨具",
        [18] = "小心驾驶",
        [19] = "关注路况",
        [20] = "公交出行",
        [21] = "地铁出行",
        [22] = "远离广告牌",
        [23] = "远离树木",
        [24] = "注意防滑",
        [25] = "减少污水排放",
        [26] = "不乱扔烟头",
        [27] = "轮渡停航",
        [28] = "船舶回港",
        [29] = "船舶固锚",
        [30] = "多喝热水",
        [31] = "佩戴口罩",
        [32] = "减少出行",
        [33] = "补充水分",
        [34] = "小心航行"
    };

    /// <summary>
    /// 预警内容
    /// </summary>
    [JsonPropertyName("content")]
    public string? Content { get; init; }

    /// <summary>
    /// 预警ID(提供了预警图标供参考,ID对应预警图标编号)
    /// </summary>
    [JsonPropertyName("infoid")]
    public int? InfoId { get; init; }

    /// <summary>
    /// 预警等级
    /// </summary>
    [JsonPropertyName("level")]
    public string? Level { get; init; }

    /// <summary>
    /// 预警名称
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// 发布时间 (格式: yyyy-MM-dd HH:mm:ss)
    /// </summary>
    [JsonPropertyName("pub_time")]
    public string? PublishTimeStr { get; init; }

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime? PublishTime => DateTime.TryParseExact(PublishTimeStr, "yyyy-MM-dd HH:mm:ss",
        CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt)
        ? dt
        : null;

    /// <summary>
    /// 预警标题
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; init; }

    /// <summary>
    /// 更新时间 (格式: yyyy-MM-dd HH:mm:ss)
    /// </summary>
    [JsonPropertyName("update_time")]
    public string? UpdateTimeStr { get; init; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdateTime => DateTime.TryParseExact(UpdateTimeStr, "yyyy-MM-dd HH:mm:ss",
        CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt)
        ? dt
        : null;

    /// <summary>
    /// 预警类型
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; init; }

    /// <summary>
    /// 陆地防御指南ID序号
    /// <para>可选字段,若API不返回可忽略</para>
    /// <para>ID对应防御措施: 1-室内活动 2-多做运动 3-准备药品 5-注意防晒 6-防暑降温 7-防寒保暖 8-节约用水 9-小心坠物 10-关闭门窗 11-加固门窗 12-切断电源 13-注意防火 14-不用手机 15-不戴耳机 16-注意防护 17-携带雨具 18-小心驾驶 19-关注路况 20-公交出行 21-地铁出行 22-远离广告牌 23-远离树木 24-注意防滑 25-减少污水排放 26-不乱扔烟头 30-多喝热水 31-佩戴口罩 32-减少出行 33-补充水分 0-无</para>
    /// </summary>
    [JsonPropertyName("land_defense_id")]
    public string? LandDefenseId { get; init; }

    /// <summary>
    /// 陆地防御指南描述
    /// <para>根据 LandDefenseId 自动转换为对应的防御措施描述</para>
    /// </summary>
    [JsonPropertyName("land_defense_desc")]
    public string[]? LandDefenseDescription => ConvertDefenseIds(LandDefenseId);

    /// <summary>
    /// 港口防御指南ID序号
    /// <para>可选字段,若API不返回可忽略</para>
    /// <para>ID对应防御措施: 27-轮渡停航 28-船舶回港 29-船舶固锚 34-小心航行 0-无</para>
    /// </summary>
    [JsonPropertyName("port_defense_id")]
    public string? PortDefenseId { get; init; }

    /// <summary>
    /// 港口防御指南描述
    /// <para>根据 PortDefenseId 自动转换为对应的防御措施描述</para>
    /// </summary>
    [JsonPropertyName("port_defense_desc")]
    public string[]? PortDefenseDescription => ConvertDefenseIds(PortDefenseId);

    /// <summary>
    /// 将防御指南ID字符串转换为描述数组
    /// </summary>
    /// <param name="defenseIds">逗号分隔的防御指南ID字符串,例如: "1,2,3"</param>
    /// <returns>防御指南描述数组,若输入为空则返回null</returns>
    private static string[]? ConvertDefenseIds(string? defenseIds)
    {
        if (string.IsNullOrWhiteSpace(defenseIds))
        {
            return null;
        }

        var ids = defenseIds.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var descriptions = new List<string>(ids.Length);

        foreach (var idStr in ids)
        {
            if (int.TryParse(idStr, out var id) && _defenseGuideMap.TryGetValue(id, out var description))
            {
                descriptions.Add(description);
            }
        }

        return descriptions.Count > 0 ? descriptions.ToArray() : null;
    }
}
