﻿using System.ComponentModel.DataAnnotations;
using AutoMapper;
using CraftersCloud.Core.Data;
using CraftersCloud.Core.Entities;
using CraftersCloud.Core.EntityFramework;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Api.Features.Users;

public static class GetUserDetails
{
    [PublicAPI]
    public class Request : IRequest<Response>
    {
        public Guid Id { get; set; }

        public static Request ById(Guid id) => new() { Id = id };
    }

    [PublicAPI]
    public class Response
    {
        [Required]
        public required Guid Id { get; set; }
        [Required]
        public required string EmailAddress { get; set; } = string.Empty;
        [Required]
        public required string FullName { get; set; } = string.Empty;
        public Guid RoleId { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset UpdatedOn { get; set; }
        public UserStatusId UserStatusId { get; set; } = UserStatusId.Active;
        public string UserStatusName { get; set; } = string.Empty;
        public string UserStatusDescription { get; set; } = string.Empty;
    }

    [UsedImplicitly]
    public class MappingProfile : Profile
    {
        public MappingProfile() => CreateMap<User, Response>();
    }

    [UsedImplicitly]
    public class RequestHandler(IRepository<User> repository, IMapper mapper) : IRequestHandler<Request, Response>
    {
        public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
        {
            var response = await repository.QueryAll()
                .BuildInclude()
                .QueryById(request.Id)
                .SingleOrDefaultMappedAsync<User, Response>(mapper, cancellationToken);
            return response;
        }
    }

    private static IQueryable<User> BuildInclude(this IQueryable<User> query) => query.Include(x => x.UserStatus);
}