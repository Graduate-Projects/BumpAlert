using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class APIContext : IdentityDbContext<BLL.Models.User>
    {
        public DbSet<BLL.Models.Danger> Dangers { get; set; }
        public DbSet<BLL.Models.Suggestion> Suggestions { get; set; }
        public APIContext() : base()
        {

        }
        public APIContext(DbContextOptions options):base(options)
        {
                
        }
    }
}
