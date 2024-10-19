using CraftersCloud.Core.Data;
using CraftersCloud.Core.Entities;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Identity;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using FastEndpoints;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.Profiles;

public static class GetProfile
{
    [PublicAPI]
    public sealed class Response
    {
        public required Guid Id { get; init; }
        public required string FullName { get; init; } = string.Empty;
        public required string EmailAddress { get; init; } = string.Empty;
        public required IReadOnlyCollection<PermissionId> Permissions { get; init; } = [];
    }

    private sealed class Endpoint(ICurrentUserProvider currentUserProvider, IRepository<User> repository)
        : EndpointWithoutRequest<Results<Ok<Response>, NotFound>, Mapper>
    {
        public override void Configure() => Get("/api/profile");

        public override async Task<Results<Ok<Response>, NotFound>> ExecuteAsync(CancellationToken ct)
        {
            if (currentUserProvider.UserId is null)
            {
                throw new InvalidOperationException(
                    "UserId could not be determined. This could happen if user is authenticated but it could not be found in the database.");
            }

            var user = await repository.QueryAll().QueryById(currentUserProvider.UserId.Value)
                .Include(u => u.Role)
                .ThenInclude(r => r.Permissions)
                .Include(u => u.UserStatus)
                .SingleOrDefaultAsync(ct);

            if (user is null)
            {
                return TypedResults.NotFound();
            }

            var response = Map.FromEntity(user);
            return TypedResults.Ok(response);
        }
    }

    [UsedImplicitly]
    private sealed class Mapper : ResponseMapper<Response, User>
    {
        public override Response FromEntity(User entity) => new()
        {
            Id = entity.Id,
            FullName = entity.FullName,
            EmailAddress = entity.EmailAddress,
            Permissions = entity.Role.Permissions.Select(p => p.Id).ToList()
        };
    }
}