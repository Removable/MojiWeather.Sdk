using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Extensions;

// 创建主机
var builder = Host.CreateApplicationBuilder(args);

// 配置墨迹天气SDK
builder.Services.AddMojiWeather(options =>
{
    options.AppCode = builder.Configuration["MojiWeather:AppCode"]
        ?? throw new InvalidOperationException("AppCode is required. Set it in appsettings.json or environment variable.");
    options.Tier = SubscriptionTier.Professional;
    options.UseHttps = true;
});

var host = builder.Build();

// 获取客户端
var weatherClient = host.Services.GetRequiredService<IMojiWeatherClient>();

// 示例：北京坐标
var beijing = LocationQuery.FromCoordinates(39.9042, 116.4074);

Console.WriteLine("=== 墨迹天气 SDK 示例 ===\n");

// 获取精简实况
Console.WriteLine("正在获取北京天气实况...\n");
var condition = await weatherClient.Weather.GetBriefConditionAsync(beijing);

if (condition.IsSuccess && condition.Data?.Condition is not null)
{
    var c = condition.Data.Condition;
    Console.WriteLine($"城市: {condition.Data.City?.City}");
    Console.WriteLine($"天气: {c.ConditionDescription}");
    Console.WriteLine($"温度: {c.Temperature}°C");
    Console.WriteLine($"体感温度: {c.RealFeel}°C");
    Console.WriteLine($"湿度: {c.Humidity}%");
    Console.WriteLine($"风向: {c.WindDirection}");
    Console.WriteLine($"风力: {c.WindLevel}级");
    Console.WriteLine($"能见度: {c.Visibility}米");
    Console.WriteLine($"气压: {c.Pressure}hPa");
    Console.WriteLine($"更新时间: {c.UpdateTime}");
    Console.WriteLine($"提示: {c.Tips}");
}
else
{
    Console.WriteLine($"获取失败: {condition.Message}");
}

Console.WriteLine("\n---\n");

// 获取3天预报
Console.WriteLine("正在获取3天天气预报...\n");
var forecast = await weatherClient.Forecast.GetForecast3DaysAsync(beijing);

if (forecast.IsSuccess && forecast.Data?.Forecasts is not null)
{
    foreach (var day in forecast.Data.Forecasts)
    {
        Console.WriteLine($"{day.PredictDate}: {day.ConditionDay}/{day.ConditionNight}, " +
                          $"温度 {day.TempLow}°C ~ {day.TempHigh}°C, " +
                          $"风向 {day.WindDirDay}, 日出 {day.SunRise}, 日落 {day.SunSet}");
    }
}
else
{
    Console.WriteLine($"获取失败: {forecast.Message}");
}

Console.WriteLine("\n---\n");

// 获取精简AQI
Console.WriteLine("正在获取空气质量...\n");
var aqi = await weatherClient.AirQuality.GetBriefAqiAsync(beijing);

if (aqi.IsSuccess && aqi.Data?.Aqi is not null)
{
    var a = aqi.Data.Aqi;
    Console.WriteLine($"AQI: {a.Value} ({a.Level})");
    Console.WriteLine($"主要污染物: {a.PrimaryPollutant}");
    Console.WriteLine($"更新时间: {a.PublishTime}");
}
else
{
    Console.WriteLine($"获取失败: {aqi.Message}");
}

Console.WriteLine("\n=== 示例结束 ===");
