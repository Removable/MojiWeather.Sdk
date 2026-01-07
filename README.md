# MojiWeather.Sdk

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Build](https://img.shields.io/badge/build-passing-brightgreen.svg)]()

墨迹天气 API 的官方 .NET SDK，用于访问阿里云 API 市场的墨迹天气服务。

[English](#english) | [中文](#中文)

---

## 中文

### 功能特性

- 🌤️ **天气实况** - 获取精简/详细天气实况
- 📅 **天气预报** - 支持 3/6/15 天预报和 24 小时预报
- 🌫️ **空气质量** - AQI 指数、污染物数据、5 天预报
- ⚠️ **天气预警** - 实时天气预警信息
- 🏃 **生活指数** - 穿衣、运动、洗车等生活指数
- 🚗 **限行数据** - 城市车辆限行信息

### 安装

```bash
dotnet add package MojiWeather.Sdk
```

### 快速开始

#### 1. 注册服务

```csharp
using MojiWeather.Sdk.Extensions;

var builder = Host.CreateApplicationBuilder(args);

// 方式 1: 使用配置文件
builder.Services.AddMojiWeather(builder.Configuration);

// 方式 2: 使用委托配置
builder.Services.AddMojiWeather(options =>
{
    options.AppCode = "your-appcode-from-aliyun";
    options.Tier = SubscriptionTier.Professional;
});
```

#### 2. 配置文件 (appsettings.json)

```json
{
  "MojiWeather": {
    "AppCode": "your-appcode-from-aliyun",
    "Tier": "Professional",
    "UseHttps": true,
    "Timeout": "00:00:30",
    "Retry": {
      "MaxRetries": 3,
      "InitialDelay": "00:00:00.500",
      "BackoffMultiplier": 2.0
    }
  }
}
```

#### 3. 使用 SDK

```csharp
using MojiWeather.Sdk.Abstractions;

public class WeatherController
{
    private readonly IMojiWeatherClient _client;

    public WeatherController(IMojiWeatherClient client)
    {
        _client = client;
    }

    public async Task<string> GetWeather(double lat, double lon)
    {
        // 通过经纬度查询
        var location = LocationQuery.FromCoordinates(lat, lon);

        // 获取天气实况
        var result = await _client.Weather.GetBriefConditionAsync(location);

        if (result.IsSuccess && result.Data?.Condition != null)
        {
            var c = result.Data.Condition;
            return $"{c.ConditionDescription}, {c.Temperature}°C";
        }

        return $"Error: {result.Message}";
    }
}
```

### API 参考

#### 位置查询

```csharp
// 经纬度查询
var location = LocationQuery.FromCoordinates(39.9042, 116.4074);

// 城市ID查询
var location = LocationQuery.FromCityId(101010100);
```

#### 可用服务

| 服务 | 接口 | 描述 |
|------|------|------|
| Weather | `IWeatherService` | 天气实况 |
| Forecast | `IForecastService` | 天气预报 |
| AirQuality | `IAirQualityService` | 空气质量 |
| Alert | `IAlertService` | 天气预警 |
| LiveIndex | `ILiveIndexService` | 生活指数 |
| Traffic | `ITrafficService` | 限行数据 |

#### 订阅级别

> 订阅级别相互独立，仅可访问对应级别的接口。

| 级别 | 枚举值 | 可用功能 |
|------|--------|----------|
| 试用版 | `SubscriptionTier.Trial` | 精简实况、精简AQI、3天预报 |
| PM2.5版 | `SubscriptionTier.Pm25` | + 详细AQI、6天预报、预警、限行 |
| 专业版 | `SubscriptionTier.Professional` | + 详细实况、15天预报、24小时、生活指数 |
| 基础版 | `SubscriptionTier.Basic` | 详细实况（城市ID） |

### 高级配置

#### 自定义 Token（可选）

如需覆盖默认的 API Token，可通过配置文件：

```json
{
  "MojiWeather": {
    "AppCode": "your-appcode",
    "Tokens": {
      "Coordinates": {
        "BriefCondition": "custom-token-if-needed"
      }
    }
  }
}
```

#### 重试策略

SDK 内置了智能重试机制：
- 指数退避重试
- 自动处理瞬时故障
- 可配置最大重试次数

### 错误处理

```csharp
try
{
    var result = await client.Weather.GetBriefConditionAsync(location);

    if (!result.IsSuccess)
    {
        // API 返回错误
        Console.WriteLine($"API Error: {result.Code} - {result.Message}");
    }
}
catch (AuthenticationException ex)
{
    // AppCode 无效或权限不足
    Console.WriteLine($"Auth Error: {ex.Message}");
}
catch (ApiException ex)
{
    // HTTP 请求或解析错误
    Console.WriteLine($"API Exception: {ex.Message}, Status: {ex.StatusCode}");
}
```

### 示例项目

查看 `samples/MojiWeather.Sdk.Sample` 目录获取完整示例。

---

## English

### Features

- 🌤️ **Current Weather** - Brief and detailed weather conditions
- 📅 **Weather Forecast** - 3/6/15 day forecasts and 24-hour forecasts
- 🌫️ **Air Quality** - AQI index, pollutant data, 5-day forecast
- ⚠️ **Weather Alerts** - Real-time weather warnings
- 🏃 **Living Index** - Clothing, sports, car washing indices
- 🚗 **Traffic Restrictions** - City vehicle restriction information

### Installation

```bash
dotnet add package MojiWeather.Sdk
```

### Quick Start

#### 1. Register Services

```csharp
using MojiWeather.Sdk.Extensions;

var builder = Host.CreateApplicationBuilder(args);

// Option 1: From configuration
builder.Services.AddMojiWeather(builder.Configuration);

// Option 2: Using delegate
builder.Services.AddMojiWeather(options =>
{
    options.AppCode = "your-appcode-from-aliyun";
    options.Tier = SubscriptionTier.Professional;
});
```

#### 2. Configuration File (appsettings.json)

```json
{
  "MojiWeather": {
    "AppCode": "your-appcode-from-aliyun",
    "Tier": "Professional",
    "UseHttps": true,
    "Timeout": "00:00:30"
  }
}
```

#### 3. Using the SDK

```csharp
using MojiWeather.Sdk.Abstractions;

public class WeatherService
{
    private readonly IMojiWeatherClient _client;

    public WeatherService(IMojiWeatherClient client)
    {
        _client = client;
    }

    public async Task<string> GetWeatherAsync(double lat, double lon)
    {
        var location = LocationQuery.FromCoordinates(lat, lon);
        var result = await _client.Weather.GetBriefConditionAsync(location);

        return result.IsSuccess
            ? $"{result.Data?.Condition?.ConditionDescription}, {result.Data?.Condition?.Temperature}°C"
            : $"Error: {result.Message}";
    }
}
```

### Requirements

- .NET 10.0 or later
- Alibaba Cloud API Marketplace account with Moji Weather subscription
- Subscription tiers are independent; access is limited to the exact tier.

### License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

### Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Links

- [阿里云 API 市场 - 墨迹天气](https://market.aliyun.com/products/57126001/cmapi010812.html)
- [墨迹天气 API 文档](https://market.aliyun.com/products/57126001/cmapi010812.html#sku=yuncode481200000)
