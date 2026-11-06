public record Incident(string SiteId, DateTime Start, DateTime? End)
{
    public double? DurationMinutes =>
        End is null ? null : Math.Round((End.Value - Start).TotalMinutes, 1);
}