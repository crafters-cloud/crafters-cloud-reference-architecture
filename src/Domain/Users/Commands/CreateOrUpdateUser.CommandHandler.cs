using Ardalis.Result;
using CraftersCloud.Core.Data;
using CraftersCloud.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;

[UsedImplicitly]
public static partial class CreateOrUpdateUser
{
    public class CommandHandler(IRepository<User, Guid> userRepository) : IRequestHandler<Command, Result<User>>
    {
        public async Task<Result<User>> Handle(Command command,
            CancellationToken cancellationToken)
        {
            User? user;
            if (command.Id.HasValue)
            {
                user = await userRepository.QueryAll().QueryById(command.Id.Value)
                    .SingleOrDefaultAsync(cancellationToken);

                if (user == null)
                {
                    return Result.NotFound();
                }

                user.Update(command);
                return Result.NoContent();
            }

            user = User.Create(command);
            userRepository.Add(user);
            return Result.Created(user);
        }
    }
}