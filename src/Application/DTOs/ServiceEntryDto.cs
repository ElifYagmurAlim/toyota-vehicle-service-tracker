namespace VehicleServiceTracker.Application.DTOs;

public record ServiceEntryDto
{
    public Guid Id { get; init; }
    public string LicensePlate { get; init; } = string.Empty;
    public string BrandName { get; init; } = string.Empty;
    public string ModelName { get; init; } = string.Empty;
    public int Kilometers { get; init; }
    public int? ModelYear { get; init; }
    public DateTime ServiceDate { get; init; }
    public bool? HasWarranty { get; init; }
    public string? ServiceCity { get; init; }
    public string? ServiceNote { get; init; }
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; init; }
}