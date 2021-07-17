using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IAuthService
    {
        Task<BLL.Models.User> Authenticate(BLL.Models.Identity.SocialToken token);
    }
}
