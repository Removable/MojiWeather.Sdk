using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Options;
using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Extensions;
using MojiWeather.Sdk.Http;
using MojiWeather.Sdk.Services;
using Polly;
using Xunit;

namespace MojiWeather.Sdk.Tests.Integration;

public class ServiceRegistrationTests
{
    [Fact]
    public void AddMojiWeather_ShouldRegisterClientAndOptions()
    {
        // 准备
        var services = new ServiceCollection();

        // 执行
        services.AddMojiWeather(options =>
        {
            options.AppCode = "test-appcode-12345";
        });

        using var provider = services.BuildServiceProvider();
        var client = provider.GetRequiredService<IMojiWeatherClient>();
        var options = provider.GetRequiredService<IOptions<MojiWeatherOptions>>().Value;

        // 断言
        client.Should().NotBeNull();
        options.AppCode.Should().Be("test-appcode-12345");
    }

    [Fact]
    public void AddMojiWeather_WithInvalidOptions_ShouldThrowValidationException()
    {
        // 准备
        using var provider = BuildProvider(options =>
        {
            options.AppCode = "short";
        });

        // 执行
        var action = () => provider.GetRequiredService<IOptions<MojiWeatherOptions>>().Value;

        // 断言
        action.Should().Throw<OptionsValidationException>();
    }

    [Fact]
    public void AddMojiWeather_ShouldRegisterAllRequiredServices()
    {
        // 准备
        using var provider = BuildProvider(options =>
        {
            options.AppCode = "test-appcode-12345";
        });

        // 执行
        var client = provider.GetRequiredService<IMojiWeatherClient>();

        // 断言
        provider.GetRequiredService<IWeatherService>().Should().NotBeNull();
        provider.GetRequiredService<IForecastService>().Should().NotBeNull();
        provider.GetRequiredService<IAirQualityService>().Should().NotBeNull();
        provider.GetRequiredService<IAlertService>().Should().NotBeNull();
        provider.GetRequiredService<ILiveIndexService>().Should().NotBeNull();
        provider.GetRequiredService<ITrafficService>().Should().NotBeNull();
        provider.GetRequiredService<IEndpointProvider>().Should().NotBeNull();
        provider.GetRequiredService<IMojiHttpClient>().Should().NotBeNull();
        client.Should().NotBeNull();
    }

    [Fact]
    public void AddMojiWeather_ShouldConfigureHttpClientTimeout()
    {
        // 准备
        var timeout = TimeSpan.FromSeconds(12);
        using var provider = BuildProvider(options =>
        {
            options.AppCode = "test-appcode-12345";
            options.Timeout = timeout;
        });

        var mojiClient = provider.GetRequiredService<IMojiHttpClient>();
        var configuredOptions = provider.GetRequiredService<IOptions<MojiWeatherOptions>>().Value;

        // 断言
        var httpClient = GetInnerHttpClient(mojiClient);
        configuredOptions.Timeout.Should().Be(timeout);
        httpClient.DefaultRequestHeaders.Accept.Should().Contain(header =>
            header.MediaType == "application/json");
    }

    [Fact]
    public void AddMojiWeather_ShouldConfigureRetryPolicy()
    {
        // 准备
        var retryOptions = new RetryOptions
        {
            MaxRetries = 7,
            InitialDelay = TimeSpan.FromMilliseconds(800),
            BackoffMultiplier = 3.5
        };

        using var provider = BuildProvider(options =>
        {
            options.AppCode = "test-appcode-12345";
            options.Retry = retryOptions;
        });

        // 执行
        var configuredOptions = provider.GetRequiredService<IOptions<MojiWeatherOptions>>().Value;
        var retryStrategy = ServiceCollectionExtensions.BuildRetryStrategyOptions(configuredOptions.Retry);

        // 断言
        retryStrategy.MaxRetryAttempts.Should().Be(retryOptions.MaxRetries);
        retryStrategy.Delay.Should().Be(retryOptions.InitialDelay);
        retryStrategy.BackoffType.Should().Be(DelayBackoffType.Exponential);
        retryStrategy.UseJitter.Should().BeFalse();
        retryStrategy.DelayGenerator.Should().NotBeNull();
    }

    [Fact]
    public void Services_ShouldHaveCorrectLifetime()
    {
        // 准备
        var services = new ServiceCollection();
        services.AddMojiWeather(options => options.AppCode = "test-appcode-12345");

        // 断言
        services.Single(descriptor => descriptor.ServiceType == typeof(IWeatherService))
            .Lifetime.Should().Be(ServiceLifetime.Singleton);
        services.Single(descriptor => descriptor.ServiceType == typeof(IForecastService))
            .Lifetime.Should().Be(ServiceLifetime.Singleton);
        services.Single(descriptor => descriptor.ServiceType == typeof(IAirQualityService))
            .Lifetime.Should().Be(ServiceLifetime.Singleton);
        services.Single(descriptor => descriptor.ServiceType == typeof(IAlertService))
            .Lifetime.Should().Be(ServiceLifetime.Singleton);
        services.Single(descriptor => descriptor.ServiceType == typeof(ILiveIndexService))
            .Lifetime.Should().Be(ServiceLifetime.Singleton);
        services.Single(descriptor => descriptor.ServiceType == typeof(ITrafficService))
            .Lifetime.Should().Be(ServiceLifetime.Singleton);
        services.Single(descriptor => descriptor.ServiceType == typeof(IEndpointProvider))
            .Lifetime.Should().Be(ServiceLifetime.Singleton);
        services.Single(descriptor => descriptor.ServiceType == typeof(IMojiWeatherClient))
            .Lifetime.Should().Be(ServiceLifetime.Singleton);

        services.Should().Contain(descriptor =>
            descriptor.ServiceType == typeof(IMojiHttpClient) &&
            descriptor.Lifetime == ServiceLifetime.Transient);
    }

    private static ServiceProvider BuildProvider(Action<MojiWeatherOptions> configure)
    {
        var services = new ServiceCollection();
        services.AddMojiWeather(configure);
        return services.BuildServiceProvider();
    }

    private static HttpClient GetInnerHttpClient(IMojiHttpClient client)
    {
        var field = client.GetType()
            .GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
            .FirstOrDefault(info => info.FieldType == typeof(HttpClient));

        if (field?.GetValue(client) is not HttpClient httpClient)
        {
            throw new InvalidOperationException("未能从 IMojiHttpClient 获取内部 HttpClient 实例。");
        }

        return httpClient;
    }

    
}
