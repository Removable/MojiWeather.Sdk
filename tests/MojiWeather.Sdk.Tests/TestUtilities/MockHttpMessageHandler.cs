using System.Net;

namespace MojiWeather.Sdk.Tests.TestUtilities;

/// <summary>
/// 用于测试的 HTTP 消息处理器
/// </summary>
internal sealed class MockHttpMessageHandler : HttpMessageHandler
{
    private readonly Func<HttpRequestMessage, Task<HttpResponseMessage>> _handler;

    public MockHttpMessageHandler(string responseContent, HttpStatusCode statusCode)
    {
        _handler = _ => Task.FromResult(new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(responseContent)
        });
    }

    public MockHttpMessageHandler(Exception exception)
    {
        _handler = _ => throw exception;
    }

    public MockHttpMessageHandler(Func<HttpRequestMessage, HttpResponseMessage> handler)
    {
        _handler = request => Task.FromResult(handler(request));
    }

    public MockHttpMessageHandler(Func<HttpRequestMessage, Task<HttpResponseMessage>> handler)
    {
        _handler = handler;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return _handler(request);
    }
}
