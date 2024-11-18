using System.Text.Json;
using Backend.Domain.Ports.ExternalServices;
using Microsoft.Extensions.Configuration;

namespace Backend.ExternalServices.Adapter;

public class BitcoinPriceExternalService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    : IBitcoinPriceExternalService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(nameof(IBitcoinPriceExternalService));

    private readonly string _uri = configuration.GetValue<string>("BitcoinPriceExternalService:Uri") ??
                                   throw new ArgumentNullException(nameof(configuration));

    public async Task<decimal> GetPriceAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(_uri, cancellationToken);

        var serializedResponse = await response.Content.ReadAsStringAsync(cancellationToken);
        var deserializedResponse = System.Text.Json.JsonSerializer.Deserialize<BitcoinPriceModel>(serializedResponse,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        return deserializedResponse?.Ticker?.High ?? 0;
    }

    private class BitcoinPriceModel
    {
        internal TickerModel? Ticker { get; init; }

        internal class TickerModel
        {
            public decimal High { get; set; }
            public decimal Low { get; set; }
            public decimal Vol { get; set; }
            public decimal Last { get; set; }
            public decimal Buy { get; set; }
            public decimal Sell { get; set; }
            public decimal Open { get; set; }
            public long Date { get; set; }
            public string Pair { get; set; }
        }
    }
}