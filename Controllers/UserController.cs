using JrEntryWebApi.Models;
using JrEntryWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    // 1.0 Read
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        var users = await _userService.GetAllUser();
        return Ok(users);
    }

    // 2.0 Create

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        await _userService.AddNewUser(user); // ✅ properly awaited
        return Ok();
    }

    // 3.1 Update
    [HttpPut("{userName}")]
    public async Task<IActionResult> UpdateUser([FromBody] User user, [FromRoute] string userName)
    {
        var userInDb = await _userService.FindByUserName(userName);

        if (userInDb == null)
        {
            return NotFound(); // return 404 if user not found
        }

        // Update fields
        userInDb.UserName = user.UserName;
        userInDb.Password = user.Password;

        await _userService.UpdateUser(userInDb);

        return NoContent(); // 204 No Content = successful update
    }

}
