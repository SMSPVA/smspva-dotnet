using System.Globalization;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
namespace Smspva;

public sealed class SmspvaClient : IDisposable
{
    internal static readonly IReadOnlyDictionary<string, object?> EmptyParameters = new Dictionary<string, object?>();
    private static readonly JsonElement Operations = LoadContracts();
    private readonly string? _apiKey;
    private readonly ClientOptions _options;
    private readonly HttpClient _http;
    private readonly bool _ownsHttp;
    internal TimeSpan DefaultTimeout => _options.Timeout;
    public ActivationResource Activation { get; }
    public RentalResource Rental { get; }
    public EmailResource Email { get; }

    public SmspvaClient(string? apiKey = null, ClientOptions? options = null, HttpClient? httpClient = null)
    {
        if (apiKey is not null && (apiKey.Length == 0 || apiKey.Contains('\r') || apiKey.Contains('\n'))) throw new ValidationException("API key must be nonempty and contain no line breaks");
        _apiKey = apiKey;
        _options = options ?? new ClientOptions();
        ValidateTimeout(_options.Timeout);
        foreach (var uri in new[] {_options.ActivationBaseUrl,_options.RentalBaseUrl,_options.EmailBaseUrl}) if (uri is not null) ValidateBaseUrl(uri);
        _ownsHttp = httpClient is null;
        _http = httpClient ?? new HttpClient(new SocketsHttpHandler {
            AllowAutoRedirect = false, UseCookies = false, UseProxy = false,
            // Fresh HTTP/1.1 connections avoid stale pooled-connection and HTTP/2 replay paths for paid GETs.
            PooledConnectionLifetime = TimeSpan.Zero,
        }) { Timeout = System.Threading.Timeout.InfiniteTimeSpan };
        Activation = new ActivationResource(this); Rental = new RentalResource(this); Email = new EmailResource(this);
    }

    private static JsonElement LoadContracts()
    {
        using var stream = typeof(SmspvaClient).Assembly.GetManifestResourceStream("Smspva.contracts.json") ?? throw new InvalidOperationException("Missing bundled API contracts");
        using var document = JsonDocument.Parse(stream);
        return document.RootElement.GetProperty("operations").Clone();
    }
    private static void ValidateTimeout(TimeSpan value)
    {
        if (value.TotalMilliseconds <= 0 || value.TotalMilliseconds > int.MaxValue) throw new ValidationException("Timeout must be positive and at most 2147483647 milliseconds");
    }
    private static void ValidateBaseUrl(Uri uri)
    {
        if (!uri.IsAbsoluteUri || (uri.Scheme != "http" && uri.Scheme != "https") || uri.UserInfo.Length > 0 || uri.Query.Length > 0 || uri.Fragment.Length > 0) throw new ValidationException("Base URL must be HTTP(S) without credentials, query or fragment");
    }
    private static string Encode(object value) => Uri.EscapeDataString(Convert.ToString(value, CultureInfo.InvariantCulture) ?? "");
    private static void ValidateValue(JsonElement p, object value)
    {
        var name = p.GetProperty("name").GetString()!;
        var schema = p.GetProperty("schema");
        var type = schema.GetProperty("type").GetString();
        if ((type == "integer" && value is not long) || (type == "string" && value is not string)) throw new ValidationException($"Invalid type for parameter: {name}");
        if (schema.TryGetProperty("enum", out var choices) && !choices.EnumerateArray().Any(choice => type == "integer" ? choice.TryGetInt64(out var n) && n == (long)value : choice.GetString() == (string)value)) throw new ValidationException($"Invalid value for parameter: {name}");
        if (value is long number && ((schema.TryGetProperty("minimum",out var minimum) && number < minimum.GetInt64()) || (schema.TryGetProperty("maximum",out var maximum) && number > maximum.GetInt64()))) throw new ValidationException($"Invalid value for parameter: {name}");
        if (value is string text && ((schema.TryGetProperty("minLength",out var min) && text.Length < min.GetInt32()) || (schema.TryGetProperty("maxLength",out var max) && text.Length > max.GetInt32()) || (schema.TryGetProperty("pattern",out var pattern) && !Regex.IsMatch(text, pattern.GetString()!, RegexOptions.CultureInvariant, TimeSpan.FromSeconds(1))))) throw new ValidationException($"Invalid value for parameter: {name}");
    }

    internal async Task<JsonElement> RequestAsync(string key, IReadOnlyDictionary<string, object?> input, RequestOptions? options, CancellationToken cancellationToken)
    {
        var op = Operations.GetProperty(key);
        var family = op.GetProperty("family").GetString()!;
        var stateful = op.GetProperty("stateful").GetBoolean();
        var parameters = op.GetProperty("params").EnumerateArray().ToArray();
        if (input.Keys.Any(name => !parameters.Any(p => p.GetProperty("name").GetString() == name))) throw new ValidationException("Unexpected request parameter", family, key);
        var path = op.GetProperty("path").GetString()!;
        var query = new List<string>();
        if (op.TryGetProperty("action",out var action)) query.Add("method=" + Encode(action.GetString()!));
        using var request = new HttpRequestMessage(new HttpMethod(op.GetProperty("method").GetString()!), "http://unused.local") { Version = HttpVersion.Version11, VersionPolicy = HttpVersionPolicy.RequestVersionExact };
        request.Headers.ConnectionClose = true;
        request.Headers.Accept.ParseAdd("application/json");
        foreach (var parameter in parameters)
        {
            var name = parameter.GetProperty("name").GetString()!;
            input.TryGetValue(name, out var value);
            if (value is null || (parameter.TryGetProperty("omitEmpty", out var omit) && omit.GetBoolean() && value is string text && text.Length == 0)) {
                if (parameter.GetProperty("required").GetBoolean()) throw new ValidationException($"Missing required parameter: {name}", family, key);
                continue;
            }
            ValidateValue(parameter, value);
            var location = parameter.GetProperty("in").GetString();
            if (location == "path") {
                if (value is "." or "..") throw new ValidationException($"Invalid path parameter: {name}", family, key);
                path = path.Replace("{" + name + "}", Encode(value), StringComparison.Ordinal);
            } else if (location == "header") {
                var header = Convert.ToString(value,CultureInfo.InvariantCulture)!;
                if (header.Contains('\r') || header.Contains('\n')) throw new ValidationException($"Invalid header: {name}", family, key);
                request.Headers.TryAddWithoutValidation(name, header);
            } else query.Add(Encode(name) + "=" + Encode(value));
        }
        options ??= new RequestOptions();
        var auth = op.GetProperty("auth").GetString();
        if (auth == "required" && _apiKey is null) throw new ValidationException("Set an API key for this operation", family, key);
        if (_apiKey is not null && auth != "none" && (auth == "required" || options.UseApiKey)) {
            if (family == "activation") request.Headers.TryAddWithoutValidation("apikey", _apiKey);
            else query.Add("apikey=" + Encode(_apiKey));
        }
        var baseUrl = family switch { "activation" => _options.ActivationBaseUrl, "rental" => _options.RentalBaseUrl, _ => _options.EmailBaseUrl };
        request.RequestUri = new Uri((baseUrl?.AbsoluteUri ?? op.GetProperty("baseUrl").GetString()!).TrimEnd('/') + path + (query.Count > 0 ? "?" + string.Join("&",query) : ""));
        var timeout = options.Timeout ?? _options.Timeout;
        ValidateTimeout(timeout);
        using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        linked.CancelAfter(timeout);
        JsonElement body;
        int status;
        try {
            using var response = await _http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, linked.Token).ConfigureAwait(false);
            status = (int)response.StatusCode;
            var text = await response.Content.ReadAsStringAsync(linked.Token).ConfigureAwait(false);
            try { using var document = JsonDocument.Parse(text); body = document.RootElement.Clone(); }
            catch (JsonException) { throw new ProtocolException(family, key, status, stateful); }
        } catch (OperationCanceledException) {
            if (cancellationToken.IsCancellationRequested) throw new RequestCanceledException(family, key, stateful, cancellationToken);
            throw new RequestTimeoutException(family, key, stateful);
        } catch (HttpRequestException) { throw new TransportException(family, key, stateful, "network"); }
          catch (IOException) { throw new TransportException(family, key, stateful, "network"); }
        if (body.ValueKind != JsonValueKind.Object) throw new ProtocolException(family, key, status, stateful);
        body.TryGetProperty(family == "activation" ? "statusCode" : "status", out var apiStatus);
        JsonElement code = JsonSerializer.SerializeToElement(status);
        var hasError = body.TryGetProperty("error", out var error) && error.ValueKind != JsonValueKind.Null && error.ValueKind != JsonValueKind.False;
        if (error.ValueKind == JsonValueKind.Object && error.TryGetProperty("type", out var errorType) && errorType.ValueKind == JsonValueKind.String) code = errorType;
        else if (body.TryGetProperty("errorid",out var errorId) && errorId.ValueKind == JsonValueKind.Number) code = errorId;
        else if (apiStatus.ValueKind == JsonValueKind.Number) code = apiStatus;
        if (status < 200 || status >= 300) throw new ApiException(family, key, status, code, body);
        if (apiStatus.ValueKind != JsonValueKind.Number || !apiStatus.TryGetInt32(out var numberStatus)) throw new ProtocolException(family, key, status, stateful);
        if (op.GetProperty("pending").GetBoolean() && status == 202 && IsPending(body)) return body;
        if (op.GetProperty("noStock").GetBoolean() && status == 200 && numberStatus == 2) return body;
        if (family == "activation" ? numberStatus != 200 || hasError : numberStatus != 1) throw new ApiException(family, key, status, code, body);
        return body;
    }
    public static bool IsPending(JsonElement value) => value.ValueKind == JsonValueKind.Object && value.TryGetProperty("statusCode",out var status) && status.ValueKind == JsonValueKind.Number && status.TryGetInt32(out var code) && code == 202 && value.TryGetProperty("error",out var error) && error.ValueKind == JsonValueKind.Object && error.TryGetProperty("type",out var type) && type.ValueKind == JsonValueKind.String && type.GetString() == "SMS_NOT_YET_RECEIVED";
    public static bool IsNoStock(JsonElement value) => value.ValueKind == JsonValueKind.Object && value.TryGetProperty("status",out var status) && status.ValueKind == JsonValueKind.Number && status.TryGetInt32(out var code) && code == 2;
    public void Dispose() { if (_ownsHttp) _http.Dispose(); }
}
