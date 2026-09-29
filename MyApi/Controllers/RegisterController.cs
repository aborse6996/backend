using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using MyApi.Services;

namespace MyApi.Controllers;

[ApiController]
[Route("api/register")]
public class RegisterController : ControllerBase
{
    private readonly UserService _userService;

    public RegisterController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Register(User user)
    {
        var users = await _userService.GetUsers();

        bool emailExists = users.Any(existingUser =>
            existingUser.Email.Equals(
                user.Email,
                StringComparison.OrdinalIgnoreCase
            )
        );

        if (emailExists)
        {
            return Conflict(new
            {
                message = "A user with this email already exists."
            });
        }

        users.Add(user);

        await _userService.SaveUsers(users);

        return Ok(new
        {
            message = "User registered successfully."
        });
    }
}