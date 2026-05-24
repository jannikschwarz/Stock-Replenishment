using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Replenishment.Application.Services;
using Replenishment.Infrastructure.Persistence;

namespace Replenishment.Infrastructure.Services;

public class RequestProcessingService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IRequestQueue _queue;

    public RequestProcessingService(
        IServiceProvider serviceProvider,
        IRequestQueue queue)
    {
        _serviceProvider = serviceProvider;
        _queue = queue;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var requestId = await _queue.DequeueAsync(stoppingToken);

            using var scope = _serviceProvider.CreateScope();

            var db = scope.ServiceProvider
                .GetRequiredService<AppDbContext>();

            var stockService = scope.ServiceProvider
                .GetRequiredService<IStockAvailabilityService>();

            var request = await db.Requests
                .Include(x => x.Items)
                .FirstOrDefaultAsync(
                    x => x.Id == requestId,
                    stoppingToken);

            if (request == null)
                continue;
            

            await stockService.CheckAvailabilityAsync(request,stoppingToken);

            Console.WriteLine($"Processing completed for request {request.WorkerName}:{request.Id}");
        }
    }
}