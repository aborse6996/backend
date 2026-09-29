using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using MyApi.Services;

namespace MyApi.Controllers;

[ApiController]
[Route("api/change-password")]
public class ChangePasswordController : ControllerBase
{
    private readonly UserService _userService;

    public ChangePasswordController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(
        ChangePasswordRequest request)
    {
        var users = await _userService.GetUsers();

        var user = users.FirstOrDefault(existingUser =>
            existingUser.Email.Equals(
                request.Email,
                StringComparison.OrdinalIgnoreCase
            )
        );

        if (user == null)
        {
            return NotFound(new
            {
                message = "Email not found."
            });
        }

        if (string.IsNullOrWhiteSpace(request.NewPassword))
        {
            return BadRequest(new
            {
                message = "New password cannot be blank."
            });
        }

        user.Password = request.NewPassword;

        await _userService.SaveUsers(users);

        return Ok(new
        {
            message = "Password changed successfully."
        });
    }
}