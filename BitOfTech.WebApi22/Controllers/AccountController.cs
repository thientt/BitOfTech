using BitOfTech.WebApi22.Infrastructure;
using BitOfTech.WebApi22.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using BitOfTech.WebApi22.Utility;

/// <summary>
/// 
/// </summary>
namespace BitOfTech.WebApi22.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix(RouteNameConfig.API_USER)]
    public class AccountController : BaseApiController
    {
        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = UserRole.ADMIN)]
        [Route(RouteNameConfig.ROUTE_TEMPLATE_USER)]
        [HttpGet]
        public IHttpActionResult GetUsers()
        {
            return Ok(this.AppUserManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));
        }

        /// <summary>
        /// Gets the user.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route(RouteNameConfig.ROUTE_GET_USER_BY_ID, Name = RouteNameConfig.ROUTE_NAME_GET_USER_BY_ID)]
        [Authorize(Roles = UserRole.ADMIN)]
        public async Task<IHttpActionResult> GetUser(long Id)
        {
            var user = await this.AppUserManager.FindByIdAsync(Id);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        /// <summary>
        /// Gets the name of the user by.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        [Route(RouteNameConfig.ROUTE_GET_USER_BY_USER_NAME)]
        [HttpGet]
        [Authorize(Roles = UserRole.ADMIN)]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            var user = await this.AppUserManager.FindByNameAsync(username);

            if (user != null)
            {
                return Ok(this.TheModelFactory.Create(user));
            }

            return NotFound();

        }

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="createUserModel">The create user model.</param>
        /// <returns></returns>
        [Route(RouteNameConfig.ROUTE_TEMPLATE_CREATE)]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CreateUser(CreateUserBindingModel createUserModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new ApplicationUser()
                {
                    UserName = createUserModel.Username,
                    Email = createUserModel.Email,
                    FirstName = createUserModel.FirstName,
                    LastName = createUserModel.LastName,
                    Level = 3,
                    JoinDate = DateTime.Now.Date,
                    IsDeleted = false,
                    CreatedBy = "admin",
                    Created = DateTime.Now
                };

                IdentityResult addUserResult = await this.AppUserManager.CreateAsync(user, createUserModel.Password);

                if (!addUserResult.Succeeded)
                {
                    return GetErrorResult(addUserResult);
                }
                string code = await this.AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code }));
                await this.AppUserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));

                return Created(locationHeader, TheModelFactory.Create(user));
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Confirms the email.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route(RouteNameConfig.ROUTE_CONFIRM_EMAIL, Name = RouteNameConfig.ROUTE_NAME_CONFIRM_EMAIL)]
        public async Task<IHttpActionResult> ConfirmEmail(long userId = 0, string code = "")
        {
            if (userId == 0 || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.AppUserManager.ConfirmEmailAsync(userId, code);

            if (result.Succeeded)
            {
                return Ok();
            }
            else
            {
                return GetErrorResult(result);
            }
        }

        [Route(RouteNameConfig.ROUTE_CHANGE_PASSWORD)]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.AppUserManager.ChangePasswordAsync(User.Identity.GetUserId<int>(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        [Route(RouteNameConfig.ROUTE_GET_USER_BY_ID)]
        [HttpDelete]
        [Authorize(Roles = UserRole.ADMIN)]
        public async Task<IHttpActionResult> DeleteUser(long id)
        {
            //Only SuperAdmin or Admin can delete users (Later when implement roles)

            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser != null)
            {
                IdentityResult result = await this.AppUserManager.DeleteAsync(appUser);

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }

                return Ok();

            }

            return NotFound();

        }

        [Authorize(Roles = UserRole.ADMIN)]
        [Route(RouteNameConfig.ROUTE_GET_ROLES_BY_USER_ID)]
        [HttpPut]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] long id, [FromBody] string[] rolesToAssign)
        {
            var appUser = await this.AppUserManager.FindByIdAsync(id);

            if (appUser == null)
                return NotFound();

            var currentRoles = await this.AppUserManager.GetRolesAsync(appUser.Id);

            var rolesNotExists = rolesToAssign.Except(this.AppRoleManager.Roles.Select(x => x.Name)).ToArray();
            if (rolesNotExists.Count() > 0)
            {
                ModelState.AddModelError("", string.Format("Roles '{0}' does not exixts in the system", string.Join(",", rolesNotExists)));
                return BadRequest(ModelState);
            }
            IdentityResult removeResult = await this.AppUserManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());
            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to remove user roles");
                return BadRequest(ModelState);
            }

            IdentityResult addResult = await this.AppUserManager.AddToRolesAsync(appUser.Id, rolesToAssign);

            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to add user roles");
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}