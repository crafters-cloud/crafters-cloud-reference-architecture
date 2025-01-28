namespace CraftersCloud.ReferenceArchitecture.Infrastructure.Tests.MediatR;

public record RequestHandlerMeta(Type RequestType, Type RequestHandlerType, Type? CachedRequestType);