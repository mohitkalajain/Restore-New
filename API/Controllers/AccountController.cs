using API.DTOs;
using API.Entities;
using API.Helper.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenConfiguration _tokenConfiguration;
        public AccountController(UserManager<User> userManager, TokenConfiguration tokenConfiguration)
        {
            _userManager = userManager;
            _tokenConfiguration = tokenConfiguration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user,loginDto.Password))
                return Unauthorized();

            return Ok(new UserDto { Email=user.Email,Token= await _tokenConfiguration.GenerateToken(user)});
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO registerDto)
        {
            var user = new User { UserName=registerDto.UserName,Email=registerDto.Email };

            var result = await _userManager.CreateAsync(user,registerDto.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return ValidationProblem();
            }

            await _userManager.AddToRoleAsync(user, "Member");
            return Ok();
        }


        [Authorize]
        [HttpPost("currentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return Ok(new UserDto { Email = user.Email, Token = await _tokenConfiguration.GenerateToken(user) });
        }
    }
}
