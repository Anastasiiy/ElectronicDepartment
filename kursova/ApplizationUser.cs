using ElectronicDepartment.Common.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ElectronicDepartment.DomainEntities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(128)]
        public string FirstName { get; set; } = default!;

        [Required]
        [MaxLength(128)]
        public string LastName { get; set; } = default!;

        [Required]
        [MaxLength(128)]
        public string MiddleName { get; set; } = default!;

        public DateTime BirthDay { get; set; }

        public Gender Gender { get; set; }

        public UserType UserType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime DeletedAt { get; set; } = DateTime.MinValue;
    }
}