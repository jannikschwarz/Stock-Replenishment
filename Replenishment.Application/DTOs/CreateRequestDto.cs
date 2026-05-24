using Replenishment.Domain.Enums;

namespace Replenishment.Application.DTOs;

public class CreateRequestDto
{
    public string WorkerName { get; set; } = string.Empty;

    public string StockLocation { get; set; } = string.Empty;

    public RequestPriority Priority { get; set; }

    public List<CreateRequestItemDto> Items { get; set; } = [];
}