namespace Replenishment.Application.DTOs;

public class CreateRequestItemDto
{
    public string ArticleNumber { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int RequestedQuantity { get; set; }
}