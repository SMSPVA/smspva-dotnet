$ErrorActionPreference = 'Stop'
$sdkRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$smokePath = Join-Path $sdkRoot '.checks/consumer'
New-Item -ItemType Directory -Force -Path $smokePath | Out-Null
@'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup><OutputType>Exe</OutputType><TargetFramework>net8.0</TargetFramework><Nullable>enable</Nullable><ImplicitUsings>enable</ImplicitUsings><TreatWarningsAsErrors>true</TreatWarningsAsErrors></PropertyGroup>
  <ItemGroup><PackageReference Include="Smspva.Sdk" Version="0.1.0-alpha.1" /></ItemGroup>
</Project>
'@ | Set-Content -LiteralPath (Join-Path $smokePath 'Consumer.csproj')
@'
<configuration><packageSources><clear/><add key="local" value="../../artifacts" /></packageSources></configuration>
'@ | Set-Content -LiteralPath (Join-Path $smokePath 'NuGet.Config')
@'
using System.Net;
using Smspva;
using var handler = new LocalHandler();
using var http = new HttpClient(handler);
using (var sdk = new SmspvaClient(httpClient:http)) {
    var result = await sdk.Email.GetDataAsync(new EmailGetDataRequest { Pcode = "example" });
    if (result.GetProperty("status").GetInt32() != 1) throw new Exception("Package call failed");
    try { await sdk.Activation.GetBalanceAsync(); throw new Exception("Missing key accepted"); }
    catch (ValidationException) { }
}
using (var second = new SmspvaClient(httpClient:http)) await second.Email.GetProvidersAsync();
using var stream = typeof(SmspvaClient).Assembly.GetManifestResourceStream("Smspva.contracts.json")!;
using var document = System.Text.Json.JsonDocument.Parse(stream);
if (document.RootElement.GetProperty("operations").EnumerateObject().Count() != 56) throw new Exception("Missing bundled contracts");
Console.WriteLine("Installed NuGet package: typed records, live-method dispatch, bundled contracts and injected HttpClient ownership OK; no production request made.");
sealed class LocalHandler : HttpMessageHandler {
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
        if (request.RequestUri!.Query.Contains("apikey")) throw new Exception("Public method leaked credentials");
        return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("{\"status\":1,\"data\":[]}") });
    }
}
'@ | Set-Content -LiteralPath (Join-Path $smokePath 'Program.cs')
dotnet run --project (Join-Path $smokePath 'Consumer.csproj')
if ($LASTEXITCODE -ne 0) { throw 'Installed-package smoke failed' }
