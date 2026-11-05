public record CheckResult(
    DateTime Time,
    bool Ok,
    int? Status,
    int? Ms,
    string? Error
);