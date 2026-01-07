using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Options;
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
    /// <param name="services">服务集合</param>
    extension(IServiceCollection services)
    {
        /// <summary>
        /// 添加墨迹天气SDK服务
        /// </summary>
        /// <param name="configure">配置委托</param>
        /// <returns>服务集合</returns>
        public IServiceCollection AddMojiWeather(Action<MojiWeatherOptions> configure)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configure);

            services.Configure(configure);
            return services.AddMojiWeatherCore();
        }

        /// <summary>
        /// 从配置文件添加墨迹天气SDK服务
        /// </summary>
        /// <param name="configuration">配置</param>
        /// <returns>服务集合</returns>
        public IServiceCollection AddMojiWeather(IConfiguration configuration)
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
        /// <param name="configurationSection">配置节</param>
        /// <returns>服务集合</returns>
        public IServiceCollection AddMojiWeather(IConfigurationSection configurationSection)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configurationSection);

            services.Configure<MojiWeatherOptions>(configurationSection);
            return services.AddMojiWeatherCore();
        }

        private IServiceCollection AddMojiWeatherCore()
        {
            // 注册配置验证
            services.AddSingleton<IValidateOptions<MojiWeatherOptions>, MojiWeatherOptionsValidator>();

            // 注册HttpClient并配置重试策略
            services.AddHttpClient<IMojiHttpClient, MojiHttpClient>((sp, client) =>
                {
                    var options = sp.GetRequiredService<IOptions<MojiWeatherOptions>>().Value;
                    client.Timeout = options.Timeout;
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                })
                .AddStandardResilienceHandler()
                .Configure((options, sp) =>
                {
                    var mojiOptions = sp.GetRequiredService<IOptions<MojiWeatherOptions>>().Value;
                    options.Retry = BuildRetryStrategyOptions(mojiOptions.Retry);
                });

            // 注册端点提供者
            services.AddSingleton<IEndpointProvider, EndpointProvider>();

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

    internal static HttpRetryStrategyOptions BuildRetryStrategyOptions(RetryOptions retryOptions)
    {
        var useCustomBackoff = retryOptions.BackoffMultiplier > 1.0 &&
                               !IsDefaultBackoffMultiplier(retryOptions.BackoffMultiplier);

        var strategyOptions = new HttpRetryStrategyOptions
        {
            MaxRetryAttempts = retryOptions.MaxRetries,
            Delay = retryOptions.InitialDelay,
            BackoffType = retryOptions.BackoffMultiplier <= 1.0
                ? DelayBackoffType.Constant
                : DelayBackoffType.Exponential,
            UseJitter = !useCustomBackoff,
            ShouldHandle = args => ValueTask.FromResult(
                args.Outcome.Exception is not null ||
                (args.Outcome.Result?.StatusCode is >= System.Net.HttpStatusCode.InternalServerError
                    or System.Net.HttpStatusCode.RequestTimeout
                    or System.Net.HttpStatusCode.TooManyRequests))
        };

        if (useCustomBackoff)
        {
            strategyOptions.DelayGenerator = args =>
            {
                var attempt = Math.Max(args.AttemptNumber, 0);
                var delayMilliseconds = retryOptions.InitialDelay.TotalMilliseconds *
                                        Math.Pow(retryOptions.BackoffMultiplier, attempt);
                return ValueTask.FromResult<TimeSpan?>(TimeSpan.FromMilliseconds(delayMilliseconds));
            };
        }

        return strategyOptions;
    }

    private static bool IsDefaultBackoffMultiplier(double backoffMultiplier)
        => Math.Abs(backoffMultiplier - 2.0) < 0.000001;
}
