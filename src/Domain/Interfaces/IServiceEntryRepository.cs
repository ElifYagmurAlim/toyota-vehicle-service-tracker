using VehicleServiceTracker.Domain.Entities;

namespace VehicleServiceTracker.Domain.Interfaces;

public interface IServiceEntryRepository
{
    Task<ServiceEntry?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<(IEnumerable<ServiceEntry> Items, int TotalCount)> GetPagedAsync(
        int pageNumber, 
        int pageSize, 
        CancellationToken cancellationToken = default);
    Task<ServiceEntry> AddAsync(ServiceEntry serviceEntry, CancellationToken cancellationToken = default);
    Task UpdateAsync(ServiceEntry serviceEntry, CancellationToken cancellationToken = default);
    Task DeleteAsync(ServiceEntry serviceEntry, CancellationToken cancellationToken = default);

    Task<bool> ExistsByLicensePlateAndDateAsync(
        string licensePlate,
        DateTime serviceDate,
        CancellationToken cancellationToken = default);
}