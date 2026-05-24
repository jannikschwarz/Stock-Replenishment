using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Replenishment.Application.DTOs;
using Replenishment.Domain.Entities;
using Replenishment.Domain.Enums;
using Replenishment.Infrastructure.Persistence;

namespace Replenishment.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequestsController : ControllerBase
{
    private readonly AppDbContext _db;

    public RequestsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRequestDto dto)
    {
        var request = new ReplenishmentRequest
        {
            Id = Guid.NewGuid(),
            WorkerName = dto.WorkerName,
            StockLocation = dto.StockLocation,
            Priority = dto.Priority,
            Status = RequestStatus.Draft,
            CreatedAt = DateTime.UtcNow,

            Items = dto.Items.Select(x => new ReplenishmentRequestItem
            {
                Id = Guid.NewGuid(),
                ArticleNumber = x.ArticleNumber,
                Description = x.Description,
                RequestedQuantity = x.RequestedQuantity
            }).ToList()
        };

        _db.Requests.Add(request);

        await _db.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetById),
            new { id = request.Id },
            request);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var requests = await _db.Requests
            .Include(x => x.Items)
            .ToListAsync();

        return Ok(requests);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var request = await _db.Requests
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (request is null)
        {
            return NotFound();
        }

        return Ok(request);
    }
}