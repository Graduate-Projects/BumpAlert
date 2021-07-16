using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace API.Services
{
    public class GoogleAuthService : IAuthService
    {
        private readonly UserManager<BLL.Models.User> _userManager;

        public GoogleAuthService(UserManager<BLL.Models.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<BLL.Models.User> Authenticate(BLL.Models.UserToken userTokenModel)
        {
            var payload = await ValidateAsync(userTokenModel.TokenId, new ValidationSettings());
            return await CreateOrGetUser(payload, userTokenModel);
        }

        private async Task<BLL.Models.User> CreateOrGetUser(Payload payload, BLL.Models.UserToken userToken)
        {
            var user = await _userManager.FindByEmailAsync(payload.Email);

            if (user == null)
            {
                var appUser = new BLL.Models.User
                {
                    FirstName = userToken.Name,
                    LastName = userToken.FamilyName,
                    Email = userToken.Email,
                    OAuthIssuer = payload.Issuer,
                    OAuthSubject = payload.Subject,
                    UserName = userToken.Name.Replace(" ", "_")
                };
                await _userManager.CreateAsync(appUser);
                return appUser;
            }

            return user;
        }
    }
}
