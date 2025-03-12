using Contracts.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistence.IdnetityProvider;
using Services.Abstractions;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IFirebaseAuthService _firebaseAuthService;

        public UserController(IUserService userService, IFirebaseAuthService firebaseAuthService)
        {
            _userService = userService;
            _firebaseAuthService = firebaseAuthService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] AddUserDto model)
        {
            string? userId = await _firebaseAuthService.SignUp(model);
            return Ok(userId);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            UserDto? user = await _firebaseAuthService.Login(model.Email, model.Password);
            if (user == null)
            {
                return BadRequest("Invalid user name or password");
            }
            return Ok(user);
        }

        [HttpPut]
        [Route("updateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto model)
        {
            await _userService.UpdateUser(model);
            return Ok();
        }

        [HttpGet]
        [Route("getUserById/{userId}")]
        public async Task<IActionResult> GetUserById([FromRoute] string userId)
        {
            var user = await _userService.GetUserById(userId);
            if (user == null)
            {
                return BadRequest("User doesn't exist");
            }
            return Ok(user);
        }

        [HttpGet]
        [Route("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            _firebaseAuthService.SignOut();
            return Ok();
        }

        [HttpPost]
        [Route("googleSignIn")]
        public async Task<IActionResult> GoogleSignIn([FromBody] GoogleSignInDto model)
        {
            var user = await _firebaseAuthService.GoogleSignIn(model.IdToken);
            return Ok(user);
        }
    }
}
