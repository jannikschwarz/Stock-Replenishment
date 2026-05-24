namespace Replenishment.Domain.Entities;

public class ReplenishmentRequestItem
{
    public Guid Id { get; set; }

    public Guid ReplenishmentRequestId { get; set; }

    public string ArticleNumber { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int RequestedQuantity { get; set; }

    public int? FulfilledQuantity { get; set; }
}