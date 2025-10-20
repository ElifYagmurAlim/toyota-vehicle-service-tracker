using MediatR;
using Microsoft.Extensions.Logging;
using VehicleServiceTracker.Application.Common.Models;
using VehicleServiceTracker.Application.DTOs;
using VehicleServiceTracker.Domain.Interfaces;

namespace VehicleServiceTracker.Application.Features.ServiceEntries.Queries;

public record GetServiceEntriesQuery(int PageNumber, int PageSize) 
    : IRequest<Result<PaginatedResult<ServiceEntryDto>>>;

public class GetServiceEntriesQueryHandler 
    : IRequestHandler<GetServiceEntriesQuery, Result<PaginatedResult<ServiceEntryDto>>>
{
    private readonly IServiceEntryRepository _repository;
    private readonly ILogger<GetServiceEntriesQueryHandler> _logger;

    public GetServiceEntriesQueryHandler(
        IServiceEntryRepository repository,
        ILogger<GetServiceEntriesQueryHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<PaginatedResult<ServiceEntryDto>>> Handle(
        GetServiceEntriesQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Servis girişlerini listeleyen method çağırıldı.");

            var (items, totalCount) = await _repository.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                cancellationToken);

            var dtos = items.Select(x => new ServiceEntryDto
            {
                Id = x.Id,
                LicensePlate = x.LicensePlate,
                BrandName = x.BrandName,
                ModelName = x.ModelName,
                Kilometers = x.Kilometers,
                ModelYear = x.ModelYear,
                ServiceDate = x.ServiceDate,
                HasWarranty = x.HasWarranty,
                ServiceCity = x.ServiceCity,
                ServiceNote = x.ServiceNote,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            });

            var result = new PaginatedResult<ServiceEntryDto>(
                dtos,
                totalCount,
                request.PageNumber,
                request.PageSize);

            return Result<PaginatedResult<ServiceEntryDto>>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Servis girişleri listelenirken hata oluştu.");
            return Result<PaginatedResult<ServiceEntryDto>>.Failure(
                "Servis girişleri yüklenirken bir hata oluştu.");
        }
    }
}