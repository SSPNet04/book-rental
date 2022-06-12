using Database.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
    public class DatabaseContext : IdentityDbContext<USR.AppUser>
    {
        private readonly IHttpContextAccessor? HttpContextAccessor;

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            this.HttpContextAccessor = httpContextAccessor;
        }

        public DbSet<USR.RefreshToken> RefreshTokens => Set<USR.RefreshToken>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<RentalHistory> RentalHistory => Set<RentalHistory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var type in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeleteEntity).IsAssignableFrom(type.ClrType))
                    modelBuilder.SetSoftDeleteFilter(type.ClrType);
            }

            modelBuilder.Entity<Book>()
                   .Property(t => t.BookName)
                   .IsRequired();

            modelBuilder.Entity<Customer>()
                   .Property(t => t.CustomerName)
                   .IsRequired();

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            AddAuditInfo();
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            AddAuditInfo();
            return await base.SaveChangesAsync();
        }

        private void AddAuditInfo(string? by = null)
        {
            //Get user ID
            var username = HttpContextAccessor?.HttpContext?.User?.Claims.Where(x => x.Type == "userid").SingleOrDefault();
            by = username?.Value;

            var entries = ChangeTracker.Entries().Where(x => (x.Entity is BaseEntity || x.Entity is BaseEntityWithoutKey) && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                if (entry.Entity is BaseEntity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        ((BaseEntity)entry.Entity).Created = DateTime.Now;
                        if (by != null)
                        {
                            ((BaseEntity)entry.Entity).CreatedBy = by;
                        }
                    }
                    ((BaseEntity)entry.Entity).Updated = DateTime.Now;
                    if (by != null)
                    {
                        ((BaseEntity)entry.Entity).UpdatedBy = by;
                    }
                }
                else if (entry.Entity is BaseEntityWithoutKey)
                {
                    if (entry.State == EntityState.Added)
                    {
                        ((BaseEntityWithoutKey)entry.Entity).Created = DateTime.Now;
                        if (by != null)
                        {
                            ((BaseEntityWithoutKey)entry.Entity).CreatedBy = by;
                        }
                    }
                    ((BaseEntityWithoutKey)entry.Entity).Updated = DateTime.Now;
                    if (by != null)
                    {
                        ((BaseEntityWithoutKey)entry.Entity).UpdatedBy = by;
                    }
                }
            }
        }

    }
}
