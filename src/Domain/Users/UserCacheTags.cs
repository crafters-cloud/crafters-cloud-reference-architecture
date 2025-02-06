namespace CraftersCloud.ReferenceArchitecture.Domain.Users;

public static class UserCacheTags
{
    public const string Users = "Users";
    public static string User(UserId id) => $"User:{id}";
}