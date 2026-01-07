using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Extensions;
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
}
