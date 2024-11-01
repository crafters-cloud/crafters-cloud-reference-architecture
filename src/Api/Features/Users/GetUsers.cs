﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using CraftersCloud.Core.Data;
using CraftersCloud.Core.EntityFramework;
using CraftersCloud.Core.Paging;
using CraftersCloud.ReferenceArchitecture.Domain.Identity;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Api.Features.Users;

public static class GetUsers
{
    [PublicAPI]
    public class Request : PagedRequest<Response.Item>
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
    }

    [PublicAPI]
    public static class Response
    {
        [PublicAPI]
        public class Item
        {
            public Guid Id { get; set; }
            public string EmailAddress { get; set; } = string.Empty;
            public string FullName { get; set; } = string.Empty;
            public string UserStatusName { get; set; } = string.Empty;
            public DateTimeOffset CreatedOn { get; set; }
            public DateTimeOffset UpdatedOn { get; set; }
        }

        [UsedImplicitly]
        public class MappingProfile : Profile
        {
            public MappingProfile() => CreateMap<User, Item>();
        }
    }

    [UsedImplicitly]
    public class RequestHandler(IRepository<User> repository, IMapper mapper)
        : IPagedRequestHandler<Request, Response.Item>
    {
        public async Task<PagedResponse<Response.Item>> Handle(Request request, CancellationToken cancellationToken) =>
            await repository.QueryAll()
                .Include(u => u.UserStatus)
                .QueryByName(request.Name)
                .QueryByEmail(request.Email)
                .ProjectTo<Response.Item>(mapper.ConfigurationProvider, cancellationToken)
                .ToPagedResponseAsync(request, cancellationToken);
    }
}