using System.Net.Mime;
using CraftersCloud.Core.Paging;
using CraftersCloud.ReferenceArchitecture.Domain.Authorization;
using CraftersCloud.ReferenceArchitecture.Domain.Users;
using CraftersCloud.ReferenceArchitecture.Domain.Users.Commands;
using CraftersCloud.ReferenceArchitecture.Infrastructure.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftersCloud.ReferenceArchitecture.Api.Features.Users;

[Produces(MediaTypeNames.Application.Json)]
[Route("api/[controller]")]
public class UsersController : Controller
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [UserHasPermission(PermissionId.UsersRead)]
    public async Task<ActionResult<PagedResponse<GetUsers.Response.Item>>> Search([FromQuery] GetUsers.Request query) =>
        await Task.FromResult(NoContent());

    [HttpGet]
    [Route("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [UserHasPermission(PermissionId.UsersRead)]
    public async Task<ActionResult<GetUserDetails.Response>> Get(Guid id) => await Task.FromResult(NoContent());

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [UserHasPermission(PermissionId.UsersWrite)]
    public async Task<ActionResult<GetUserDetails.Response>> Post(CreateOrUpdateUser.Command command) =>
        await Task.FromResult(NoContent());

    [HttpGet]
    [Route("roles")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [UserHasPermission(PermissionId.UsersRead)]
    public async Task<ActionResult<IEnumerable<LookupResponse<Guid>>>> GetRolesLookup() =>
        await Task.FromResult(NoContent());

    [HttpGet]
    [Route("statuses")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [UserHasPermission(PermissionId.UsersRead)]
    public async Task<ActionResult<IEnumerable<LookupResponse<UserStatusId>>>> GetStatusesLookup() =>
        await Task.FromResult(NoContent());
}