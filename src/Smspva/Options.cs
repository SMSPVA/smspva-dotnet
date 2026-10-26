namespace Smspva;

public sealed record ClientOptions
{
    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(30);
    public Uri? ActivationBaseUrl { get; init; }
    public Uri? RentalBaseUrl { get; init; }
    public Uri? EmailBaseUrl { get; init; }
}
public sealed record RequestOptions
{
    public TimeSpan? Timeout { get; init; }
    public bool UseApiKey { get; init; } = true;
}
public interface ISmspvaRequest
{
    IReadOnlyDictionary<string, object?> ToParameters();
}
