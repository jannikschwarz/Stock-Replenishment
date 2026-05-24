using Replenishment.Domain.Entities;

namespace Replenishment.Application.Services;

public interface IStockAvailabilityService
{
    Task<bool> CheckAvailabilityAsync(
        ReplenishmentRequest request,
        CancellationToken cancellationToken = default);
}