using AutoMapper;
using AutoMapper.QueryableExtensions;
using CraftersCloud.Core.Data;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CraftersCloud.ReferenceArchitecture.Api.Features.Users;

public static class GetRolesLookup
{
    [PublicAPI]
    public class Request : LookupRequest<Guid>;

    [UsedImplicitly]
    public class MappingProfile : Profile
    {
        public MappingProfile() =>
            CreateMap<Role, LookupResponse<Guid>>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.Name));
    }

    [UsedImplicitly]
    public class RequestHandler(IMapper mapper, IRepository<Role> roleRepository)
        : IRequestHandler<Request, IEnumerable<LookupResponse<Guid>>>
    {
        public async Task<IEnumerable<LookupResponse<Guid>>> Handle(Request request,
            CancellationToken cancellationToken) =>
            await roleRepository
                .QueryAll()
                .ProjectTo<LookupResponse<Guid>>(mapper.ConfigurationProvider, cancellationToken)
                .OrderBy(r => r.Label)
                .ToListAsync(cancellationToken);
    }
}