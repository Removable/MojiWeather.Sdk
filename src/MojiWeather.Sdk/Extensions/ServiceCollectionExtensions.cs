using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Http;
using MojiWeather.Sdk.Services;
using Polly;

namespace MojiWeather.Sdk.Extensions;

/// <summary>
/// 服务集合扩展方法
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加墨迹天气SDK服务
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configure">配置委托</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddMojiWeather(
        this IServiceCollection services,
        Action<MojiWeatherOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        services.Configure(configure);
        return services.AddMojiWeatherCore();
    }

    /// <summary>
    /// 从配置文件添加墨迹天气SDK服务
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddMojiWeather(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.Configure<MojiWeatherOptions>(
            configuration.GetSection(MojiWeatherOptions.SectionName));

        return services.AddMojiWeatherCore();
    }

    /// <summary>
    /// 从配置节添加墨迹天气SDK服务
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configurationSection">配置节</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddMojiWeather(
        this IServiceCollection services,
        IConfigurationSection configurationSection)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configurationSection);

        services.Configure<MojiWeatherOptions>(configurationSection);
        return services.AddMojiWeatherCore();
    }

    private static IServiceCollection AddMojiWeatherCore(this IServiceCollection services)
    {
        // 注册HttpClient并配置重试策略
        services.AddHttpClient<IMojiHttpClient, MojiHttpClient>((sp, client) =>
            {
                var options = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<MojiWeatherOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
                client.Timeout = options.Timeout;
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddStandardResilienceHandler(options =>
            {
                options.Retry = new HttpRetryStrategyOptions
                {
                    MaxRetryAttempts = 3,
                    Delay = TimeSpan.FromMilliseconds(500),
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true,
                    ShouldHandle = args => ValueTask.FromResult(
                        args.Outcome.Exception is not null ||
                        (args.Outcome.Result?.StatusCode is >= System.Net.HttpStatusCode.InternalServerError
                            or System.Net.HttpStatusCode.RequestTimeout
                            or System.Net.HttpStatusCode.TooManyRequests))
                };
            });

        // 注册服务
        services.AddSingleton<IWeatherService, WeatherService>();
        services.AddSingleton<IForecastService, ForecastService>();
        services.AddSingleton<IAirQualityService, AirQualityService>();
        services.AddSingleton<IAlertService, AlertService>();
        services.AddSingleton<ILiveIndexService, LiveIndexService>();
        services.AddSingleton<ITrafficService, TrafficService>();

        // 注册主客户端
        services.AddSingleton<IMojiWeatherClient, MojiWeatherClient>();

        return services;
    }
}
