using CraftersCloud.ReferenceArchitecture.Domain.Users;
using CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users;

public static partial class CreateUser
{
    public sealed record Request(string EmailAddress, string FullName, Guid RoleId, UserStatusId UserStatusId);

    public static async Task<Results<Created<User>, BadRequest<ValidationProblemDetails>>> Handle(
        [FromBody] Request request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = UpdateUserRequestMapper.ToCommand(request);
        var commandResult = await sender.Send(command, cancellationToken);
        var results = commandResult.ToMinimalApiResult();
        return results;
    }

    [Mapper]
    public static partial class UpdateUserRequestMapper
    {
        public static partial CreateUserCommand ToCommand(Request source);
    }
}