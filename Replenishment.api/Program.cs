using Microsoft.EntityFrameworkCore;
using Replenishment.Infrastructure.Persistence;
using Replenishment.Application.Services;
using Replenishment.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IRequestQueue, RequestQueue>();

builder.Services.AddScoped<IStockAvailabilityService,
    SlowStockAvailabilityService>();

builder.Services.AddHostedService<RequestProcessingService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ReplenishmentDb"));

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapControllers();

//Seeder / Data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    await DataSeeder.SeedAsync(db);
}

app.Run();
