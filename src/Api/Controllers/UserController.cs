using Api.Common;
using Domain.Dtos;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserProfileResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _userService.GetAllUsers();
        return Ok(result);
    }
    
    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserProfileResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUser(Guid id)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.GetUserById(id);
            if (result == null)
                return NotFound($"No user found with ID {id}");

            return Ok(result);
        }
        catch (ResourceNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpGet("role/{roleName}")]
    public async Task<IActionResult> GetUsersByRole(string roleName)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var users = await _userService.GetUsersByRole(roleName);
        if (users == null)
            return NotFound($"No users found with role {roleName}");

        return Ok(users);
    }
    
    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(UserProfileResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserUpdateDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try {
            var result = await _userService.UpdateUser(id, dto);
            return Ok(result);
        } catch (ResourceNotFoundException ex) {
            return NotFound(ex.Message);
        }
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _userService.DeleteUser(id);
            return NoContent();
        }
        catch (ResourceNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}