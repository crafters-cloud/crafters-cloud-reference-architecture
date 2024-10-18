using CraftersCloud.Core.Data;
using CraftersCloud.Core.Entities;
using CraftersCloud.Core.EntityFramework;
using JetBrains.Annotations;
using MediatR;

namespace CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;

[UsedImplicitly]
public class CreateOrUpdateUserCommandHandler(IRepository<User, Guid> userRepository)
    : IRequestHandler<CreateOrUpdateUser.Command, User>
{
    public async Task<User> Handle(CreateOrUpdateUser.Command request,
        CancellationToken cancellationToken)
    {
        User? user;
        if (request.Id.HasValue)
        {
            user = await userRepository.QueryAll().QueryById(request.Id.Value).SingleOrNotFoundAsync(cancellationToken);
            user.Update(request);
        }
        else
        {
            user = User.Create(request);
            userRepository.Add(user);
        }

        return user;
    }
}