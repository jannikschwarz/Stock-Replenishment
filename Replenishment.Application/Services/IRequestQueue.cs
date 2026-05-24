namespace Replenishment.Application.Services;

public interface IRequestQueue
{
    ValueTask QueueAsync(Guid requestId);

    ValueTask<Guid> DequeueAsync(CancellationToken cancellationToken);
}