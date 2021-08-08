using BLL.Models;
using BLL.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace API.Services
{
    public class GoogleAuthService : IAuthService
    {
        private readonly UserManager<BLL.Models.User> userManager;
        public GoogleAuthService(UserManager<BLL.Models.User> _userManager)
        {
            this.userManager = _userManager;
        }
        public async Task<User> Authenticate(SocialToken token)
        {
            var payLoad = await ValidateAsync(token.TokenId, new ValidationSettings());
            return await CreateOrGetUser(payLoad, token);
        }
        private async Task<BLL.Models.User> CreateOrGetUser(Payload payload ,BLL.Models.Identity.SocialToken userToken) {
            var user = await userManager.FindByEmailAsync(payload.Email);
            if (user != null) return user;

            var appUser = new BLL.Models.User
            {
                FirstName = userToken.GivenName,
                LastName = userToken.FamilyName,
                Email = userToken.Email,
                OAuthIssuer = payload.Issuer,
                UserName = userToken.Email
            };
            await userManager.CreateAsync(appUser);
            return appUser;
        }
    }
}
