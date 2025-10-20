using VehicleServiceTracker.Domain.Common;

namespace VehicleServiceTracker.Domain.Entities;

public class ServiceEntry : BaseEntity
{
    public string LicensePlate { get; private set; }
    public string BrandName { get; private set; }
    public string ModelName { get; private set; }
    public int Kilometers { get; private set; }
    public int? ModelYear { get; private set; }
    public DateTime ServiceDate { get; private set; }
    public bool? HasWarranty { get; private set; }
    public string? ServiceCity { get; private set; }
    public string? ServiceNote { get; private set; }

    // EF Core için parametre olmayan constructor
    private ServiceEntry() { }

    public ServiceEntry(
        string licensePlate,
        string brandName,
        string modelName,
        int kilometers,
        DateTime serviceDate,
        int? modelYear = null,
        bool? hasWarranty = null,
        string? serviceCity = null,
        string? serviceNote = null)
    {
        ValidateAndSetProperties(licensePlate, brandName, modelName, kilometers, serviceDate);
        
        ModelYear = modelYear;
        HasWarranty = hasWarranty;
        ServiceCity = serviceCity;
        ServiceNote = serviceNote;
    }

    public void Update(
        string licensePlate,
        string brandName,
        string modelName,
        int kilometers,
        DateTime serviceDate,
        int? modelYear = null,
        bool? hasWarranty = null,
        string? serviceCity = null,
        string? serviceNote = null)
    {
        ValidateAndSetProperties(licensePlate, brandName, modelName, kilometers, serviceDate);
        
        ModelYear = modelYear;
        HasWarranty = hasWarranty;
        ServiceCity = serviceCity;
        ServiceNote = serviceNote;
        UpdatedAt = DateTime.UtcNow;
    }

    private void ValidateAndSetProperties(
        string licensePlate,
        string brandName,
        string modelName,
        int kilometers,
        DateTime serviceDate)
    {
        if (string.IsNullOrWhiteSpace(licensePlate))
            throw new ArgumentException("Araç plakası zorunludur.", nameof(licensePlate));

        if (string.IsNullOrWhiteSpace(brandName))
            throw new ArgumentException("Marka adı zorunludur.", nameof(brandName));

        if (string.IsNullOrWhiteSpace(modelName))
            throw new ArgumentException("Model adı zorunludur.", nameof(modelName));

        if (kilometers < 0)
            throw new ArgumentException("KM bilgisi negatif olamaz.", nameof(kilometers));

        LicensePlate = licensePlate.Trim().ToUpperInvariant();
        BrandName = brandName.Trim();
        ModelName = modelName.Trim();
        Kilometers = kilometers;
        ServiceDate = serviceDate;

        ServiceDate = serviceDate.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(serviceDate, DateTimeKind.Utc)
            : serviceDate.ToUniversalTime();
    }
}