using System.Reflection;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MojiWeather.Sdk.Abstractions;
using MojiWeather.Sdk.Configuration;
using MojiWeather.Sdk.Extensions;
using MojiWeather.Sdk.Sample.Playground;

// 配置控制台编码
Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

// 创建主机
var builder = Host.CreateApplicationBuilder(args);

// 加载 User Secrets 配置（不依赖环境变量）
builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), optional: true);

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

// 运行 Playground 应用
var app = new PlaygroundApp(weatherClient);
await app.RunAsync();
