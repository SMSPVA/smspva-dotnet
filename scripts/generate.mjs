import { readFile, writeFile, mkdir } from 'node:fs/promises';
const root = new URL('../', import.meta.url);
const manifest = JSON.parse(await readFile(new URL('../tests/contracts.json', import.meta.url), 'utf8'));
const pascal = value => value[0].toUpperCase() + value.slice(1);
const property = name => ({orderid:'OrderId',partnerkey:'PartnerKey',partner_key:'PartnerKey'}[name] ?? name.split('_').map(pascal).join(''));
const type = p => p.schema.type === 'integer' ? 'long' : 'string';
let requests = '// Generated from sdk/contracts.json.\n#nullable enable\nusing System.Text.Json.Serialization;\nnamespace Smspva;\n\n';
let resources = '// Generated from sdk/contracts.json.\n#nullable enable\nusing System.Text.Json;\nnamespace Smspva;\n\n';
for (const family of ['activation','rental','email']) {
  resources += `public sealed partial class ${pascal(family)}Resource\n{\n    private readonly SmspvaClient _client;\n    internal ${pascal(family)}Resource(SmspvaClient client) => _client = client;\n`;
  for (const [key,op] of Object.entries(manifest.operations).filter(([,v]) => v.family === family)) {
    const request = pascal(family)+pascal(op.jsName)+'Request';
    const required = op.params.filter(p=>p.required), optional = op.params.filter(p=>!p.required);
    if(op.params.length) {
      requests += `public sealed record ${request}(${required.map(p=>`[property: JsonPropertyName(${JSON.stringify(p.name)})] ${type(p)} ${property(p.name)}`).join(', ')}) : ISmspvaRequest\n{\n`;
      for(const p of optional) requests += `    [JsonPropertyName(${JSON.stringify(p.name)})] public ${type(p)}? ${property(p.name)} { get; init; }\n`;
      requests += `    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>\n    {\n${op.params.map(p=>`        [${JSON.stringify(p.name)}] = ${property(p.name)},`).join('\n')}\n    };\n}\n\n`;
    }
    resources += `    /// <summary>${op.method} ${op.path}${op.action?'?method='+op.action:''}. ${op.stateful?'Changes state or may incur charges.':'Read operation.'}</summary>\n`;
    const argument = op.params.length ? `${request}${required.length?'':'?'} request${required.length?'':' = null'}, ` : '';
    const values = op.params.length ? (required.length?'request.ToParameters()':'request?.ToParameters() ?? SmspvaClient.EmptyParameters') : 'SmspvaClient.EmptyParameters';
    resources += `    public Task<JsonElement> ${pascal(op.jsName)}Async(${argument}RequestOptions? options = null, CancellationToken cancellationToken = default)\n        => _client.RequestAsync(${JSON.stringify(key)}, ${values}, options, cancellationToken);\n\n`;
  }
  resources += '}\n\n';
}
await mkdir(new URL('src/Smspva/',root),{recursive:true});
await writeFile(new URL('src/Smspva/Requests.g.cs',root), requests);
await writeFile(new URL('src/Smspva/Resources.g.cs',root), resources);
await writeFile(new URL('src/Smspva/contracts.json',root), JSON.stringify(manifest,null,2)+'\n');
console.log(`Generated ${Object.keys(manifest.operations).length} .NET methods and typed request records.`);
