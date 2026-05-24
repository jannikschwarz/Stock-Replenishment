namespace Replenishment.Application.DTOs;

public class FulfillRequestDto
{
    public List<FulfillRequestItemDto> Items { get; set; } = [];
}