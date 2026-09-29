using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using MyApi.Services;

namespace MyApi.Controllers;

[ApiController]
[Route("api/login")]
public class LoginController : ControllerBase
{
    private readonly UserService _userService;

    public LoginController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        var users = await _userService.GetUsers();

        var user = users.FirstOrDefault(existingUser =>
            existingUser.Email.Equals(
                loginRequest.Email,
                StringComparison.OrdinalIgnoreCase
            )
        );

        if (user == null)
        {
            return Unauthorized(new
            {
                message = "User doesn't exist."
            });
        }

        if (user.Password != loginRequest.Password)
        {
            return Unauthorized(new
            {
                message = "Password is incorrect."
            });
        }

        return Ok(new
        {
            message = "Login successful.",
            name = user.Name
        });
    }
}