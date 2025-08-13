using CraftersCloud.ReferenceArchitecture.Api.MinimalApi;
using CraftersCloud.ReferenceArchitecture.Application.Identity.GetUserById;
using CraftersCloud.ReferenceArchitecture.Domain.Users;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users;

public static partial class GetUserById
{
    [PublicAPI]
    public class Response
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
    
    [Mapper]
    public static partial class Mapper
    {
        public static partial Response Map(QueryResponse source);
    }

    public static async Task<Results<Ok<Response>, NotFound>> Handle(Guid id, ISender sender,
        CancellationToken cancellationToken)
    {
        var query = new Query(new UserId(id));
        var response = await sender.Send(query, cancellationToken);
        return response.ToMinimalApiResult(Mapper.Map);
    }
}