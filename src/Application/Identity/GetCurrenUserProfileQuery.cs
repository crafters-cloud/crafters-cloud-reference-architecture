using CraftersCloud.Core.Messaging;

public sealed record GetCurrentUserProfileQuery() : IQuery<CurrentUserProfileResponse>;

public class CurrentUserProfileResponse
{
}