using Xunit;

public class IncidentDetectorTests
{
    private static readonly DateTime Start =
        new DateTime(2026, 11, 5, 8, 0, 0, DateTimeKind.Utc);

    private static CheckResult Check(int minutes, bool ok) =>
        new CheckResult(
            Start.AddMinutes(minutes),
            ok,
            ok ? 200 : null,
            ok ? 100 : null,
            ok ? null : "timeout");

    [Fact]
    public void AllSuccessful_HasNoIncidents()
    {
        var history = new List<CheckResult> { Check(0, true), Check(30, true) };

        Assert.Empty(IncidentDetector.Detect("a", history));
    }

    [Fact]
    public void SlowButSuccessful_IsNotAnIncident()
    {
        var slow = new CheckResult(Start, true, 200, 5000, null);

        Assert.Empty(IncidentDetector.Detect("a", new List<CheckResult> { slow }));
    }

    [Fact]
    public void FailureThenRecovery_IsOneFinishedIncident()
    {
        var history = new List<CheckResult> { Check(0, true), Check(30, false), Check(60, true) };

        var incident = Assert.Single(IncidentDetector.Detect("a", history));

        Assert.Equal(Start.AddMinutes(30), incident.Start);
        Assert.Equal(Start.AddMinutes(60), incident.End.GetValueOrDefault());
        Assert.Equal(30.0, incident.DurationMinutes.GetValueOrDefault());
    }

    [Fact]
    public void FailureWithoutRecovery_IsOngoing()
    {
        var history = new List<CheckResult> { Check(0, true), Check(30, false), Check(60, false) };

        var incident = Assert.Single(IncidentDetector.Detect("a", history));

        Assert.Equal(Start.AddMinutes(30), incident.Start);
        Assert.Null(incident.End);
        Assert.Null(incident.DurationMinutes);
    }

    [Fact]
    public void TwoSeparateOutages_AreTwoIncidents()
    {
        var history = new List<CheckResult>
        {
            Check(0, false), Check(30, true), Check(60, false), Check(90, true),
        };

        Assert.Equal(2, IncidentDetector.Detect("a", history).Count);
    }

    [Fact]
    public void UnsortedHistory_IsSortedFirst()
    {
        var history = new List<CheckResult> { Check(60, true), Check(0, false), Check(30, false) };

        var incident = Assert.Single(IncidentDetector.Detect("a", history));

        Assert.Equal(Start, incident.Start);
        Assert.Equal(Start.AddMinutes(60), incident.End.GetValueOrDefault());
    }
}