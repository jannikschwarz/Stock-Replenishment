namespace Replenishment.Blazor.Models;

public class ReplenishmentRequestModel
{
    public Guid Id { get; set; }

    public string WorkerName { get; set; } = string.Empty;

    public string StockLocation { get; set; } = string.Empty;

    public int Status { get; set;} = 0;

    public int Priority { get; set; } = 0;

    public string? RejectReason { get; set; } = string.Empty;

    public List<ReplenishmentRequestItemModel> Items { get; set; } = [];

    public DateTime CreatedAt { get; set; }
}