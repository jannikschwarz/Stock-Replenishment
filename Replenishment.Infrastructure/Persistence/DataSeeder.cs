using Replenishment.Domain.Entities;
using Replenishment.Domain.Enums;

namespace Replenishment.Infrastructure.Persistence;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (db.Requests.Any())
        {
            return;
        }

        List<ReplenishmentRequest> requests =
        [
            new()
            {
                Id = Guid.NewGuid(),
                WorkerName = "Jonas",
                StockLocation = "Warehouse A",
                Priority = RequestPriority.Urgent,
                Status = RequestStatus.Draft,
                CreatedAt = DateTime.UtcNow.AddHours(-5),

                Items =
                [
                    new ReplenishmentRequestItem
                    {
                        Id = Guid.NewGuid(),
                        ArticleNumber = "ART-1001",
                        Description = "Safety Gloves",
                        RequestedQuantity = 10
                    },

                    new ReplenishmentRequestItem
                    {
                        Id = Guid.NewGuid(),
                        ArticleNumber = "ART-1002",
                        Description = "Safety Helmets",
                        RequestedQuantity = 5
                    }
                ]
            },

            new()
            {
                Id = Guid.NewGuid(),
                WorkerName = "Ruji",
                StockLocation = "Warehouse B",
                Priority = RequestPriority.MEDIUM,
                Status = RequestStatus.Submitted,
                CreatedAt = DateTime.UtcNow.AddHours(-3),

                Items =
                [
                    new ReplenishmentRequestItem
                    {
                        Id = Guid.NewGuid(),
                        ArticleNumber = "ART-2001",
                        Description = "Protective Glasses",
                        RequestedQuantity = 20
                    }
                ]
            },

            new()
            {
                Id = Guid.NewGuid(),
                WorkerName = "Jannik",
                StockLocation = "Warehouse C",
                Priority = RequestPriority.LOW,
                Status = RequestStatus.Approved,
                CreatedAt = DateTime.UtcNow.AddHours(-2),

                Items =
                [
                    new ReplenishmentRequestItem
                    {
                        Id = Guid.NewGuid(),
                        ArticleNumber = "ART-3001",
                        Description = "Work Boots",
                        RequestedQuantity = 15,
                        FulfilledQuantity = 15
                    }
                ]
            },

            new()
            {
                Id = Guid.NewGuid(),
                WorkerName = "Peter",
                StockLocation = "Warehouse A",
                Priority = RequestPriority.Urgent,
                Status = RequestStatus.Rejected,
                RejectionReason = "Insufficient justification",
                CreatedAt = DateTime.UtcNow.AddHours(-1),

                Items =
                [
                    new ReplenishmentRequestItem
                    {
                        Id = Guid.NewGuid(),
                        ArticleNumber = "ART-4001",
                        Description = "High Visibility Vests",
                        RequestedQuantity = 50
                    }
                ]
            },

            new()
            {
                Id = Guid.NewGuid(),
                WorkerName = "Chris",
                StockLocation = "Warehouse D",
                Priority = RequestPriority.MEDIUM,
                Status = RequestStatus.Fulfilled,
                CreatedAt = DateTime.UtcNow.AddMinutes(-30),

                Items =
                [
                    new ReplenishmentRequestItem
                    {
                        Id = Guid.NewGuid(),
                        ArticleNumber = "ART-5001",
                        Description = "Industrial Masks",
                        RequestedQuantity = 30,
                        FulfilledQuantity = 30
                    }
                ]
            }
        ];

        await db.Requests.AddRangeAsync(requests);

        await db.SaveChangesAsync();
    }
}