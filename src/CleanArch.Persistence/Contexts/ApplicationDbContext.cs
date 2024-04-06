using CleanArch.Application.Abstractions.Infrastructure.Services;
using CleanArch.Domain.Common;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Entities.Identity;
using CleanArch.Persistence.Configurations;
using CleanArch.Persistence.Configurations.Identity;
using CleanArch.Persistence.Contexts.Seeds.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.Persistence.Contexts
{
    public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string,
            IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
            IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        private readonly IDateTimeService _dateTime;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeService dateTime, IAuthenticatedUserService authenticatedUser) : base(options)
        {
            //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _dateTime = dateTime;
            _authenticatedUser = authenticatedUser;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies();
        }

        #region Entities
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        #endregion

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = _dateTime.NowUtc;
                        entry.Entity.CreatedBy = _authenticatedUser.UserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedDate = _dateTime.NowUtc;
                        entry.Entity.UpdatedBy = _authenticatedUser.UserId;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("dbo");

            #region Identity

            builder.Entity<ApplicationUser>().ToTable(name: "Users", "Identity");
            builder.Entity<ApplicationRole>().ToTable("Roles", "Identity");
            builder.Entity<ApplicationUserRole>().ToTable("UserRoles", "Identity");
            builder.Entity<RefreshToken>().ToTable(name: "RefreshTokens", "Identity");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "Identity"); //external logins
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "Identity");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "Identity");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "Identity");

            #endregion

            #region Seed

            //builder.IdentitySeed();

            #endregion

            foreach (var relationship in builder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.NoAction;
            }

            #region Configurations

            // builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            builder.ApplyConfiguration(new CustomerConfiguration());
            builder.ApplyConfiguration(new OrderConfiguration());
            builder.ApplyConfiguration(new OrderItemConfiguration());
            builder.ApplyConfiguration(new ProductConfiguration());
            builder.ApplyConfiguration(new SupplierConfiguration());

            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());
            builder.ApplyConfiguration(new RefreshTokenConfiguration());

            #endregion

            foreach (var property in builder.Model.GetEntityTypes()
                         .SelectMany(t => t.GetProperties())
                         .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,6)");
            }
        }
    }
}
