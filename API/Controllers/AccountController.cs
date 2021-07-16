using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using API.Services;
using BLL.Utils;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<BLL.Models.User> _signInManager;
        private readonly UserManager<BLL.Models.User> _userManager;
        private readonly GoogleAuthService _googleAuthService;
        private readonly FacebookAuthService _facebookAuthService;
        public AccountController(SignInManager<BLL.Models.User> signInManager, UserManager<BLL.Models.User> userManager, GoogleAuthService googleAuthService, FacebookAuthService facebookAuthService) 
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._facebookAuthService = facebookAuthService;
            this._googleAuthService = googleAuthService;
        }

        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login([FromBody] BLL.Models.LoginRequest loginRequest)
        {
            var result = await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, false,false);
            if (result.Succeeded)
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == loginRequest.Email);
                return Ok(JWT.GenerateJwtToken(loginRequest.Email, appUser));
            }
            else
            {
                if (result.IsLockedOut) return Problem("your account is locked out");
                if (result.IsNotAllowed) return Problem("your account is not allowed");
                if (result.RequiresTwoFactor) return Problem("your account is requires two factor");
                return Unauthorized("email or password not correct");
            }
        }

        [HttpPost(nameof(Register))]
        public async Task<IActionResult> Register([FromBody] BLL.Models.User user)
        {
            user.UserName = user.Email;
            var result = await _userManager.CreateAsync(user, user.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Ok(JWT.GenerateJwtToken(user.Email, user));
            }
            else
            {
                return Problem(string.Join('\n', result.Errors));
            }
        }

        [HttpPost(nameof(Google))]
        public async Task<IActionResult> Google([FromBody] BLL.Models.UserToken token)
        {
            try
            {
                var user = await _googleAuthService.Authenticate(token);
                await _signInManager.SignInAsync(user, true);
                return Ok(JWT.GenerateJwtToken(user.Email, user));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPost(nameof(Facebook))]
        public async Task<IActionResult> Facebook([FromBody] BLL.Models.UserToken token)
        {
            try
            {
                var user = await _facebookAuthService.Authenticate(token);
                await _signInManager.SignInAsync(user, true);
                var jwtToken = JWT.GenerateJwtToken(user.Email, user);
                return Ok(jwtToken);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}
