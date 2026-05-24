namespace Replenishment.Blazor.Models;

public class ReplenishmentRequestItemModel
{
    public Guid Id { get; set; }

    public string ArticleNumber { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int RequestedQuantity { get; set; }

    public int? FulfilledQuantity { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? RejectionReason { get; set; }

    public List<ReplenishmentRequestItemModel> Items { get; set; } = [];
}