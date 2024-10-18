using MediatR;

namespace CraftersCloud.ReferenceArchitecture.Api.Features;

public class LookupRequest<T> : IRequest<IEnumerable<LookupResponse<T>>>;