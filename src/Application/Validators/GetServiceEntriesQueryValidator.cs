using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleServiceTracker.Application.Features.ServiceEntries.Queries;

namespace VehicleServiceTracker.Application.Validators
{
    public class GetServiceEntriesQueryValidator : AbstractValidator<GetServiceEntriesQuery>
    {
        public GetServiceEntriesQueryValidator()
        {
            //   Pagination Kuralları
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                    .WithMessage("Sayfa numarası en az 1 olmalıdır.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100)
                    .WithMessage("Sayfa boyutu 1 ile 100 arasında olmalıdır.");
        }
    }
}
