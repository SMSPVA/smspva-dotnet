using System.Text.Json;
namespace Smspva;

public class SmspvaException : Exception
{
    public string Family { get; }
    public string Operation { get; }
    internal SmspvaException(string message, string family, string operation) : base(message) { Family = family; Operation = operation; }
}
public sealed class ValidationException : SmspvaException
{
    internal ValidationException(string message, string family = "", string operation = "") : base(message, family, operation) { }
}
public sealed class ApiException : SmspvaException
{
    public int HttpStatus { get; }
    public JsonElement Code { get; }
    public JsonElement Response { get; }
    internal ApiException(string family, string operation, int httpStatus, JsonElement code, JsonElement response)
        : base($"SMSPVA {family} request failed (HTTP {httpStatus})", family, operation) { HttpStatus=httpStatus; Code=code.Clone(); Response=response.Clone(); }
}
public sealed class ProtocolException : SmspvaException
{
    public int HttpStatus { get; }
    public bool OutcomeUnknown { get; }
    internal ProtocolException(string family, string operation, int status, bool stateful)
        : base($"SMSPVA {family} returned an invalid response (HTTP {status})", family, operation) { HttpStatus=status; OutcomeUnknown=stateful; }
}
public class TransportException : SmspvaException
{
    public bool OutcomeUnknown { get; }
    public string Kind { get; }
    internal TransportException(string family, string operation, bool stateful, string kind)
        : base($"SMSPVA {family} request {kind}; no automatic retry", family, operation) { OutcomeUnknown=stateful; Kind=kind; }
}
public sealed class RequestTimeoutException : TransportException
{
    internal RequestTimeoutException(string family, string operation, bool stateful) : base(family, operation, stateful, "timeout") { }
}
public sealed class RequestCanceledException : OperationCanceledException
{
    public string Family { get; }
    public string Operation { get; }
    public bool OutcomeUnknown { get; }
    internal RequestCanceledException(string family, string operation, bool stateful, CancellationToken token)
        : base($"SMSPVA {family} request canceled; no automatic retry", token) { Family=family; Operation=operation; OutcomeUnknown=stateful; }
}
public sealed class PollingTimeoutException : SmspvaException
{
    internal PollingTimeoutException() : base("No SMS result before the saved order deadline", "activation", "activation.receiveSms") { }
}
