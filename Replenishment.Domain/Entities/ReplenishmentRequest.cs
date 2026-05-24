using Replenishment.Domain.Enums;

namespace Replenishment.Domain.Entities; 

public class ReplenishmentRequest
{
    public Guid Id { get; set; }

    public string WorkerName { get; set; } = string.Empty;

    public string StockLocation { get; set; } = string.Empty;

    public RequestStatus Status { get; set; } = RequestStatus.Draft;

    public RequestPriority Priority { get; set; } = RequestPriority.MEDIUM;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? RejectionReason { get; set; }

    public ICollection<ReplenishmentRequestItem> Items { get; set; }
        = new List<ReplenishmentRequestItem>();
}