using Microsoft.EntityFrameworkCore;
using VehicleServiceTracker.Domain.Entities;
using VehicleServiceTracker.Domain.Interfaces;

namespace VehicleServiceTracker.Infrastructure.Persistence.Repositories;

public class ServiceEntryRepository : IServiceEntryRepository
{
    private readonly ApplicationDbContext _context;

    public ServiceEntryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceEntry?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ServiceEntries
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<(IEnumerable<ServiceEntry> Items, int TotalCount)> GetPagedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = _context.ServiceEntries.AsQueryable();

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.ServiceDate)
            .ThenByDescending(x => x.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<ServiceEntry> AddAsync(ServiceEntry serviceEntry, CancellationToken cancellationToken = default)
    {
        await _context.ServiceEntries.AddAsync(serviceEntry, cancellationToken);
        return serviceEntry;
    }

    public Task UpdateAsync(ServiceEntry serviceEntry, CancellationToken cancellationToken = default)
    {
        _context.ServiceEntries.Update(serviceEntry);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(ServiceEntry serviceEntry, CancellationToken cancellationToken = default)
    {
        _context.ServiceEntries.Remove(serviceEntry);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsByLicensePlateAndDateAsync(
    string licensePlate,
    DateTime serviceDate,
    CancellationToken cancellationToken = default)
    {
        // Ayn? gün içinde ayn? plaka var m? kontrol et
        var dateOnly = serviceDate.Date;
        var nextDay = dateOnly.AddDays(1);

        return await _context.ServiceEntries
            .AnyAsync(x =>
                x.LicensePlate == licensePlate.ToUpperInvariant() &&
                x.ServiceDate >= dateOnly &&
                x.ServiceDate < nextDay,
                cancellationToken);
    }
}