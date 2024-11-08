using CraftersCloud.Core.Data;
using CraftersCloud.Core.Entities;
using CraftersCloud.Core.EntityFramework;
using CraftersCloud.ReferenceArchitecture.Api.Features.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Identity;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Api.Features.Identity;

public static class GetUserProfile
{
    [PublicAPI]
    public class Response
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public IReadOnlyCollection<PermissionId> Permissions { get; set; } = [];
    }

    public static async Task<Results<Ok<Response>, NotFound>> Handle(ICurrentUserProvider currentUserProvider,
        IRepository<User> repository,
        CancellationToken cancellationToken)
    {
        if (currentUserProvider.UserId is null)
        {
            return TypedResults.NotFound();
        }

        var user = await repository.QueryAll().QueryById(currentUserProvider.UserId.Value)
            .Include(u => u.Role)
            .ThenInclude(r => r.Permissions)
            .Include(u => u.UserStatus)
            .AsNoTracking()
            .SingleOrNotFoundAsync(cancellationToken);

        var response = user.ToResponse();
        return TypedResults.Ok(response);
    }
}