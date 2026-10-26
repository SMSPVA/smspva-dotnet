# SMSPVA .NET SDK

Official C# and .NET client for [SMSPVA](https://smspva.com/), a service for receiving verification SMS, renting phone numbers and renting private email addresses. Covers all 56 methods in Activation V2 (20), Rental (22) and Private Email (14). Requires .NET 8 or later. This is an asynchronous alpha release. Alternative API is excluded.

## Installation

```sh
dotnet add package Smspva.Sdk --version 0.1.0-alpha.1
```

[Sign in to your SMSPVA account](https://smspva.com/signin.html), open **Profile and ApiKey**, and set **`SMSPVA_API_KEY`** in your server environment. Keep it out of source control. The example reads your balance and reference lists; it does not create an order.

```csharp
using Smspva;

using var client = new SmspvaClient(Environment.GetEnvironmentVariable("SMSPVA_API_KEY"));
Console.WriteLine(await client.Activation.GetBalanceAsync());
Console.WriteLine(await client.Rental.GetCountriesAsync());
Console.WriteLine(await client.Email.GetProvidersAsync());
```

All methods are async and accept a `CancellationToken`. There is no sync-over-async wrapper. Request records provide typed properties and retain the API's field names when encoded. Responses are `JsonElement`, preserving original envelopes, field names, numeric values, strings, IDs and timestamps.

```csharp
// Public live reference data, without sending a key.
using var publicClient = new SmspvaClient();
var providers = await publicClient.Email.GetProvidersAsync();
var services = await publicClient.Email.GetDataAsync(new EmailGetDataRequest { Pcode = "CODE_FROM_GET_PROVIDERS" });
```

Paid creation example (run only when you intend to rent a mailbox):

```csharp
var result = await client.Email.CreateOrderAsync(
    new EmailCreateOrderRequest("CODE_FROM_GET_DATA", "week", 1));
if (SmspvaClient.IsNoStock(result))
    Console.WriteLine("No mailbox available; refresh the counts before trying again.");
else
    Console.WriteLine(result);
```

The generated methods and request records are in `Resources.g.cs` and `Requests.g.cs`. Regenerate them with `node scripts/generate.mjs` after changing `tests/contracts.json`. Building and installing the package does not require the documentation tree. Embedded contracts are included in the NuGet assembly.

## Authentication, errors and timeouts

Activation V2 sends `apikey` as a header; Rental and Email use the query parameter. Public methods omit the account key. Optional-key Rental methods can omit it using `new RequestOptions { UseApiKey = false }`. Required-key methods still require authentication. Omitted or empty Email `pcode` is not sent.

```csharp
try { Console.WriteLine(await client.Activation.GetBalanceAsync()); }
catch (ApiException error) { Console.WriteLine($"{error.HttpStatus}: {error.Code}"); }
catch (TransportException error) { Console.WriteLine($"{error.Kind}; outcome unknown: {error.OutcomeUnknown}"); }
```

`ApiException` retains `HttpStatus`, original JSON `Code`, and raw `Response`. `ProtocolException` means invalid JSON or a malformed envelope. `TransportException` and `RequestTimeoutException` report network/timeout failures without exposing request URLs or keys. `RequestCanceledException` extends `OperationCanceledException` and reports `OutcomeUnknown`; it preserves the caller's cancellation token. Exceptions do not retain the underlying HTTP exception, which could contain credential-bearing URLs. Raw API bodies may contain sensitive message data: redact your own logs.

The default timeout is 30 seconds for the whole HTTP request, including reading its body. Override it in `ClientOptions.Timeout` or a method's `RequestOptions.Timeout`. Base URLs are configurable with `ActivationBaseUrl`, `RentalBaseUrl` and `EmailBaseUrl` for controlled proxies and tests.

The default transport disables redirects, cookies, environment proxies and HTTP/2. It uses fresh HTTP/1.1 connections with `Connection: close`, avoiding stale-pool/HTTP2 replay paths for state-changing GETs. No request is retried by the SDK. An advanced caller may inject an `HttpClient`; that caller is responsible for disabling redirects, retries, cookies and credential-bearing default headers in its handler chain. An injected client's own timeout may shorten the SDK timeout; the SDK does not mutate or dispose an injected client.

Some GET methods purchase or modify orders. After an uncertain paid result, inspect existing orders before repeating it. Public country, service and provider methods fetch current data; the SDK has no fixed lists.

## SMS waiting

HTTP 202 / `SMS_NOT_YET_RECEIVED` is returned normally; use `SmspvaClient.IsPending(result)`. Email creation `status=2` is also returned normally; use `IsNoStock`.

Save the order expiry when obtaining a number and reuse it:

```csharp
// numberResult is the successful response from your own GetNumberAsync request.
var data = numberResult.GetProperty("data");
var expiresAt = DateTimeOffset.UtcNow.AddSeconds(data.GetProperty("orderExpireIn").GetDouble());
var sms = await client.Activation.WaitForSmsAsync(
    data.GetProperty("orderId").GetInt64(), expiresAt, cancellationToken: cancellationToken);
```

The helper polls an existing order with at least five seconds between pending responses. It uses the saved expiry and a monotonic clock; it never purchases or cancels orders. It stops on expiry, terminal API errors, cancellation and transport failures. Retrieving a newly received Activation SMS can incur a charge. `PollingTimeoutException` means the saved deadline passed without a result. The helper bounds each request to the remaining lifetime and the client's configured timeout (30 seconds by default).

## Development and validation

```sh
node scripts/generate.mjs
dotnet run --project tests/Smspva.Tests.csproj
dotnet pack src/Smspva/Smspva.csproj -c Release
```

The standalone assertion runner uses fixture responses and injectable HTTP handlers, exercises all 56 methods plus auth, optional parameters, encoding, errors, pending/no-stock, validation, cancellation, timeout and saved-expiry behavior. It makes no production or paid requests. The library and test runner target .NET 8. Generated sources, contracts and fixtures are committed, so a standalone checkout builds without the documentation workspace.

## License

MIT. See [LICENSE](https://github.com/SMSPVA/smspva-dotnet/blob/main/LICENSE).

## Documentation and support

Read the [SMSPVA API documentation](https://docs.smspva.com/) for parameters, response codes and workflows. Country, service and provider lists come from the live API. SMSPVA supports [phone number rentals](https://smspva.com/rent) and [private email rentals](https://smspva.com/rent-mail.html); availability and prices can change.

For account or billing questions, visit the [SMSPVA help center](https://smspva.com/help.html). Report SDK bugs through [GitHub issues](https://github.com/SMSPVA/smspva-dotnet/issues), without API keys or private message content.
