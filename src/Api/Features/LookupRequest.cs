using CraftersCloud.Core.Cqrs;

namespace CraftersCloud.ReferenceArchitecture.Api.Features;

public class LookupRequest<T> : IQuery<IEnumerable<LookupResponse<T>>>;