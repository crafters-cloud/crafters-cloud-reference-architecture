using CraftersCloud.ReferenceArchitecture.Api.MinimalApi;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users;

public static partial class UpdateUser
{
    public sealed record Request(
        UserId Id,
        string EmailAddress,
        string FirstName,
        string LastName,
        RoleId RoleId,
        UserStatusId UserStatusId);
    
    [UsedImplicitly]
    public class Validator : AbstractValidator<Request>
    {
        public Validator(IServiceScopeFactory scopeFactory)
        {
            RuleFor(x => x.EmailAddress).ValidateUserEmail(x => x.Id, scopeFactory);
            RuleFor(x => x.FirstName).ValidateUserFirstName();
            RuleFor(x => x.LastName).ValidateUserLastName();
            RuleFor(x => x.RoleId).ValidateRoleId();
        }
    }
    
    [Mapper]
    public static partial class Mapper
    {
        public static partial UpdateUserCommand Map(Request source);
    }

    public static async Task<Results<NoContent, NotFound, BadRequest<ValidationProblemDetails>>> Handle(
        [FromBody] Request request,
        ISender sender,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map(request);
        var commandResult = await sender.Send(command, cancellationToken);
        return commandResult.ToMinimalApiResult(context);
    }
}