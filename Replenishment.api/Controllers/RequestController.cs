using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Replenishment.Application.DTOs;
using Replenishment.Application.Services;
using Replenishment.Domain.Entities;
using Replenishment.Domain.Enums;
using Replenishment.Infrastructure.Persistence;

namespace Replenishment.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequestsController : ControllerBase
{
    private readonly AppDbContext _db;

    private readonly IRequestQueue _queue;

    public RequestsController(
        AppDbContext db,
        IRequestQueue queue)
    {
        _db = db;
        _queue = queue;
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
    public async Task<IActionResult> GetAll(
        [FromQuery] RequestStatus? status,
        [FromQuery] RequestPriority? priority,
        [FromQuery] string? location
    )
    {
        var query = _db.Requests.Include(x => x.Items).AsQueryable();

        if(status.HasValue)
            query = query.Where(x => x.Status == status.Value);
        if(priority.HasValue)
            query = query.Where(x => x.Priority == priority.Value);
        if(!string.IsNullOrWhiteSpace(location))
            query = query.Where(x => x.StockLocation.Contains(location));

        var requests = await query.OrderByDescending(x => x.CreatedAt).ToListAsync();

        return Ok(requests);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var request = await _db.Requests
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (request == null)
            return NotFound();
        
        return Ok(request);
    }

    [HttpPost("{id:guid}/submit")]
    public async Task<IActionResult> Submit(Guid id)
    {
        var request = await _db.Requests
            .FirstOrDefaultAsync(x => x.Id == id);

        if (request == null)
            return NotFound();

        if (request.Status != RequestStatus.Draft)
            return BadRequest("Only draft requests can be submitted");

        request.Status = RequestStatus.Submitted;

        await _db.SaveChangesAsync();
        await _queue.QueueAsync(request.Id);

        return Accepted(new { Message = "Request submitted successfully" });
    }

    [HttpPost("{id:guid}/approve")]
    public async Task<IActionResult> Approve(Guid id)
    {
        var request = await _db.Requests.FirstOrDefaultAsync(x => x.Id == id);

        if(request == null)
            return NotFound();

        if(request.Status != RequestStatus.Submitted)
            return BadRequest("Only submitted requests can be approved");

        request.Status = RequestStatus.Approved;

        await _db.SaveChangesAsync();

        return Ok(new {Message = "Request approved"});
    }

    [HttpPost("{id:guid}/reject")]
    public async Task<IActionResult> Reject(Guid id, RejectRequestDto dto)
    {
        var request = await _db.Requests.FirstOrDefaultAsync(x => x.Id == id);

        if(request == null)
            return NotFound();

        if(request.Status != RequestStatus.Submitted)
            return BadRequest("Only submitted requests can be rejected");

        request.Status = RequestStatus.Rejected;
        request.RejectionReason = dto.Reason;

        await _db.SaveChangesAsync();

        return Ok(new {Message = "Request rejected"});
    }

    [HttpPost("{id:guid}/fulfill")]
    public async Task<IActionResult> FulFill(Guid id, FulfillRequestDto dto)
    {
        var request = await _db.Requests.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == id);


        if(request == null)
            return NotFound();

        if(request.Status != RequestStatus.Approved)
            return BadRequest("Only approved requests can be fulfilled");

        int overflow = 0;
        foreach(var itemDto in dto.Items)
        {
            var item = request.Items.FirstOrDefault(x => x.Id == itemDto.RequestItemId);
            if(item == null || itemDto == null)
                continue;

            int missingQuantity = item.RequestedQuantity - itemDto.FulfilledQuantity;

            if(missingQuantity > itemDto.FulfilledQuantity)
                item.FulfilledQuantity += itemDto.FulfilledQuantity;
            else if(missingQuantity == itemDto.FulfilledQuantity)
                item.FulfilledQuantity = itemDto.FulfilledQuantity;
            else if(missingQuantity < itemDto.FulfilledQuantity)
            {
                item.FulfilledQuantity = item.RequestedQuantity;
                overflow += itemDto.FulfilledQuantity - missingQuantity;
            }else
                item.FulfilledQuantity = item.RequestedQuantity;
        }

        bool requestFulfilled = true; 
        foreach(var item in request.Items)
        {
            if(item.RequestedQuantity != item.FulfilledQuantity)
            {
                requestFulfilled = false; 
                Console.WriteLine($"A request item has not yet been fulfilled {item.Description} {item.RequestedQuantity}:{item.FulfilledQuantity}");
            }
        }

        if(requestFulfilled)
            request.Status = RequestStatus.Fulfilled;

        await _db.SaveChangesAsync();

        return Ok(new 
        {
            Message = "Request fulfilled",
            Overflow = overflow
        });
    }
}