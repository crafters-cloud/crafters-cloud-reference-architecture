using CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users;
public static partial class CreateUser
{
    public sealed record Request(string EmailAddress, string FullName, Guid RoleId, UserStatusId UserStatusId);

    [UsedImplicitly]
    public class Validator : AbstractValidator<Request>;

    public static async Task<IResult> Handle([FromBody] Request request, ISender sender,
        CancellationToken cancellationToken)
    {
        var command = UpdateUserRequestMapper.ToCommand(request);
        var result = await sender.Send(command, cancellationToken);
        return result.ToMinimalApiResult();
    }

    [Mapper]
    public static partial class UpdateUserRequestMapper
    {
        [MapperIgnoreTarget(nameof(CreateOrUpdateUser.Command.Id))]
        public static partial CreateOrUpdateUser.Command ToCommand(Request source);
    }
}