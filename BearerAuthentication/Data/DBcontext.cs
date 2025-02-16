using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BearerAuthentication.Data
{
    public class DBcontext : DbContext
    {
        public DBcontext(DbContextOptions<DBcontext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
    }
}
