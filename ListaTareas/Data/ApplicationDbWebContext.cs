using ListaTareas.Models.Todo;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace ListaTareas.Data
{
    public class ApplicationDbWebContext : IdentityDbContext
    {
        public ApplicationDbWebContext(DbContextOptions<ApplicationDbWebContext> options)
            : base (options) { }

        public DbSet<Todo> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Todo>()
            .ToTable(tb => tb.HasTrigger("someTrigger"));
        }
    }
}
