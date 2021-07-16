using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace API.Services
{
    public class FacebookAuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<BLL.Models.User> _userManager;

        private string AppId = "814293879114232";
        private string AppSecret = "f9f2412a01807c6888875afe3da69fc5";
        public FacebookAuthService(IConfiguration configuration, UserManager<BLL.Models.User> userManager)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<BLL.Models.User> Authenticate(BLL.Models.UserToken token)
        {
            //NB: These config files are loaded from the user secrets.
            var appToken = await GenerateAppAccessToken(AppId, AppSecret);
            var isValid = await DebugUserAccessToken(appToken, token.TokenId);

            if (isValid)
            {
                var user = await CreateOrGetUser(token);
                return user;
            }

            throw new Exception("Invalid Token");
        }

        private async Task<BLL.Models.User> CreateOrGetUser(BLL.Models.UserToken userToken)
        {
            var user = await _userManager.FindByEmailAsync(userToken.Email);

            if (user == null)
            {
                var appUser = new BLL.Models.User
                {
                    FirstName = userToken.Name,
                    LastName = userToken.FamilyName,
                    Email = userToken.Email,
                    OAuthIssuer = "facebook",
                    UserName = userToken.FamilyName + "_" + userToken.GivenName
                };
                await _userManager.CreateAsync(appUser);
                return appUser;
            }
            return user;
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
