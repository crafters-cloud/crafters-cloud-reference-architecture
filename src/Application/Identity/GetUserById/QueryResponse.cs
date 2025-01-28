using CraftersCloud.ReferenceArchitecture.Domain.Users;

namespace CraftersCloud.ReferenceArchitecture.Application.Identity.GetUserById;

public class QueryResponse
{
    public Guid Id { get; set; }
    public string EmailAddress { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset UpdatedOn { get; set; }
    public UserStatusId UserStatusId { get; set; } = UserStatusId.Active;
    public string UserStatusName { get; set; } = string.Empty;
    public string UserStatusDescription { get; set; } = string.Empty;
}