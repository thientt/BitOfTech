using Microsoft.AspNet.Identity;
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
    public class ApplicationPasswordValidator : PasswordValidator
    {
        /// <summary>
        /// Validates the asynchronous.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public override async Task<IdentityResult> ValidateAsync(string password)
        {
            IdentityResult result = await base.ValidateAsync(password);

            if (password.Contains("abcdef") || password.Contains("123456"))
            {
                var errors = result.Errors.ToList();
                errors.Add("Password can not contain sequence of chars");
                result = new IdentityResult(errors);
            }
            return result;
        }
    }
}