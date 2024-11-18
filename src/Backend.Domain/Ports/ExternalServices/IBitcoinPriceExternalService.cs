namespace Backend.Domain.Ports.ExternalServices;

public interface IBitcoinPriceExternalService
{
    Task<decimal> GetPriceAsync(CancellationToken cancellationToken = default);
}