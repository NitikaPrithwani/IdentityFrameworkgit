using IdentityFramework.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityFramework.Models
{
    public class IdentityFrameworkDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public IdentityFrameworkDbContext(DbContextOptions<IdentityFrameworkDbContext> options)
            : base(options)
        {
        }
        public DbSet<Books> Books { get; set; }
        public DbSet<BorrowingRecords> BorrowingRecords { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<Authors> Authors { get; set; }
    }
}