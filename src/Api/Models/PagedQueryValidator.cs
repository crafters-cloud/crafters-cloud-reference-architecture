using CraftersCloud.Core.Paging;

namespace CraftersCloud.ReferenceArchitecture.Api.Models;

public static class PagedQueryValidatorExtensions
{
    public static void AddPagedQueryRules<T>(this AbstractValidator<T> validator) where T : IPagedQuery
    {
        validator.RuleFor(x => x.PageSize).GreaterThanOrEqualTo(0).LessThanOrEqualTo(1000);
        validator.RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
    }
}