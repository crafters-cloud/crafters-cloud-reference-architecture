using CraftersCloud.Core.Cqrs;

public sealed record GetCurrentUserProfileQuery() : IQuery<CurrentUserProfileResponse>;

public class CurrentUserProfileResponse
{
}