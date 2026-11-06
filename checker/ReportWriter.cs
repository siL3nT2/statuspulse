using System.Text.Json;

public static class ReportWriter
{
    private static readonly JsonSerializerOptions Options =
        new JsonSerializerOptions(JsonSerializerDefaults.Web) { WriteIndented = true };

    public static string ToJson(LatestReport report) =>
        JsonSerializer.Serialize(report, Options);

    public static string IncidentsToJson(List<Incident> incidents) =>
        JsonSerializer.Serialize(incidents, Options);

    public static async Task WriteLatestAsync(LatestReport report, string folder)
    {
        await WriteFileAsync(folder, "latest.json", ToJson(report));
    }

    public static async Task WriteIncidentsAsync(List<Incident> incidents, string folder)
    {
        await WriteFileAsync(folder, "incidents.json", IncidentsToJson(incidents));
    }

    private static async Task WriteFileAsync(string folder, string fileName, string content)
    {
        Directory.CreateDirectory(folder);
        await File.WriteAllTextAsync(Path.Combine(folder, fileName), content);
    }
}