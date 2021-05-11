using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //localhost:52600/api/account/
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<BLL.Models.User> signInManager;
        private readonly UserManager<BLL.Models.User> userManager;
        public AccountController(SignInManager<BLL.Models.User> _signInManager, UserManager<BLL.Models.User> _userManager) 
        {
            this.signInManager = _signInManager;
            this.userManager = _userManager;
        }
        //[HttpPost("Login")]
        ////localhost:52600/api/account/login
        //public async Task<IActionResult> Login([FromBody] BLL.Models.LoginRequest loginRequest)
        //{

        //}
        [HttpPost("Register")]
        //localhost:52600/api/account/login
        public async Task<IActionResult> Register([FromBody] BLL.Models.User user)
        {
            var result = await userManager.CreateAsync(user, user.Password);
            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return Problem(string.Join('\n', result.Errors));
            }
        }
    }
}
