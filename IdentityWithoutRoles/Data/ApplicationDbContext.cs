using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityWithoutRoles.Data
{
    public class ApplicationUser : IdentityUser<long>
    {
        public long InternalUserId { get; set; }
        //public User InternalUser { get; set; }
    }

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of Users.
        /// </summary>
        public DbSet<ApplicationUser> WebUsers { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of User claims.
        /// </summary>
        public DbSet<IdentityUserClaim<long>> UserClaims { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of User logins.
        /// </summary>
        public DbSet<IdentityUserLogin<long>> UserLogins { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of User tokens.
        /// </summary>
        public DbSet<IdentityUserToken<long>> UserTokens { get; set; }

        /// <summary>
        /// Configures the schema needed for the identity framework.
        /// </summary>
        /// <param name="builder">
        /// The builder being used to construct the model for this context.
        /// </param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(b =>
            {
                b.HasKey(u => u.Id);
                b.HasIndex(u => u.NormalizedUserName).HasName("UserNameIndex").IsUnique();
                b.HasIndex(u => u.NormalizedEmail).HasName("EmailIndex");
                b.ToTable("AspNetUsers");
                b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

                b.Property(u => u.UserName).HasMaxLength(256);
                b.Property(u => u.NormalizedUserName).HasMaxLength(256);
                b.Property(u => u.Email).HasMaxLength(256);
                b.Property(u => u.NormalizedEmail).HasMaxLength(256);

                // Replace with b.HasMany<IdentityUserClaim>().
                b.HasMany<IdentityUserClaim<long>>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
                b.HasMany<IdentityUserLogin<long>>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
                b.HasMany<IdentityUserToken<long>>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
            });

            builder.Entity<IdentityUserClaim<long>>(b =>
            {
                b.HasKey(uc => uc.Id);
                b.ToTable("AspNetUserClaims");
            });

            builder.Entity<IdentityUserLogin<long>>(b =>
            {
                b.HasKey(l => new { l.LoginProvider, l.ProviderKey });
                b.ToTable("AspNetUserLogins");
            });

            builder.Entity<IdentityUserToken<long>>(b =>
            {
                b.HasKey(l => new { l.UserId, l.LoginProvider, l.Name });
                b.ToTable("AspNetUserTokens");
            });
        }
    }
}
