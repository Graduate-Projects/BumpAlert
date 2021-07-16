using Google.Apis.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace API.Services
{
    public interface IAuthService
    {
        Task<BLL.Models.User> Authenticate(BLL.Models.UserToken token);
    }
}
