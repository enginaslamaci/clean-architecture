using CleanArch.API.Controllers.Base.v1;
using CleanArch.Application.Enums;
using CleanArch.Application.Features.User.Commands.ChangePassword;
using CleanArch.Application.Features.User.Commands.CreateUser;
using CleanArch.Application.Features.User.Commands.DeleteUser;
using CleanArch.Application.Features.User.Commands.UpdateUser;
using CleanArch.Application.Features.User.Queries.GetAllUsers;
using CleanArch.Application.Features.User.Queries.GetUserById;
using CleanArch.Application.Features.User.Queries.GetUserByRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanArch.API.Controllers.v1
{
    [Authorize(Roles = nameof(Roles.Admin))]
    public class UserController : Basev1ApiController
    {

        [HttpGet("")]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersQueryRequest query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("role")]
        public async Task<IActionResult> GetByRole([FromQuery] GetUserByRoleQueryRequest query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByKey(string id)
        {
            return Ok(await Mediator.Send(new GetUserByIdQueryRequest() { Id = id }));
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromBody] CreateUserCommandRequest request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpPut("")]
        public async Task<IActionResult> Edit([FromBody] UpdateUserCommandRequest request)
        {
            return Ok(await Mediator.Send(request));
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            return Ok(await Mediator.Send(new DeleteUserCommandRequest() { UserId = userId }));
        }


        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest command)
        {
            return Ok(await Mediator.Send(command));
        }
    }
}
