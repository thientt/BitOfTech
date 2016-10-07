using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
namespace BitOfTech.WebApi22.Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public class ApplicationUserValidator : UserValidator<ApplicationUser, long>
    {
        List<string> _allowedEmailDomains = new List<string> { "outlook.com", "hotmail.com", "gmail.com", "yahoo.com" };

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationUserValidator"/> class.
        /// </summary>
        /// <param name="appUserManager">The application user manager.</param>
        public ApplicationUserValidator(ApplicationUserManager appUserManager)
            : base(appUserManager)
        {
        }

        /// <summary>
        /// Validates the asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public override async Task<IdentityResult> ValidateAsync(ApplicationUser user)
        {
            IdentityResult result = await base.ValidateAsync(user);

            var emailDomain = user.Email.Split('@')[1];

            if (!_allowedEmailDomains.Contains(emailDomain.ToLower()))
            {
                var errors = result.Errors.ToList();

                errors.Add(String.Format("Email domain '{0}' is not allowed", emailDomain));

                result = new IdentityResult(errors);
            }

            return result;
        }
    }
}