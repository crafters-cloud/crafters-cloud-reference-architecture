using CraftersCloud.ReferenceArchitecture.Api.MinimalApi;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Users;

public static partial class CreateUser
{
    public sealed record Request(
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
            RuleFor(x => x.EmailAddress).ValidateUserEmail(x => null, scopeFactory);
            RuleFor(x => x.FirstName).ValidateUserFirstName();
            RuleFor(x => x.LastName).ValidateUserLastName();
            RuleFor(x => x.RoleId).ValidateRoleId();
        }
    }
    
    [Mapper]
    public static partial class Mapper
    {
        public static partial CreateUserCommand Map(Request source);
    }

    public static async Task<Results<Created<User>, BadRequest<ValidationProblemDetails>>> Handle(
        [FromBody] Request request,
        HttpContext httpContext,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = Mapper.Map(request);
        var commandResult = await sender.Send(command, cancellationToken);
        return commandResult.ToMinimalApiResult(httpContext, user => $"/users/{user.Id}");
    }
}