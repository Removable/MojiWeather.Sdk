using System.Net.Http;
using FluentAssertions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Extensions;
using Polly;
using Polly.Retry;
using Xunit;

namespace MojiWeather.Sdk.Tests.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void BuildRetryStrategyOptions_WithDefaultMultiplier_ShouldUseStandardExponentialBackoff()
    {
        // 准备
        var retryOptions = new RetryOptions
        {
            MaxRetries = 5,
            InitialDelay = TimeSpan.FromSeconds(1),
            BackoffMultiplier = 2.0
        };

        // 执行
        var options = ServiceCollectionExtensions.BuildRetryStrategyOptions(retryOptions);

        // 断言
        options.MaxRetryAttempts.Should().Be(5);
        options.Delay.Should().Be(TimeSpan.FromSeconds(1));
        options.BackoffType.Should().Be(DelayBackoffType.Exponential);
        options.UseJitter.Should().BeTrue();
    }

    [Fact]
    public async Task BuildRetryStrategyOptions_WithCustomMultiplier_ShouldUseCustomDelayGenerator()
    {
        // 准备
        var retryOptions = new RetryOptions
        {
            MaxRetries = 2,
            InitialDelay = TimeSpan.FromMilliseconds(200),
            BackoffMultiplier = 3.0
        };

        // 执行
        var options = ServiceCollectionExtensions.BuildRetryStrategyOptions(retryOptions);

        // 断言
        options.UseJitter.Should().BeFalse();
        options.DelayGenerator.Should().NotBeNull();

        var context = ResilienceContextPool.Shared.Get();
        try
        {
            var outcome = Outcome.FromResult(new HttpResponseMessage());
            var delay0 = await options.DelayGenerator!(
                new RetryDelayGeneratorArguments<HttpResponseMessage>(context, outcome, 0));
            var delay1 = await options.DelayGenerator!(
                new RetryDelayGeneratorArguments<HttpResponseMessage>(context, outcome, 1));

            delay0.Should().Be(TimeSpan.FromMilliseconds(200));
            delay1.Should().Be(TimeSpan.FromMilliseconds(600));
        }
        finally
        {
            ResilienceContextPool.Shared.Return(context);
        }
    }
}
