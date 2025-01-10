using CraftersCloud.Core.Data;
using CraftersCloud.ReferenceArchitecture.Core.Cqrs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;

public class UpdateUserCommandHandler(IRepository<User, UserId> userRepository)
    : IRequestHandler<UpdateUserCommand, UpdateCommandResult<User>>
{
    public async Task<UpdateCommandResult<User>> Handle(UpdateUserCommand command,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.QueryAll().QueryById(command.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            return CommandResult.UpdateNotFound<User>();
        }

        user.Update(command);
        return CommandResult.UpdateSuccess<User>();
    }
}