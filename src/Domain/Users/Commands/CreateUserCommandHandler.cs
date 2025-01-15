using CraftersCloud.Core.Data;
using CraftersCloud.ReferenceArchitecture.Core.Cqrs;
using MediatR;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;

public class CreateUserCommandHandler(IRepository<User> repository)
    : IRequestHandler<CreateUserCommand, CreateCommandResult<User>>
{
    public Task<CreateCommandResult<User>> Handle(CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var user = User.Create(command);
        repository.Add(user);
        var result = CommandResult.Created(user);
        return Task.FromResult(result);
    }
}