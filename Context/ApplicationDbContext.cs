using Microsoft.EntityFrameworkCore;
using NTTest.Models;

namespace NTTest.Context
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Company> Company { get; set; }
        public DbSet<Charges> Charges { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }
    }
}
