using CraftersCloud.Core.Paging;

namespace CraftersCloud.ReferenceArchitecture.Api.Models;

public class PagedRequestValidator<T, TResponse> : AbstractValidator<T> where T : PagedQuery<TResponse>
{
    protected PagedRequestValidator()
    {
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(0).LessThanOrEqualTo(1000);
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
    }
}