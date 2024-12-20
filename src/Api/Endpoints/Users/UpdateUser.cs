﻿using CraftersCloud.Core.Messaging.CommandResults;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;

namespace CraftersCloud.ReferenceArchitecture.Api.Endpoints.ComplexExamples.Users;

public static partial class UpdateUser
{
    public sealed record Request(
        Guid Id,
        string EmailAddress,
        string FullName,
        Guid RoleId,
        UserStatusId UserStatusId);

    public static async Task<Results<NoContent, NotFound, BadRequest<ValidationProblemDetails>>> Handle([FromBody] Request request,
        IMediator sender,
        CancellationToken cancellationToken)
    {
        var command = UpdateUserRequestMapper.ToCommand(request);
        var commandResult = await sender.Send(command, cancellationToken);
        return commandResult.ToMinimalApiResult();
    }

    [Mapper]
    public static partial class UpdateUserRequestMapper
    {
        public static partial UpdateUserCommand ToCommand(Request source);
    }
}