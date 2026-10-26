using System.Diagnostics;
using System.Text.Json;
namespace Smspva;

public sealed partial class ActivationResource
{
    public async Task<JsonElement> WaitForSmsAsync(long orderId, DateTimeOffset expiresAt, TimeSpan? interval = null, string? partnerKey = null, CancellationToken cancellationToken = default)
    {
        var pause = interval ?? TimeSpan.FromSeconds(5);
        if (pause < TimeSpan.FromSeconds(5) || pause.TotalMilliseconds > int.MaxValue) throw new ValidationException("Polling interval must be at least five seconds and at most 2147483647 milliseconds");
        var available = expiresAt - DateTimeOffset.UtcNow;
        var clock = Stopwatch.StartNew();
        while (clock.Elapsed < available) {
            var remaining = available - clock.Elapsed;
            var response = await ReceiveSmsAsync(new ActivationReceiveSmsRequest(orderId) { PartnerKey = partnerKey }, new RequestOptions { Timeout = remaining < _client.DefaultTimeout ? remaining : _client.DefaultTimeout }, cancellationToken).ConfigureAwait(false);
            if (!SmspvaClient.IsPending(response)) return response;
            var left = available - clock.Elapsed;
            if (left <= TimeSpan.Zero) throw new PollingTimeoutException();
            await Task.Delay(left < pause ? left : pause, cancellationToken).ConfigureAwait(false);
            if (left <= pause) throw new PollingTimeoutException();
        }
        throw new PollingTimeoutException();
    }
}
