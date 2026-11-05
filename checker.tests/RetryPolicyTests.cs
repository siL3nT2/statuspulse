using Xunit;

public class RetryPolicyTests
{
    private static CheckResult Up() =>
    new CheckResult(DateTime.UtcNow, true, 200, 100, null);

    private static CheckResult Down() =>
    new CheckResult(DateTime.UtcNow, false, null, null, "timeout");

    [Fact]
    public async Task SuccessOnFirstTry_DoesNotRetry()
    {
        int calls = 0;

        var result = await RetryPolicy.CheckWithRetryAsync(() =>
        {
            calls++;
            return Task.FromResult(Up());
        }, TimeSpan.Zero);

        Assert.True(result.Ok);
        Assert.Equal(1, calls);
    }

    [Fact]
    public async Task FailureThenSuccess_ReturnsSuccess()
    {
        int calls = 0;

        var result = await RetryPolicy.CheckWithRetryAsync(() =>
        {
            calls++;
            return Task.FromResult(calls == 1 ? Down() : Up());
        }, TimeSpan.Zero);

        Assert.True(result.Ok);
        Assert.Equal(2, calls);
    }

    [Fact]
    public async Task FailureTwice_ReturnsFailure()
    {
        int calls = 0;

        var result = await RetryPolicy.CheckWithRetryAsync(() =>
        {
            calls++;
            return Task.FromResult(Down());
        }, TimeSpan.Zero);

        Assert.False(result.Ok);
        Assert.Equal(2, calls);
    }
}