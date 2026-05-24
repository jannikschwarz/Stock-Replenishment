namespace Replenishment.Application.DTOs;

public class FulfillRequestItemDto
{
    public Guid RequestItemId { get; set; }

    public int FulfilledQuantity { get; set; }
}