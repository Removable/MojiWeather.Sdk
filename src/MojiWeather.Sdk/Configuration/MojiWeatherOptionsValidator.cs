using Microsoft.Extensions.Options;

namespace MojiWeather.Sdk.Configuration;

/// <summary>
/// MojiWeatherOptions 配置验证器
/// </summary>
public sealed class MojiWeatherOptionsValidator : IValidateOptions<MojiWeatherOptions>
{
    /// <inheritdoc />
    public ValidateOptionsResult Validate(string? name, MojiWeatherOptions options)
    {
        var errors = new List<string>();

        // 验证 AppCode
        if (string.IsNullOrWhiteSpace(options.AppCode))
        {
            errors.Add("AppCode is required. Please provide your Alibaba Cloud API Marketplace AppCode.");
        }
        else if (options.AppCode.Length < 10)
        {
            errors.Add("AppCode appears to be invalid. Please verify your Alibaba Cloud API Marketplace AppCode.");
        }

        // 验证超时时间
        if (options.Timeout <= TimeSpan.Zero)
        {
            errors.Add("Timeout must be greater than zero.");
        }
        else if (options.Timeout > TimeSpan.FromMinutes(5))
        {
            errors.Add("Timeout should not exceed 5 minutes.");
        }

        // 验证重试配置
        if (options.Retry.MaxRetries < 0)
        {
            errors.Add("MaxRetries cannot be negative.");
        }
        else if (options.Retry.MaxRetries > 10)
        {
            errors.Add("MaxRetries should not exceed 10 to avoid excessive delays.");
        }

        if (options.Retry.InitialDelay < TimeSpan.Zero)
        {
            errors.Add("InitialDelay cannot be negative.");
        }

        if (options.Retry.BackoffMultiplier < 1.0)
        {
            errors.Add("BackoffMultiplier must be at least 1.0.");
        }

        return errors.Count > 0
            ? ValidateOptionsResult.Fail(errors)
            : ValidateOptionsResult.Success;
    }
}
