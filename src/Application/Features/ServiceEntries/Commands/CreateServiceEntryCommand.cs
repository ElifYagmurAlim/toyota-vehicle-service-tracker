using MediatR;
using Microsoft.Extensions.Logging;
using VehicleServiceTracker.Application.Common.Models;
using VehicleServiceTracker.Application.DTOs;
using VehicleServiceTracker.Domain.Entities;
using VehicleServiceTracker.Domain.Interfaces;

namespace VehicleServiceTracker.Application.Features.ServiceEntries.Commands;

public record CreateServiceEntryCommand(CreateServiceEntryDto Data) 
    : IRequest<Result<ServiceEntryDto>>;

public class CreateServiceEntryCommandHandler 
    : IRequestHandler<CreateServiceEntryCommand, Result<ServiceEntryDto>>
{
    private readonly IServiceEntryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateServiceEntryCommandHandler> _logger;

    public CreateServiceEntryCommandHandler(
        IServiceEntryRepository repository,
        IUnitOfWork unitOfWork,
        ILogger<CreateServiceEntryCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<ServiceEntryDto>> Handle(
        CreateServiceEntryCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var data = request.Data;

            var businessValidation = await ValidateBusinessRules(data, cancellationToken);
            if (!businessValidation.IsSuccess)
            {
                _logger.LogWarning("İş kuralı hatası: {Error}", businessValidation.Error);
                return Result<ServiceEntryDto>.Failure(businessValidation.Error!);
            }

            var entity = new ServiceEntry(
                data.LicensePlate,
                data.BrandName,
                data.ModelName,
                data.Kilometers,
                data.ServiceDate,
                data.ModelYear,
                data.HasWarranty,
                data.ServiceCity,
                data.ServiceNote);

            await _repository.AddAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation(
                "{LicensePlate} plakalı araç için servis girişi yapıldı. KM: {Kilometers}, Tarih: {Date}",
                entity.LicensePlate,
                entity.Kilometers,
                entity.ServiceDate.ToString("yyyy-MM-dd"));

            var dto = new ServiceEntryDto
            {
                Id = entity.Id,
                LicensePlate = entity.LicensePlate,
                BrandName = entity.BrandName,
                ModelName = entity.ModelName,
                Kilometers = entity.Kilometers,
                ModelYear = entity.ModelYear,
                ServiceDate = entity.ServiceDate,
                HasWarranty = entity.HasWarranty,
                ServiceCity = entity.ServiceCity,
                ServiceNote = entity.ServiceNote,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };

            return Result<ServiceEntryDto>.Success(dto);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Geçersiz servis girişi verisi.");
            return Result<ServiceEntryDto>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Servis girişi oluşturulurken hata oluştu.");
            return Result<ServiceEntryDto>.Failure(
                "Servis girişi kaydedilirken bir hata oluştu.");
        }
    }


    private async Task<Result<bool>> ValidateBusinessRules(
        CreateServiceEntryDto data,
        CancellationToken cancellationToken)
    {
        var isDuplicate = await _repository.ExistsByLicensePlateAndDateAsync(
             data.LicensePlate,
             data.ServiceDate,
             cancellationToken);

        if (isDuplicate)
        {
            _logger.LogWarning(
                "Duplicate servis girişi engellendi: Plaka {Plate}, Tarih {Date}",
                data.LicensePlate,
                data.ServiceDate.ToString("yyyy-MM-dd"));

            return Result<bool>.Failure(
                $"{data.LicensePlate} plakalı araç için {data.ServiceDate:dd.MM.yyyy} tarihinde zaten bir servis kaydı bulunmaktadır.");
        }

        return Result<bool>.Success(true);
    }

}