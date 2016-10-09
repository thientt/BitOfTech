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
        private const string connOthIdentity = "OAuthIdentity";

        public ApplicationDbContext()
            : base(connOthIdentity)
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
        private const string TABLE_USER = "User";
        private const string TABLE_ROLE = "Role";
        private const string TABLE_USE_RROLE = "UserRole";
        private const string TABLE_CLAIM = "Claim";
        private const string TABLE_LOGIN = "Login";

        private const string PRIMARY_KEY_USER = "UserId";
        private const string PRIMRY_KEY_ROLE = "RoleId";
        private const string PRIMARY_KEY_CLAIM = "ClaimId";

        public ApplicationIdentityDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().
                ToTable(TABLE_USER).
                Property<long>(x => x.Id).
                HasColumnName(PRIMARY_KEY_USER).
                HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ApplicationRole>().
                ToTable(TABLE_ROLE).
                Property<long>(x => x.Id).
                HasColumnName(PRIMRY_KEY_ROLE).
                HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ApplicationUserRole>().
                ToTable(TABLE_USE_RROLE);
            modelBuilder.Entity<ApplicationUserClaim>().
                ToTable(TABLE_CLAIM).
                Property<long>(x => x.Id).
                HasColumnName(PRIMARY_KEY_CLAIM).
                HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<ApplicationUserLogin>().
                ToTable(TABLE_LOGIN);
        }
    }
}