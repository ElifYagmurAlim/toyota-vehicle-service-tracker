using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleServiceTracker.Domain.Entities;

namespace VehicleServiceTracker.Infrastructure.Persistence.Configurations;

public class ServiceEntryConfiguration : IEntityTypeConfiguration<ServiceEntry>
{
    public void Configure(EntityTypeBuilder<ServiceEntry> builder)
    {
        builder.ToTable("service_entries");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.LicensePlate)
            .HasColumnName("license_plate")
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(x => x.BrandName)
            .HasColumnName("brand_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.ModelName)
            .HasColumnName("model_name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(x => x.Kilometers)
            .HasColumnName("kilometers")
            .IsRequired();

        builder.Property(x => x.ModelYear)
            .HasColumnName("model_year");

        builder.Property(x => x.ServiceDate)
            .HasColumnName("service_date")
            .HasColumnType("timestamp with time zone")
            .IsRequired()
            .HasConversion(
                v => v.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    : v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        builder.Property(x => x.HasWarranty)
            .HasColumnName("has_warranty");

        builder.Property(x => x.ServiceCity)
            .HasColumnName("service_city")
            .HasMaxLength(100);

        builder.Property(x => x.ServiceNote)
            .HasColumnName("service_note")
            .HasMaxLength(1000);

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired()
            .HasConversion(
                v => v.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(v, DateTimeKind.Utc)
                    : v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp with time zone")
            .HasConversion(
                v => v.HasValue && v.Value.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc)
                    : v.HasValue ? v.Value.ToUniversalTime() : (DateTime?)null,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : (DateTime?)null);


        builder.HasIndex(x => x.LicensePlate);
        builder.HasIndex(x => x.ServiceDate);
    }
}