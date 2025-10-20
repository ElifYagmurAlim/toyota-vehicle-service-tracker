using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleServiceTracker.Application.Common.Models;
using VehicleServiceTracker.Application.DTOs;
using VehicleServiceTracker.Application.Features.ServiceEntries.Commands;
using VehicleServiceTracker.Application.Features.ServiceEntries.Queries;

namespace VehicleServiceTracker.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ServiceEntriesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ServiceEntriesController> _logger;

    public ServiceEntriesController(IMediator mediator, ILogger<ServiceEntriesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Sayfalanmış servis girişlerini listeler
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResult<ServiceEntryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetServiceEntries(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1)
        {
            _logger.LogWarning("Geçersiz pageNumber: {PageNumber}", pageNumber);
            return BadRequest(new { message = "Sayfa numarası 1'den küçük olamaz." });
        }

        if (pageSize < 1 || pageSize > 100)
        {
            _logger.LogWarning("Geçersiz pageSize: {PageSize}", pageSize);
            return BadRequest(new { message = "Sayfa boyutu 1-100 arasında olmalıdır." });
        }

        var query = new GetServiceEntriesQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });

        return Ok(result.Data);
    }

    /// <summary>
    /// Yeni servis girişi oluşturur
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ServiceEntryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateServiceEntry([FromBody] CreateServiceEntryDto dto)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Model validation hatası: {Errors}",
                string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            return BadRequest(ModelState);
        }

        var command = new CreateServiceEntryCommand(dto);
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
            return BadRequest(new { message = result.Error });

        return CreatedAtAction(
            nameof(GetServiceEntries),
            new { id = result.Data!.Id },
            result.Data);
    }
}