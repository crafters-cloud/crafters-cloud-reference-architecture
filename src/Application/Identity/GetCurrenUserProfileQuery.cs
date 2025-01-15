using CraftersCloud.Core.Cqrs;

namespace CraftersCloud.ReferenceArchitecture.Application.Identity;

public sealed record GetCurrentUserProfileQuery() : IQuery<CurrentUserProfileResponse>;

public class CurrentUserProfileResponse
{
}