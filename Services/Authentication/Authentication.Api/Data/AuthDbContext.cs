using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Authentication.Api.Data
{
    public class AuthDbContext : DbContext
    {
        public DbSet<AuthUser> Users => Set<AuthUser>();

        public AuthDbContext(DbContextOptions<AuthDbContext> options)
            : base(options) { }
    }
}
