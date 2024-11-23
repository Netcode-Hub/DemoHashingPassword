using DemoHashingPassword.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace DemoHashingPassword.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly PasswordHasher<AppUser> _passwordHasher;
        public AuthenticationController()
        {
            _passwordHasher = new PasswordHasher<AppUser>();
        }
        private static List<AppUser> Users = [];

        [HttpPost("create")]
        public IActionResult Register(AppUser user)
        {
            
            var hashedPassword = _passwordHasher.HashPassword(user, user.Password!);
            user.Password = hashedPassword;
            Users.Add(user);
            return Ok(user);
        }

        [HttpPost("login")]
        public IActionResult Login(AppUser user)
        {
            var _user = Users.FirstOrDefault(_ => _.Email == user.Email);
            var result = _passwordHasher.VerifyHashedPassword(user, _user.Password, user.Password);
            return result == PasswordVerificationResult.Success ? Ok(_user) : Unauthorized();
        }
    }
}
