public static class RetryPolicy
{
    public static async Task<CheckResult> CheckWithRetryAsync(
        Func<Task<CheckResult>> check,
        TimeSpan delay)
        {
            var first = await check();

            if (first.Ok)
            {
                return first;
            }

            await Task.Delay(delay);
            return await check();
        }
}