using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

/// <summary>
/// 
/// </summary>
namespace BitOfTech.WebApi22.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationDbContext : ApplicationIdentityDbContext
    {
        public ApplicationDbContext()
            : base("OAuthIdentity")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = true;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ApplicationIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationIdentityDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().
                ToTable("User").
                Property<long>(x => x.Id).
                HasColumnName("UserId").
                HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ApplicationRole>().
                ToTable("Role").
                Property<long>(x => x.Id).
                HasColumnName("RoleId").
                HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ApplicationUserRole>().
                ToTable("UserRole");
            modelBuilder.Entity<ApplicationUserClaim>().
                ToTable("Claim").
                Property<long>(x => x.Id).
                HasColumnName("ClaimId").
                HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ApplicationUserLogin>().
                ToTable("Login");
        }
    }
}