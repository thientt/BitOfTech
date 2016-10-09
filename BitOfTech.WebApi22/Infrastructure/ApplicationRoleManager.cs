using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Data.Entity;

namespace BitOfTech.WebApi22.Infrastructure
{
    public class ApplicationRoleManager : RoleManager<ApplicationRole, long>
    {
        public ApplicationRoleManager(IRoleStore<ApplicationRole, long> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            var appRoleManager = new ApplicationRoleManager(new ApplicationStoreRole(context.Get<ApplicationDbContext>()));

            return appRoleManager;
        }
    }

    public class ApplicationStoreRole : RoleStore<ApplicationRole, long, ApplicationUserRole>
    {
        public ApplicationStoreRole(DbContext dbContext)
            : base(dbContext)
        {

        }
    }
}