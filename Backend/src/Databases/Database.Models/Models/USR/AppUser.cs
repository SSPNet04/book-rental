using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.USR
{
    [Table("AspNetUsers", Schema = Schema.USER)]
    public class AppUser : IdentityUser, ISoftDeleteEntity
    {
        [Required]
        [MaxLength(200)]
        public virtual string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public virtual string LastName { get; set; } = null!;

        [MaxLength(1000)]
        public virtual string FullName { get; set; } = null!;

        [MaxLength(200)]
        public virtual string? MidName { get; set; }

        [MaxLength(20)]
        public virtual string? TelNo { get; set; }

        [MaxLength(20)]
        public virtual string? Mobile { get; set; }

        public bool IsDeleted { get; set; }
    }
}
