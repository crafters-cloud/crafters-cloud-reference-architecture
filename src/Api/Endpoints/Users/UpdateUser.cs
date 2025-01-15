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
    
    [UsedImplicitly]
    public class Validator : AbstractValidator<Request>
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public Validator(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            RuleFor(x => x.EmailAddress).ValidateUserEmail(UniqueEmailAddress);
            RuleFor(x => x.FirstName).ValidateUserFirstName();
            RuleFor(x => x.LastName).ValidateUserLastName();
            RuleFor(x => x.RoleId).ValidateRoleId();
        }

        private async Task<bool> UniqueEmailAddress(Request command, string name,
            CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var repository = scope.Resolve<IRepository<User>>();
            return !await repository.QueryAll()
                .QueryByEmail(name)
                .QueryExceptWithId(UserId.Create(command.Id))
                .AnyAsync(cancellationToken);
        }
    }

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