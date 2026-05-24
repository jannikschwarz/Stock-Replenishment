using Replenishment.Application.Services;
using Replenishment.Domain.Entities;

namespace Replenishment.Infrastructure.Services;

public class SlowStockAvailabilityService : IStockAvailabilityService
{
    public async Task<bool> CheckAvailabilityAsync(
        ReplenishmentRequest request,
        CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"Checking stock for request {request.Id}");

        await Task.Delay(5000, cancellationToken);

        Console.WriteLine($"Finished stock check for request {request.Id}");

        return true;
    }
}