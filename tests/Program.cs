using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Smspva;

var tests = 0;
void Check(bool value, string message) { if (!value) throw new Exception(message); }
JsonElement Json(string text) { using var document=JsonDocument.Parse(text); return document.RootElement.Clone(); }
async Task<T> Throws<T>(Func<Task> action) where T:Exception { try { await action(); } catch(T error) { return error; } throw new Exception($"Expected {typeof(T).Name}"); }
var fixtures = Json(await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory,"cases.json")));
var operations = Json(await File.ReadAllTextAsync(Path.Combine(AppContext.BaseDirectory,"contracts.json"))).GetProperty("operations");
const string secret="dotnet-local-secret-+&?=/";
var current=fixtures[0];
var seen=new List<(string Method,Uri Uri, Dictionary<string,string> Headers)>();
using var handler=new FakeHandler((request,_) => {
    seen.Add((request.Method.Method,request.RequestUri!,request.Headers.ToDictionary(h=>h.Key,h=>string.Join(",",h.Value),StringComparer.OrdinalIgnoreCase)));
    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) {Content=new StringContent(current.GetProperty("response").GetRawText())});
});
using var http=new HttpClient(handler) {Timeout=Timeout.InfiniteTimeSpan};
using var client=new SmspvaClient(secret,httpClient:http);
foreach(var fixture in fixtures.EnumerateArray()) {
    current=fixture;
    var family=fixture.GetProperty("family").GetString()!;
    var operation=operations.GetProperty(fixture.GetProperty("key").GetString()!);
    var resource=typeof(SmspvaClient).GetProperty(char.ToUpperInvariant(family[0])+family[1..])!.GetValue(client)!;
    var name=fixture.GetProperty("jsName").GetString()!;
    var method=resource.GetType().GetMethod(char.ToUpperInvariant(name[0])+name[1..]+"Async")!;
    var methodArgs=new List<object?>();
    var input=fixture.GetProperty("params");
    if(operation.GetProperty("params").GetArrayLength()>0) {
        var requestType=method.GetParameters()[0].ParameterType;
        var constructor=requestType.GetConstructors().Single();
        object? Value(string propertyName,Type type) {
            var property=requestType.GetProperty(propertyName)!;
            var wire=property.GetCustomAttribute<JsonPropertyNameAttribute>()!.Name;
            if(!input.TryGetProperty(wire,out var value)) return null;
            return (Nullable.GetUnderlyingType(type)??type)==typeof(long) ? value.GetInt64() : value.GetString();
        }
        var request=constructor.Invoke(constructor.GetParameters().Select(p=>Value(p.Name!,p.ParameterType)).ToArray());
        foreach(var property in requestType.GetProperties()) property.SetValue(request,Value(property.Name,property.PropertyType));
        methodArgs.Add(request);
    }
    methodArgs.Add(null); methodArgs.Add(CancellationToken.None);
    var result=await (Task<JsonElement>)method.Invoke(resource,methodArgs.ToArray())!;
    Check(result.GetRawText()==fixture.GetProperty("response").GetRawText(),"raw result changed: "+name);
    var actual=seen[^1];
    Check(actual.Method==operation.GetProperty("method").GetString(),"verb changed: "+name);
    var path=operation.GetProperty("path").GetString()!;
    var expectedQuery=new Dictionary<string,string>();
    if(operation.TryGetProperty("action",out var action)) expectedQuery["method"]=action.GetString()!;
    foreach(var p in operation.GetProperty("params").EnumerateArray()) {
        var pName=p.GetProperty("name").GetString()!;
        if(!input.TryGetProperty(pName,out var pValue)) continue;
        var text=pValue.ValueKind==JsonValueKind.String?pValue.GetString()!:pValue.GetRawText();
        if(p.GetProperty("in").GetString()=="path") path=path.Replace("{"+pName+"}",Uri.EscapeDataString(text),StringComparison.Ordinal);
        else if(p.GetProperty("in").GetString()=="query" && !(p.TryGetProperty("omitEmpty",out var omit) && omit.GetBoolean() && text=="")) expectedQuery[pName]=text;
    }
    var auth=operation.GetProperty("auth").GetString();
    if(auth!="none" && family!="activation") expectedQuery["apikey"]=secret;
    var query=actual.Uri.Query.TrimStart('?').Split('&',StringSplitOptions.RemoveEmptyEntries).Select(p=>p.Split('=',2)).ToDictionary(p=>Uri.UnescapeDataString(p[0]),p=>Uri.UnescapeDataString(p[1]));
    Check(actual.Uri.AbsolutePath==path,"path changed: "+name);
    Check(query.Count==expectedQuery.Count && expectedQuery.All(p=>query.TryGetValue(p.Key,out var value)&&value==p.Value),"query changed: "+name);
    Check(actual.Headers.GetValueOrDefault("apikey")==((auth!="none"&&family=="activation")?secret:null),"auth changed: "+name);
    Check(actual.Headers.GetValueOrDefault("Connection")=="close","connection replay guard missing");
    tests++;
}
Check(tests==56 && seen.Count==56,"all methods must be covered exactly once");
Console.WriteLine("PASS: all 56 named methods preserve wire contracts and response JSON");

var status=HttpStatusCode.OK;
var body="{\"status\":1,\"data\":[]}";
var calls=0;
using var edgeHandler=new FakeHandler((request,_) => { calls++; seen.Add((request.Method.Method,request.RequestUri!,request.Headers.ToDictionary(h=>h.Key,h=>string.Join(",",h.Value),StringComparer.OrdinalIgnoreCase))); return Task.FromResult(new HttpResponseMessage(status){Content=new StringContent(body)}); });
using var edgeHttp=new HttpClient(edgeHandler){Timeout=Timeout.InfiniteTimeSpan};
using var edge=new SmspvaClient(secret,httpClient:edgeHttp);
await edge.Email.GetDataAsync(new EmailGetDataRequest {Pcode=""});
Check(!seen[^1].Uri.Query.Contains("pcode",StringComparison.Ordinal),"empty pcode must be omitted");
await edge.Rental.GetCountriesAsync(options:new RequestOptions{UseApiKey=false});
Check(!seen[^1].Uri.Query.Contains("apikey",StringComparison.Ordinal),"public key must be omitted");
await edge.Rental.GetDataAsync(new RentalGetDataRequest("US"),new RequestOptions{UseApiKey=false});
Check(!seen[^1].Uri.Query.Contains("apikey",StringComparison.Ordinal),"optional key must be omitted");
await edge.Email.GetDataAsync(new EmailGetDataRequest{Pcode="provider +&?/="});
Check(seen[^1].Uri.Query.Contains("pcode=provider%20%2B%26%3F%2F%3D",StringComparison.Ordinal),"query escaping changed");
body="{\"statusCode\":200,\"data\":{}}";
await edge.Activation.GetOperatorsAsync(new ActivationGetOperatorsRequest("US +/&?#="));
Check(seen[^1].Uri.AbsolutePath.EndsWith("US%20%2B%2F%26%3F%23%3D",StringComparison.Ordinal),"path escaping changed");
await edge.Activation.GetBalanceAsync(options:new RequestOptions{UseApiKey=false});
Check(seen[^1].Headers.GetValueOrDefault("apikey")==secret,"required authentication was disabled");
tests++;

status=HttpStatusCode.Accepted; body="{\"statusCode\":202,\"error\":{\"type\":\"SMS_NOT_YET_RECEIVED\"}}";
Check(SmspvaClient.IsPending(await edge.Activation.ReceiveSmsAsync(new ActivationReceiveSmsRequest(123))),"pending SMS changed");
status=HttpStatusCode.OK; body="{\"status\":2}";
Check(SmspvaClient.IsNoStock(await edge.Email.CreateOrderAsync(new EmailCreateOrderRequest("CODE","week",1))),"no-stock changed");
var wrongNoStock=await Throws<ApiException>(()=>edge.Email.GetProvidersAsync());
Check(wrongNoStock.Code.GetInt32()==2,"no-stock must only be ordinary for create"); tests++;

status=HttpStatusCode.PaymentRequired; body="{\"statusCode\":402,\"error\":{\"type\":\"NO_BALANCE\"}}";
var apiError=await Throws<ApiException>(()=>edge.Activation.GetBalanceAsync());
Check(apiError.HttpStatus==402 && apiError.Code.GetString()=="NO_BALANCE" && apiError.Response.GetProperty("statusCode").GetInt32()==402,"API metadata changed");
Check(!apiError.ToString().Contains(secret,StringComparison.Ordinal),"error leaked credentials");
status=HttpStatusCode.OK; body="{\"status\":0,\"errorid\":301}";
apiError=await Throws<ApiException>(()=>edge.Email.GetProvidersAsync());
Check(apiError.HttpStatus==200 && apiError.Code.GetInt32()==301,"HTTP200 API error metadata changed"); tests++;

foreach(var invalid in new[]{"not-json","[]","{\"status\":\"1\"}","{\"status\":true}","{}"}) {
    body=invalid;
    await Throws<ProtocolException>(()=>edge.Email.GetProvidersAsync());
}
tests++;
var before=calls;
await Throws<ValidationException>(()=>edge.Activation.GetOperatorsAsync(new ActivationGetOperatorsRequest("..")));
await Throws<ValidationException>(()=>edge.Email.CreateOrderAsync(new EmailCreateOrderRequest("CODE","invalid",1)));
using(var noKey=new SmspvaClient(httpClient:edgeHttp)) await Throws<ValidationException>(()=>noKey.Activation.GetBalanceAsync());
Check(calls==before,"validation made an upstream call"); tests++;

var timedCalls=0;
using var slowHandler=new FakeHandler(async (_,ct)=> { timedCalls++; await Task.Delay(Timeout.InfiniteTimeSpan,ct); return new HttpResponseMessage(HttpStatusCode.OK); });
using var slowHttp=new HttpClient(slowHandler){Timeout=Timeout.InfiniteTimeSpan};
using var slow=new SmspvaClient(secret,new ClientOptions{Timeout=TimeSpan.FromMilliseconds(20)},slowHttp);
var timeout=await Throws<RequestTimeoutException>(()=>slow.Activation.GetNumberAsync(new ActivationGetNumberRequest("US","opt1")));
Check(timeout.OutcomeUnknown && timedCalls==1,"timeout repeated paid request or lost outcome"); tests++;
using(var cts=new CancellationTokenSource()) {
    cts.CancelAfter(5);
    var canceled=await Throws<RequestCanceledException>(()=>slow.Activation.GetBalanceAsync(cancellationToken:cts.Token));
    Check(canceled.CancellationToken==cts.Token && !canceled.OutcomeUnknown,"cancellation semantics changed");
}
Check(timedCalls==2,"cancellation retried"); tests++;

before=calls;
await Throws<PollingTimeoutException>(()=>edge.Activation.WaitForSmsAsync(123,DateTimeOffset.UtcNow.AddSeconds(-1)));
Check(calls==before,"expired poll made an upstream call");
status=HttpStatusCode.Accepted; body="{\"statusCode\":202,\"error\":{\"type\":\"SMS_NOT_YET_RECEIVED\"}}";
await Throws<PollingTimeoutException>(()=>edge.Activation.WaitForSmsAsync(123,DateTimeOffset.UtcNow.AddMilliseconds(40)));
Check(calls==before+1,"polling deadline allowed an extra call"); tests++;

// Exercise the default handler through a real local socket: redirects must not be followed.
var listener=new TcpListener(IPAddress.Loopback,0); listener.Start();
var port=((IPEndPoint)listener.LocalEndpoint).Port;
var server=Task.Run(async()=> {
    using var connection=await listener.AcceptTcpClientAsync();
    using var stream=connection.GetStream();
    using var reader=new StreamReader(stream,Encoding.ASCII,leaveOpen:true);
    var first=await reader.ReadLineAsync();
    Check(first!.StartsWith("GET /api/rent_email.php?method=get_providers HTTP/1.1",StringComparison.Ordinal),"default HTTP version/path changed");
    var headers=new List<string>(); string? line;
    while(!string.IsNullOrEmpty(line=await reader.ReadLineAsync())) headers.Add(line);
    Check(headers.Any(x=>x.Equals("Connection: close",StringComparison.OrdinalIgnoreCase)),"default close missing");
    Check(!headers.Any(x=>x.StartsWith("Cookie:",StringComparison.OrdinalIgnoreCase)||x.StartsWith("apikey:",StringComparison.OrdinalIgnoreCase)),"default public credentials leaked");
    var payload="{\"status\":1,\"data\":[]}";
    var wire=$"HTTP/1.1 302 Found\r\nLocation: http://127.0.0.1:{port}/unexpected\r\nContent-Type: application/json\r\nContent-Length: {Encoding.UTF8.GetByteCount(payload)}\r\nConnection: close\r\n\r\n{payload}";
    await stream.WriteAsync(Encoding.UTF8.GetBytes(wire));
});
try {
    using var real=new SmspvaClient(secret,new ClientOptions{EmailBaseUrl=new Uri($"http://127.0.0.1:{port}")});
    var redirect=await Throws<ApiException>(()=>real.Email.GetProvidersAsync());
    Check(redirect.HttpStatus==302,"default transport followed redirect");
    await server;
} finally {listener.Stop();}
tests++;
Console.WriteLine($"PASS: {tests} checks (56 wire cases + auth, pending/no-stock, errors, validation, timeout, cancellation, expiry, real default HTTP transport)");

sealed class FakeHandler(Func<HttpRequestMessage,CancellationToken,Task<HttpResponseMessage>> send) : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,CancellationToken token) => send(request,token);
}
