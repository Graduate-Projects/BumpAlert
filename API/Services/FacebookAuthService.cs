using BLL.Models;
using BLL.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace API.Services
{
    public class FacebookAuthService : IAuthService
    {
        private readonly UserManager<BLL.Models.User> userManager;
        public FacebookAuthService(UserManager<BLL.Models.User> _userManager)
        {
            this.userManager = _userManager;
        }
        public async Task<User> Authenticate(SocialToken token)
        {
            var AccessToken = await GenerateAppAccessToken(BLL.Constants.Facebook.AppId, BLL.Constants.Facebook.AppSecret);
            var IsValidAccessToken = await DebugUserAccessToken(AccessToken, token.TokenId);
            if (IsValidAccessToken)
            {
                var user = await CreateOrGetUser(token);
                return user;
            }
            throw new Exception("Invalid Token");
        }
        private async Task<BLL.Models.User> CreateOrGetUser(BLL.Models.Identity.SocialToken userToken) {
            var user = await userManager.FindByEmailAsync(userToken.Email);
            if (user != null) return user;

            var appUser = new BLL.Models.User
            {
                FirstName = userToken.GivenName,
                LastName = userToken.FamilyName,
                Email = userToken.Email,
                OAuthIssuer = "facebook",
                UserName = userToken.Email
            };
            await userManager.CreateAsync(appUser);
            return appUser;
        }
        private async Task<string> GenerateAppAccessToken(string appId, string appSecret)
        {
            using var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync($@"https://graph.facebook.com/oauth/access_token?client_id={appId}&client_secret={appSecret}&grant_type=client_credentials");
            var obj = JsonConvert.DeserializeAnonymousType(json, new { access_token = "", token_type = "" });
            return obj.access_token;
        }
        private async Task<bool> DebugUserAccessToken(string appAccessToken, string userAccessToken)
        {
            using var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync($@"https://graph.facebook.com/debug_token?input_token={userAccessToken}&access_token={appAccessToken}");
            var obj = JsonConvert.DeserializeAnonymousType(json, new { data = new { app_id = "", is_valid = false } });
            return obj.data.is_valid;
        }
    }
}
