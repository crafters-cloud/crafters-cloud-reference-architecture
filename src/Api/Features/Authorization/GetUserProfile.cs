using AutoMapper;
using CraftersCloud.Core.Data;
using CraftersCloud.Core.Entities;
using CraftersCloud.Core.EntityFramework;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Identity;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using JetBrains.Annotations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Api.Features.Authorization;

public static class GetUserProfile
{
    [PublicAPI]
    public class Request : IRequest<Response>;

    [PublicAPI]
    public class Response
    {
        public Guid Id { get; init; }
        public string FullName { get; init; } = string.Empty;
        public string EmailAddress { get; init; } = string.Empty;
        public IEnumerable<PermissionId> Permissions { get; init; } = [];
    }

    [UsedImplicitly]
    public class MappingProfile : Profile
    {
        public MappingProfile() =>
            CreateMap<User, Response>()
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(user => user.GetPermissionIds()));
    }

    [UsedImplicitly]
    public class RequestHandler(IMapper mapper, ICurrentUserProvider currentUserProvider, IRepository<User> repository)
        : IRequestHandler<Request, Response>
    {
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
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
                .SingleOrNotFoundAsync(cancellationToken);
            var response = mapper.Map<Response>(user);
            return response;
        }
    }
}