using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly SignInManager<BLL.Models.User> signInManager;
        private readonly UserManager<BLL.Models.User> userManager;
        private readonly API.Services.EmailSender emailSender;
        private readonly API.Services.FacebookAuthService facebookAuthService;
        private readonly API.Services.GoogleAuthService googleAuthService;
        public AccountController(SignInManager<BLL.Models.User> _signInManager, UserManager<BLL.Models.User> _userManager, API.Services.FacebookAuthService _facebookAuthService, API.Services.GoogleAuthService _googleAuthService, API.Services.EmailSender _emailSender) 
        {
            this.signInManager = _signInManager;
            this.userManager = _userManager;
            this.facebookAuthService = _facebookAuthService;
            this.googleAuthService = _googleAuthService;
            this.emailSender = _emailSender;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] BLL.Models.LoginRequest loginRequest)
        {
            var result = await signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, false,false);
            if (result.Succeeded)
            {
                var user = userManager.Users.First(item => item.Email == loginRequest.Email);
                return Ok(BLL.Utils.Jwt.GenerateJwtToken(loginRequest.Email, user));
            }
            else
            {
                if (result.IsLockedOut) return Problem("your account is locked out");
                if (result.IsNotAllowed) return Problem("your account is not allowed");
                if (result.RequiresTwoFactor) return Problem("your account is requires two factor");
                return Unauthorized("email or password not correct");
            }
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] BLL.Models.User user)
        {
            user.UserName = user.Email;
            var result = await userManager.CreateAsync(user, user.Password);
            if (result.Succeeded)
            {
                return Ok(BLL.Utils.Jwt.GenerateJwtToken(user.Email, user));
            }
            else
            {
                return Problem(string.Join('\n', result.Errors));
            }
        }
        [HttpPost("Facebook")]
        public async Task<IActionResult> Facebook([FromBody] BLL.Models.Identity.SocialToken userToken)
        {
            try
            {
                var user = await facebookAuthService.Authenticate(userToken);
                await signInManager.SignInAsync(user, true);
                return Ok(BLL.Utils.Jwt.GenerateJwtToken(userToken.Email, user));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Google")]
        public async Task<IActionResult> Google([FromBody] BLL.Models.Identity.SocialToken userToken)
        {
            try
            {
                var user = await googleAuthService.Authenticate(userToken);
                await signInManager.SignInAsync(user, true);
                return Ok(BLL.Utils.Jwt.GenerateJwtToken(userToken.Email, user));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return BadRequest(ex.Message);
            }
        }

        //controller to open forget password page
        [AllowAnonymous]
        [HttpGet("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        //action to send token to this email
        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(BLL.Models.ForgotPassword forgotPassword)
        {
            var user = await userManager.FindByEmailAsync(forgotPassword.Email);
            if (user == null) return RedirectToAction(nameof(ForgotPasswordFailed));

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            var callback = $"{BLL.Settings.Connections.GetServerAddress()}/{Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email })}";
            var message = new Services.Message(user.Email, "Reset Password", callback);
            await emailSender.SendEmailAsync(message);

            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }
        //--- confrmation
        [AllowAnonymous]
        [HttpGet("ForgotPasswordConfirmation")]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        //--- failed
        [AllowAnonymous]
        [HttpGet("ForgotPasswordFailed")]
        public IActionResult ForgotPasswordFailed()
        {
            return View();
        }
        //controller to open reset password page
        [AllowAnonymous]
        [HttpGet("ResetPassword")]
        public IActionResult ResetPassword(string token, string email)
        {
            return View();
        }
        //action provide process to reset password
        [AllowAnonymous]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromForm] BLL.Models.ResetPassword resetPassword)
        {
            var user = await userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null) return RedirectToAction(nameof(ResetPasswordFailed));

            var result = await userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
            if (result.Succeeded) return RedirectToAction(nameof(ResetPasswordConfirmation));

            foreach (var error in result.Errors)
            {
                ModelState.TryAddModelError(error.Code, error.Description);

            }
            return View();
        }
        //--- confrmation
        [AllowAnonymous]
        [HttpGet("ResetPasswordConfirmation")]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        //--- failed
        [AllowAnonymous]
        [HttpGet("ResetPasswordFailed")]
        public IActionResult ResetPasswordFailed()
        {
            return View();
        }
    }
}
