using BitOfTech.WebApi22.Infrastructure;
using BitOfTech.WebApi22.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Net.Http;
using System.Web.Http;

/// <summary>
/// 
/// </summary>
namespace BitOfTech.WebApi22.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseApiController : ApiController
    {
        private ModelFactory _modelFactory;
        private ApplicationUserManager _appUserManager = null;
        private ApplicationRoleManager _appRoleManager = null;
        /// <summary>
        /// Gets the application user manager.
        /// </summary>
        /// <value>
        /// The application user manager.
        /// </value>
        protected ApplicationUserManager AppUserManager
        {
            get
            {
                return _appUserManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }


        /// <summary>
        /// Gets the application role manage.
        /// </summary>
        /// <value>
        /// The application role manage.
        /// </value>
        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _appRoleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApiController"/> class.
        /// </summary>
        public BaseApiController()
        {
        }

        /// <summary>
        /// Gets the model factory.
        /// </summary>
        /// <value>
        /// The model factory.
        /// </value>
        protected ModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(this.Request, this.AppUserManager);
                }
                return _modelFactory;
            }
        }

        /// <summary>
        /// Gets the error result.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
