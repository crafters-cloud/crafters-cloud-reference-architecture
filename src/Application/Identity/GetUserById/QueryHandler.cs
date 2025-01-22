using CraftersCloud.Core.Cqrs;
using CraftersCloud.Core.Data;
using CraftersCloud.Core.Entities;
using CraftersCloud.Core.Results;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;
using OneOf;

namespace CraftersCloud.ReferenceArchitecture.Application.Identity.GetUserById;

[UsedImplicitly]
public class QueryHandler(IRepository<User> repository)
    : IQueryHandler<Query, OneOf<QueryResponse, NotFoundResult>>
{
    public async Task<OneOf<QueryResponse, NotFoundResult>> Handle(Query query,
        CancellationToken cancellationToken)
    {
        var entity = await repository.QueryAll()
            .Include(x => x.UserStatus)
            .AsNoTracking()
            .QueryById(query.Id)
            .QueryActiveOnly()
            .SingleOrDefaultAsync(cancellationToken);

        return entity != null ? Map(entity) : Result.NotFound();
    }

    private static QueryResponse Map(User source) =>
        new()
        {
            Id = source.Id,
            EmailAddress = source.EmailAddress,
            FirstName = source.FirstName,
            LastName = source.LastName,
            RoleId = source.RoleId,
            CreatedOn = source.CreatedOn,
            UpdatedOn = source.UpdatedOn,
            UserStatusId = source.UserStatusId,
            UserStatusName = source.UserStatus.Name,
            UserStatusDescription = source.UserStatus.Description
        };
}