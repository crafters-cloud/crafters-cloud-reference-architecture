using System.Net.Mime;
using CraftersCloud.Core.AspNetCore;
using CraftersCloud.Core.Data;
using CraftersCloud.Core.Paging;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CraftersCloud.ReferenceArchitecture.Api.Features.Users;

[Produces(MediaTypeNames.Application.Json)]
[Route("api/[controller]")]
public class UsersController(
    IUnitOfWork unitOfWork,
    IMediator mediator) : Controller
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [UserHasPermission(PermissionId.UsersRead)]
    public async Task<ActionResult<PagedResponse<GetUsers.Response.Item>>> Search([FromQuery] GetUsers.Request query)
    {
        var response = await mediator.Send(query);
        return response.ToActionResult();
    }

    [HttpGet]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [UserHasPermission(PermissionId.UsersRead)]
    public async Task<ActionResult<GetUserDetails.Response>> Get(Guid id)
    {
        var response = await mediator.Send(GetUserDetails.Request.ById(id));
        return response.ToActionResult();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [UserHasPermission(PermissionId.UsersWrite)]
    public async Task<ActionResult<GetUserDetails.Response>> Post(CreateOrUpdateUser.Command command)
    {
        var user = await mediator.Send(command);
        await unitOfWork.SaveChangesAsync();
        return await Get(user.Id);
    }

    [HttpGet]
    [Route("roles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [UserHasPermission(PermissionId.UsersRead)]
    public async Task<ActionResult<IEnumerable<LookupResponse<Guid>>>> GetRolesLookup()
    {
        var response = await mediator.Send(new GetRolesLookup.Request());
        return response.ToActionResult();
    }

    [HttpGet]
    [Route("statuses")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [UserHasPermission(PermissionId.UsersRead)]
    public async Task<ActionResult<IEnumerable<LookupResponse<UserStatusId>>>> GetStatusesLookup()
    {
        var response = await mediator.Send(new GetStatusesLookup.Request());
        return response.ToActionResult();
    }
}