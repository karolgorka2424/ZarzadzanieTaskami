using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZarzadzanieTaskami.Models;

namespace ZarzadzanieTaskami.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ZarzadzanieTaskami.Models.Projekt> Projekt { get; set; } = default!;
        public DbSet<ZarzadzanieTaskami.Models.ProjectTask> ProjectTask { get; set; } = default!;
        public DbSet<ZarzadzanieTaskami.Models.Komentarz> Komentarz { get; set; } = default!;
    }
}
