// Generated from sdk/contracts.json.
#nullable enable
using System.Text.Json.Serialization;
namespace Smspva;

public sealed record ActivationGetOperatorsRequest([property: JsonPropertyName("country")] string Country) : ISmspvaRequest
{
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["country"] = Country,
    };
}

public sealed record ActivationGetConversionsRequest([property: JsonPropertyName("service")] string Service) : ISmspvaRequest
{
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["service"] = Service,
    };
}

public sealed record ActivationGetCountryNumberCountsRequest([property: JsonPropertyName("country")] string Country) : ISmspvaRequest
{
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["country"] = Country,
    };
}

public sealed record ActivationGetServicePriceRequest([property: JsonPropertyName("country")] string Country, [property: JsonPropertyName("service")] string Service) : ISmspvaRequest
{
    [JsonPropertyName("voice")] public long? Voice { get; init; }
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["country"] = Country,
        ["service"] = Service,
        ["voice"] = Voice,
    };
}

public sealed record ActivationGetCountryOperatorPricesRequest([property: JsonPropertyName("country")] string Country) : ISmspvaRequest
{
    [JsonPropertyName("voice")] public long? Voice { get; init; }
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["country"] = Country,
        ["voice"] = Voice,
    };
}

public sealed record ActivationGetServicePricesRequest([property: JsonPropertyName("service")] string Service) : ISmspvaRequest
{
    [JsonPropertyName("voice")] public long? Voice { get; init; }
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["service"] = Service,
        ["voice"] = Voice,
    };
}

public sealed record ActivationGetAllServicePricesRequest() : ISmspvaRequest
{
    [JsonPropertyName("voice")] public long? Voice { get; init; }
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["voice"] = Voice,
    };
}

public sealed record ActivationGetNumberRequest([property: JsonPropertyName("country")] string Country, [property: JsonPropertyName("service")] string Service) : ISmspvaRequest
{
    [JsonPropertyName("voice")] public long? Voice { get; init; }
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["country"] = Country,
        ["service"] = Service,
        ["voice"] = Voice,
    };
}

public sealed record ActivationGetNumberByOperatorRequest([property: JsonPropertyName("country")] string Country, [property: JsonPropertyName("service")] string Service, [property: JsonPropertyName("operator")] string Operator) : ISmspvaRequest
{
    [JsonPropertyName("voice")] public long? Voice { get; init; }
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["country"] = Country,
        ["service"] = Service,
        ["operator"] = Operator,
        ["voice"] = Voice,
    };
}

public sealed record ActivationReceiveSmsRequest([property: JsonPropertyName("orderid")] long OrderId) : ISmspvaRequest
{
    [JsonPropertyName("partnerkey")] public string? PartnerKey { get; init; }
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["partnerkey"] = PartnerKey,
        ["orderid"] = OrderId,
    };
}

public sealed record ActivationGetNumberStatusRequest([property: JsonPropertyName("number")] string Number, [property: JsonPropertyName("service")] string Service) : ISmspvaRequest
{
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["number"] = Number,
        ["service"] = Service,
    };
}

public sealed record ActivationClearSmsRequest([property: JsonPropertyName("orderid")] long OrderId) : ISmspvaRequest
{
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["orderid"] = OrderId,
    };
}

public sealed record ActivationStopSmsRequest([property: JsonPropertyName("orderid")] long OrderId) : ISmspvaRequest
{
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["orderid"] = OrderId,
    };
}

public sealed record ActivationCancelOrderRequest([property: JsonPropertyName("orderid")] long OrderId) : ISmspvaRequest
{
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["orderid"] = OrderId,
    };
}

public sealed record ActivationBlockNumberRequest([property: JsonPropertyName("orderid")] long OrderId) : ISmspvaRequest
{
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["orderid"] = OrderId,
    };
}

public sealed record RentalGetCountriesByServiceRequest([property: JsonPropertyName("service")] string Service, [property: JsonPropertyName("dtype")] string Dtype) : ISmspvaRequest
{
    [JsonPropertyName("dcount")] public long? Dcount { get; init; }
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["service"] = Service,
        ["dtype"] = Dtype,
        ["dcount"] = Dcount,
    };
}

public sealed record RentalGetDataRequest([property: JsonPropertyName("country")] string Country) : ISmspvaRequest
{
    [JsonPropertyName("dtype")] public string? Dtype { get; init; }
    [JsonPropertyName("dcount")] public long? Dcount { get; init; }
    [JsonPropertyName("extend")] public string? Extend { get; init; }
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["country"] = Country,
        ["dtype"] = Dtype,
        ["dcount"] = Dcount,
        ["extend"] = Extend,
    };
}

public sealed record RentalGetDataWithProvidersRequest([property: JsonPropertyName("country")] string Country) : ISmspvaRequest
{
    [JsonPropertyName("dtype")] public string? Dtype { get; init; }
    [JsonPropertyName("dcount")] public long? Dcount { get; init; }
    [JsonPropertyName("extend")] public string? Extend { get; init; }
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["country"] = Country,
        ["dtype"] = Dtype,
        ["dcount"] = Dcount,
        ["extend"] = Extend,
    };
}

public sealed record RentalGetCountRequest([property: JsonPropertyName("country")] string Country) : ISmspvaRequest
{
    [JsonPropertyName("dtype")] public string? Dtype { get; init; }
    [JsonPropertyName("dcount")] public long? Dcount { get; init; }
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["country"] = Country,
        ["dtype"] = Dtype,
        ["dcount"] = Dcount,
    };
}

public sealed record RentalGetCountWithProvidersRequest([property: JsonPropertyName("country")] string Country) : ISmspvaRequest
{
    [JsonPropertyName("dtype")] public string? Dtype { get; init; }
    [JsonPropertyName("dcount")] public long? Dcount { get; init; }
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["country"] = Country,
        ["dtype"] = Dtype,
        ["dcount"] = Dcount,
    };
}

public sealed record RentalGetMultiServiceCountRequest([property: JsonPropertyName("country")] string Country, [property: JsonPropertyName("services")] string Services, [property: JsonPropertyName("dtype")] string Dtype, [property: JsonPropertyName("dcount")] long Dcount) : ISmspvaRequest
{
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["country"] = Country,
        ["services"] = Services,
        ["dtype"] = Dtype,
        ["dcount"] = Dcount,
    };
}

public sealed record RentalGetMinimumPaypalDaysRequest([property: JsonPropertyName("country")] string Country, [property: JsonPropertyName("service")] string Service) : ISmspvaRequest
{
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["country"] = Country,
        ["service"] = Service,
    };
}

public sealed record RentalCreateRequest([property: JsonPropertyName("dtype")] string Dtype, [property: JsonPropertyName("dcount")] long Dcount, [property: JsonPropertyName("country")] string Country, [property: JsonPropertyName("service")] string Service) : ISmspvaRequest
{
    [JsonPropertyName("provider")] public string? Provider { get; init; }
    [JsonPropertyName("partner_key")] public string? PartnerKey { get; init; }
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["dtype"] = Dtype,
        ["dcount"] = Dcount,
        ["country"] = Country,
        ["service"] = Service,
        ["provider"] = Provider,
        ["partner_key"] = PartnerKey,
    };
}

public sealed record RentalCreateMultiRequest([property: JsonPropertyName("dtype")] string Dtype, [property: JsonPropertyName("dcount")] long Dcount, [property: JsonPropertyName("country")] string Country, [property: JsonPropertyName("services")] string Services) : ISmspvaRequest
{
    [JsonPropertyName("provider")] public string? Provider { get; init; }
    [JsonPropertyName("partner_key")] public string? PartnerKey { get; init; }
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["dtype"] = Dtype,
        ["dcount"] = Dcount,
        ["country"] = Country,
        ["services"] = Services,
        ["provider"] = Provider,
        ["partner_key"] = PartnerKey,
    };
}

public sealed record RentalAddServiceToOrderRequest([property: JsonPropertyName("id")] long Id, [property: JsonPropertyName("service")] string Service, [property: JsonPropertyName("pnumber")] string Pnumber) : ISmspvaRequest
{
    [JsonPropertyName("rent_days")] public long? RentDays { get; init; }
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["id"] = Id,
        ["service"] = Service,
        ["pnumber"] = Pnumber,
        ["rent_days"] = RentDays,
    };
}

public sealed record RentalActivateRequest([property: JsonPropertyName("id")] long Id) : ISmspvaRequest
{
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["id"] = Id,
    };
}

public sealed record RentalGetSmsRequest([property: JsonPropertyName("id")] long Id) : ISmspvaRequest
{
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["id"] = Id,
    };
}

public sealed record RentalProlongRequest([property: JsonPropertyName("id")] long Id, [property: JsonPropertyName("dtype")] string Dtype, [property: JsonPropertyName("dcount")] long Dcount) : ISmspvaRequest
{
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["id"] = Id,
        ["dtype"] = Dtype,
        ["dcount"] = Dcount,
    };
}

public sealed record RentalProlongByDaysRequest([property: JsonPropertyName("id")] long Id, [property: JsonPropertyName("dtype")] string Dtype, [property: JsonPropertyName("dcount")] long Dcount) : ISmspvaRequest
{
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["id"] = Id,
        ["dtype"] = Dtype,
        ["dcount"] = Dcount,
    };
}

public sealed record RentalDeleteOrderRequest([property: JsonPropertyName("id")] long Id) : ISmspvaRequest
{
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["id"] = Id,
    };
}

public sealed record RentalGetHistoryRequest() : ISmspvaRequest
{
    [JsonPropertyName("skip")] public long? Skip { get; init; }
    [JsonPropertyName("take")] public long? Take { get; init; }
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["skip"] = Skip,
        ["take"] = Take,
    };
}

public sealed record RentalGetRestoreCostRequest([property: JsonPropertyName("id")] long Id) : ISmspvaRequest
{
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["id"] = Id,
    };
}

public sealed record RentalRestoreRequest([property: JsonPropertyName("id")] long Id) : ISmspvaRequest
{
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["id"] = Id,
    };
}

public sealed record EmailGetDataRequest() : ISmspvaRequest
{
    [JsonPropertyName("pcode")] public string? Pcode { get; init; }
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["pcode"] = Pcode,
    };
}

public sealed record EmailGetCountRequest() : ISmspvaRequest
{
    [JsonPropertyName("pcode")] public string? Pcode { get; init; }
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["pcode"] = Pcode,
    };
}

public sealed record EmailCreateOrderRequest([property: JsonPropertyName("scode")] string Scode, [property: JsonPropertyName("dtype")] string Dtype, [property: JsonPropertyName("dcount")] long Dcount) : ISmspvaRequest
{
    [JsonPropertyName("pcode")] public string? Pcode { get; init; }
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["scode"] = Scode,
        ["pcode"] = Pcode,
        ["dtype"] = Dtype,
        ["dcount"] = Dcount,
    };
}

public sealed record EmailGetMailsRequest([property: JsonPropertyName("orderid")] long OrderId) : ISmspvaRequest
{
    [JsonPropertyName("t")] public long? T { get; init; }
    [JsonPropertyName("s")] public long? S { get; init; }
    [JsonPropertyName("sq")] public string? Sq { get; init; }
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["orderid"] = OrderId,
        ["t"] = T,
        ["s"] = S,
        ["sq"] = Sq,
    };
}

public sealed record EmailGetMailBodyRequest([property: JsonPropertyName("orderid")] long OrderId, [property: JsonPropertyName("emailid")] long Emailid) : ISmspvaRequest
{
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["orderid"] = OrderId,
        ["emailid"] = Emailid,
    };
}

public sealed record EmailBuyAliasRequest([property: JsonPropertyName("orderid")] long OrderId, [property: JsonPropertyName("aliasid")] long Aliasid) : ISmspvaRequest
{
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["orderid"] = OrderId,
        ["aliasid"] = Aliasid,
    };
}

public sealed record EmailGetMailsByAliasRequest([property: JsonPropertyName("orderid")] long OrderId, [property: JsonPropertyName("aliasid")] long Aliasid) : ISmspvaRequest
{
    [JsonPropertyName("t")] public long? T { get; init; }
    [JsonPropertyName("s")] public long? S { get; init; }
    [JsonPropertyName("sq")] public string? Sq { get; init; }
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["orderid"] = OrderId,
        ["aliasid"] = Aliasid,
        ["t"] = T,
        ["s"] = S,
        ["sq"] = Sq,
    };
}

public sealed record EmailProlongOrderRequest([property: JsonPropertyName("orderid")] long OrderId, [property: JsonPropertyName("dtype")] string Dtype, [property: JsonPropertyName("dcount")] long Dcount) : ISmspvaRequest
{
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["orderid"] = OrderId,
        ["dtype"] = Dtype,
        ["dcount"] = Dcount,
    };
}

public sealed record EmailCancelOrderRequest([property: JsonPropertyName("orderid")] long OrderId) : ISmspvaRequest
{
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["orderid"] = OrderId,
    };
}

public sealed record EmailCalcRestoreOrderRequest([property: JsonPropertyName("orderid")] long OrderId) : ISmspvaRequest
{
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["orderid"] = OrderId,
    };
}

public sealed record EmailRestoreOrderRequest([property: JsonPropertyName("orderid")] long OrderId) : ISmspvaRequest
{
    public IReadOnlyDictionary<string, object?> ToParameters() => new Dictionary<string, object?>
    {
        ["orderid"] = OrderId,
    };
}
