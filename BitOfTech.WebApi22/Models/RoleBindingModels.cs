using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// 
/// </summary>
namespace BitOfTech.WebApi22.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateRoleBindingModel
    {
        [Required]
        [StringLength(256, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Role Name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class UsersInRoleModel
    {
        public long Id { get; set; }
        public List<long> EnrolledUsers { get; set; }
        public List<long> RemovedUsers { get; set; }
    }
}