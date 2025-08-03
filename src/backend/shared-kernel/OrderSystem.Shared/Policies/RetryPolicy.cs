using Polly;
using Polly.Extensions.Http;

namespace OrderSystem.Shared.Policies;

public static class RetryPolicy
{
    public static IAsyncPolicy<HttpResponseMessage> GetPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError() // 5xx, 408
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}
