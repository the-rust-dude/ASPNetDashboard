using ASPNetDashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace ASPNetDashboard.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<LoginUser> LoginUsers { get; set; }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<TransactionModel> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<LoginUser>(e =>
            {
                e.HasIndex(u => u.Email).IsUnique();
                e.Property(u => u.Role).HasDefaultValue("Viewer");
            });

            modelBuilder.Entity<UserModel>(e =>
            {
                e.HasIndex(u => u.Email).IsUnique();
            });

            modelBuilder.Entity<TransactionModel>(e =>
            {
                e.Property(t => t.Status).HasDefaultValue("Completed");
            });
        }
    }
}
