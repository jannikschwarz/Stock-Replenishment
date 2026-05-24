using System.Threading.Channels;
using Replenishment.Application.Services;

namespace Replenishment.Infrastructure.Services;

public class RequestQueue : IRequestQueue
{
    private readonly Channel<Guid> _queue = Channel.CreateUnbounded<Guid>();

    public async ValueTask QueueAsync(Guid requestId)
    {
        await _queue.Writer.WriteAsync(requestId);
    }

    public async ValueTask<Guid> DequeueAsync(
        CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}