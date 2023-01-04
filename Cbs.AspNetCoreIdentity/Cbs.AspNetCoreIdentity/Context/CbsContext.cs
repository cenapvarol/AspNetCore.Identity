using Cbs.AspNetCoreIdentity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cbs.AspNetCoreIdentity.Context
{
    public class CbsContext : IdentityDbContext<AppUser,AppRole,int>
    {
        public CbsContext(DbContextOptions<CbsContext> options) : base(options)
        {
        }
    }
}
