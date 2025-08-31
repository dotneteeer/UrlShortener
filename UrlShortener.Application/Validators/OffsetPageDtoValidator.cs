using FluentValidation;
using Microsoft.Extensions.Options;
using UrlShortener.Application.Settings;
using UrlShortener.Domain.Dtos.Page;
using UrlShortener.Domain.Interfaces.Validation;

namespace UrlShortener.Application.Validators;

public class OffsetPageDtoValidator : AbstractValidator<OffsetPageDto>, INullSafeValidator<OffsetPageDto>
{
    public OffsetPageDtoValidator(IOptions<PaginationRules> businessRules)
    {
        var maxPageSize = businessRules.Value.MaxPageSize;

        RuleFor(x => x.Skip).NotNull().GreaterThanOrEqualTo(0);
        RuleFor(x => x.Take).NotNull().InclusiveBetween(0, maxPageSize);
    }

    public bool IsValid(OffsetPageDto? instance, out IEnumerable<string> errorMessages)
    {
        errorMessages = [];

        if (instance == null) return false;

        var result = Validate(instance);

        errorMessages = result.Errors.Select(x => x.ErrorMessage);

        return result.IsValid;
    }
}