using Xunit;

public class StatusRulesTests
{
    [Fact]
    public void FastSuccessfulCheck_IsUp()
    {
        var result = new CheckResult(DateTime.UtcNow, true, 200, 245, null);

        Assert.Equal("up", StatusRules.GetStatus(result));
    }

    [Fact]
    public void SlowSuccessfulCheck_IsDegraded()
    {
        var result = new CheckResult(DateTime.UtcNow, true, 200, 4200, null);

        Assert.Equal("degraded", StatusRules.GetStatus(result));
    }

    [Fact]
    public void FailedCheck_IsDown()
    {
        var result = new CheckResult(DateTime.UtcNow, false, null, null, "timeout");

        Assert.Equal("down", StatusRules.GetStatus(result));
    }

    [Fact]
    public void ExactlyThreeThousandMs_IsUp()
    {
        var result = new CheckResult(DateTime.UtcNow, true, 200, 3000, null);

        Assert.Equal("up", StatusRules.GetStatus(result));
    }
}