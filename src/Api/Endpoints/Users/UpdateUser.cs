using CraftersCloud.ReferenceArchitecture.Api.MinimalApi;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users;

public static partial class UpdateUser
{
    public sealed record Request(
        Guid Id,
        string EmailAddress,
        string FirstName,
        string LastName,
        Guid RoleId,
        UserStatusId UserStatusId);

    public static async Task<Results<NoContent, NotFound, BadRequest<ValidationProblemDetails>>> Handle(
        [FromBody] Request request,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var command = UpdateUserRequestMapper.ToCommand(request);
        var commandResult = await sender.Send(command, cancellationToken);
        return commandResult.ToMinimalApiResult(context);
    }

    [Mapper]
    public static partial class UpdateUserRequestMapper
    {
        public static partial UpdateUserCommand ToCommand(Request source);
    }
}