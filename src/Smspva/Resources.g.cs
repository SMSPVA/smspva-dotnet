// Generated from sdk/contracts.json.
#nullable enable
using System.Text.Json;
namespace Smspva;

public sealed partial class ActivationResource
{
    private readonly SmspvaClient _client;
    internal ActivationResource(SmspvaClient client) => _client = client;
    /// <summary>GET /activation/balance. Read operation.</summary>
    public Task<JsonElement> GetBalanceAsync(RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("activation.getBalance", SmspvaClient.EmptyParameters, options, cancellationToken);

    /// <summary>GET /activation/userinfo. Read operation.</summary>
    public Task<JsonElement> GetUserInfoAsync(RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("activation.getUserInfo", SmspvaClient.EmptyParameters, options, cancellationToken);

    /// <summary>GET /activation/operators/{country}. Read operation.</summary>
    public Task<JsonElement> GetOperatorsAsync(ActivationGetOperatorsRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("activation.getOperators", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /activation/conversions/{service}. Read operation.</summary>
    public Task<JsonElement> GetConversionsAsync(ActivationGetConversionsRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("activation.getConversions", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /activation/countryservicesettings. Read operation.</summary>
    public Task<JsonElement> GetMessageTypeSettingsAsync(RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("activation.getMessageTypeSettings", SmspvaClient.EmptyParameters, options, cancellationToken);

    /// <summary>GET /activation/countnumbers. Read operation.</summary>
    public Task<JsonElement> GetAllNumberCountsAsync(RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("activation.getAllNumberCounts", SmspvaClient.EmptyParameters, options, cancellationToken);

    /// <summary>GET /activation/countnumbers/{country}. Read operation.</summary>
    public Task<JsonElement> GetCountryNumberCountsAsync(ActivationGetCountryNumberCountsRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("activation.getCountryNumberCounts", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /activation/serviceprice/{country}/{service}. Read operation.</summary>
    public Task<JsonElement> GetServicePriceAsync(ActivationGetServicePriceRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("activation.getServicePrice", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /activation/serviceprice/{country}. Read operation.</summary>
    public Task<JsonElement> GetCountryOperatorPricesAsync(ActivationGetCountryOperatorPricesRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("activation.getCountryOperatorPrices", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /activation/serviceprices/{service}. Read operation.</summary>
    public Task<JsonElement> GetServicePricesAsync(ActivationGetServicePricesRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("activation.getServicePrices", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /activation/servicesprices. Read operation.</summary>
    public Task<JsonElement> GetAllServicePricesAsync(ActivationGetAllServicePricesRequest? request = null, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("activation.getAllServicePrices", request?.ToParameters() ?? SmspvaClient.EmptyParameters, options, cancellationToken);

    /// <summary>GET /activation/number/{country}/{service}. Changes state or may incur charges.</summary>
    public Task<JsonElement> GetNumberAsync(ActivationGetNumberRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("activation.getNumber", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /activation/number/{country}/{service}/{operator}. Changes state or may incur charges.</summary>
    public Task<JsonElement> GetNumberByOperatorAsync(ActivationGetNumberByOperatorRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("activation.getNumberByOperator", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /activation/sms/{orderid}. Changes state or may incur charges.</summary>
    public Task<JsonElement> ReceiveSmsAsync(ActivationReceiveSmsRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("activation.receiveSms", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /activation/orders. Read operation.</summary>
    public Task<JsonElement> GetOrdersAsync(RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("activation.getOrders", SmspvaClient.EmptyParameters, options, cancellationToken);

    /// <summary>GET /activation/numberstatus/{number}/{service}. Read operation.</summary>
    public Task<JsonElement> GetNumberStatusAsync(ActivationGetNumberStatusRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("activation.getNumberStatus", request.ToParameters(), options, cancellationToken);

    /// <summary>PUT /activation/clearsms/{orderid}. Changes state or may incur charges.</summary>
    public Task<JsonElement> ClearSmsAsync(ActivationClearSmsRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("activation.clearSms", request.ToParameters(), options, cancellationToken);

    /// <summary>PUT /activation/stopsms/{orderid}. Changes state or may incur charges.</summary>
    public Task<JsonElement> StopSmsAsync(ActivationStopSmsRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("activation.stopSms", request.ToParameters(), options, cancellationToken);

    /// <summary>PUT /activation/cancelorder/{orderid}. Changes state or may incur charges.</summary>
    public Task<JsonElement> CancelOrderAsync(ActivationCancelOrderRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("activation.cancelOrder", request.ToParameters(), options, cancellationToken);

    /// <summary>PUT /activation/blocknumber/{orderid}. Changes state or may incur charges.</summary>
    public Task<JsonElement> BlockNumberAsync(ActivationBlockNumberRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("activation.blockNumber", request.ToParameters(), options, cancellationToken);

}

public sealed partial class RentalResource
{
    private readonly SmspvaClient _client;
    internal RentalResource(SmspvaClient client) => _client = client;
    /// <summary>GET /api/rent.php?method=getcountries. Read operation.</summary>
    public Task<JsonElement> GetCountriesAsync(RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("rental.getCountries", SmspvaClient.EmptyParameters, options, cancellationToken);

    /// <summary>GET /api/rent.php?method=get_default_services. Read operation.</summary>
    public Task<JsonElement> GetDefaultServicesAsync(RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("rental.getDefaultServices", SmspvaClient.EmptyParameters, options, cancellationToken);

    /// <summary>GET /api/rent.php?method=get_country_by_service. Read operation.</summary>
    public Task<JsonElement> GetCountriesByServiceAsync(RentalGetCountriesByServiceRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("rental.getCountriesByService", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /api/rent.php?method=getdata. Read operation.</summary>
    public Task<JsonElement> GetDataAsync(RentalGetDataRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("rental.getData", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /api/rent.php?method=getdataWithProviders. Read operation.</summary>
    public Task<JsonElement> GetDataWithProvidersAsync(RentalGetDataWithProvidersRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("rental.getDataWithProviders", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /api/rent.php?method=getcount. Read operation.</summary>
    public Task<JsonElement> GetCountAsync(RentalGetCountRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("rental.getCount", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /api/rent.php?method=getCountWithProviders. Read operation.</summary>
    public Task<JsonElement> GetCountWithProvidersAsync(RentalGetCountWithProvidersRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("rental.getCountWithProviders", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /api/rent.php?method=get_count_multi. Read operation.</summary>
    public Task<JsonElement> GetMultiServiceCountAsync(RentalGetMultiServiceCountRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("rental.getMultiServiceCount", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /api/rent.php?method=get_days_canada_paypal. Read operation.</summary>
    public Task<JsonElement> GetCanadaPaypalDaysAsync(RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("rental.getCanadaPaypalDays", SmspvaClient.EmptyParameters, options, cancellationToken);

    /// <summary>GET /api/rent.php?method=get_min_paypal_days. Read operation.</summary>
    public Task<JsonElement> GetMinimumPaypalDaysAsync(RentalGetMinimumPaypalDaysRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("rental.getMinimumPaypalDays", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /api/rent.php?method=create. Changes state or may incur charges.</summary>
    public Task<JsonElement> CreateAsync(RentalCreateRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("rental.create", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /api/rent.php?method=create_multi. Changes state or may incur charges.</summary>
    public Task<JsonElement> CreateMultiAsync(RentalCreateMultiRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("rental.createMulti", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /api/rent.php?method=add_service_to_order. Changes state or may incur charges.</summary>
    public Task<JsonElement> AddServiceToOrderAsync(RentalAddServiceToOrderRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("rental.addServiceToOrder", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /api/rent.php?method=orders. Read operation.</summary>
    public Task<JsonElement> GetOrdersAsync(RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("rental.getOrders", SmspvaClient.EmptyParameters, options, cancellationToken);

    /// <summary>GET /api/rent.php?method=activate. Changes state or may incur charges.</summary>
    public Task<JsonElement> ActivateAsync(RentalActivateRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("rental.activate", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /api/rent.php?method=sms. Read operation.</summary>
    public Task<JsonElement> GetSmsAsync(RentalGetSmsRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("rental.getSms", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /api/rent.php?method=prolong. Changes state or may incur charges.</summary>
    public Task<JsonElement> ProlongAsync(RentalProlongRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("rental.prolong", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /api/rent.php?method=prolong_max. Changes state or may incur charges.</summary>
    public Task<JsonElement> ProlongByDaysAsync(RentalProlongByDaysRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("rental.prolongByDays", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /api/rent.php?method=delete. Changes state or may incur charges.</summary>
    public Task<JsonElement> DeleteOrderAsync(RentalDeleteOrderRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("rental.deleteOrder", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /api/rent.php?method=get_rent_history. Read operation.</summary>
    public Task<JsonElement> GetHistoryAsync(RentalGetHistoryRequest? request = null, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("rental.getHistory", request?.ToParameters() ?? SmspvaClient.EmptyParameters, options, cancellationToken);

    /// <summary>GET /api/rent.php?method=restore_user_precalc. Read operation.</summary>
    public Task<JsonElement> GetRestoreCostAsync(RentalGetRestoreCostRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("rental.getRestoreCost", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /api/rent.php?method=restore_user. Changes state or may incur charges.</summary>
    public Task<JsonElement> RestoreAsync(RentalRestoreRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("rental.restore", request.ToParameters(), options, cancellationToken);

}

public sealed partial class EmailResource
{
    private readonly SmspvaClient _client;
    internal EmailResource(SmspvaClient client) => _client = client;
    /// <summary>GET /api/rent_email.php?method=get_providers. Read operation.</summary>
    public Task<JsonElement> GetProvidersAsync(RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("email.getProviders", SmspvaClient.EmptyParameters, options, cancellationToken);

    /// <summary>GET /api/rent_email.php?method=get_data. Read operation.</summary>
    public Task<JsonElement> GetDataAsync(EmailGetDataRequest? request = null, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("email.getData", request?.ToParameters() ?? SmspvaClient.EmptyParameters, options, cancellationToken);

    /// <summary>GET /api/rent_email.php?method=get_count. Read operation.</summary>
    public Task<JsonElement> GetCountAsync(EmailGetCountRequest? request = null, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("email.getCount", request?.ToParameters() ?? SmspvaClient.EmptyParameters, options, cancellationToken);

    /// <summary>GET /api/rent_email.php?method=create_order. Changes state or may incur charges.</summary>
    public Task<JsonElement> CreateOrderAsync(EmailCreateOrderRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("email.createOrder", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /api/rent_email.php?method=get_orders. Read operation.</summary>
    public Task<JsonElement> GetOrdersAsync(RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("email.getOrders", SmspvaClient.EmptyParameters, options, cancellationToken);

    /// <summary>GET /api/rent_email.php?method=get_mails. Read operation.</summary>
    public Task<JsonElement> GetMailsAsync(EmailGetMailsRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("email.getMails", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /api/rent_email.php?method=get_mail_body. Read operation.</summary>
    public Task<JsonElement> GetMailBodyAsync(EmailGetMailBodyRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("email.getMailBody", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /api/rent_email.php?method=buy_alias. Changes state or may incur charges.</summary>
    public Task<JsonElement> BuyAliasAsync(EmailBuyAliasRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("email.buyAlias", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /api/rent_email.php?method=get_mails_by_alias. Read operation.</summary>
    public Task<JsonElement> GetMailsByAliasAsync(EmailGetMailsByAliasRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("email.getMailsByAlias", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /api/rent_email.php?method=prolong_order. Changes state or may incur charges.</summary>
    public Task<JsonElement> ProlongOrderAsync(EmailProlongOrderRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("email.prolongOrder", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /api/rent_email.php?method=cancel_order. Changes state or may incur charges.</summary>
    public Task<JsonElement> CancelOrderAsync(EmailCancelOrderRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("email.cancelOrder", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /api/rent_email.php?method=get_archived_orders. Read operation.</summary>
    public Task<JsonElement> GetArchivedOrdersAsync(RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("email.getArchivedOrders", SmspvaClient.EmptyParameters, options, cancellationToken);

    /// <summary>GET /api/rent_email.php?method=calc_restore_order. Read operation.</summary>
    public Task<JsonElement> CalcRestoreOrderAsync(EmailCalcRestoreOrderRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("email.calcRestoreOrder", request.ToParameters(), options, cancellationToken);

    /// <summary>GET /api/rent_email.php?method=restore_order. Changes state or may incur charges.</summary>
    public Task<JsonElement> RestoreOrderAsync(EmailRestoreOrderRequest request, RequestOptions? options = null, CancellationToken cancellationToken = default)
        => _client.RequestAsync("email.restoreOrder", request.ToParameters(), options, cancellationToken);

}
