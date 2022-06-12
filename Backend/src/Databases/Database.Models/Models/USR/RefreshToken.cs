using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.USR
{
	[Table("RefreshTokens", Schema = Schema.USER)]
	public class RefreshToken : BaseEntityWithoutKey
	{
        [Key]
        [MaxLength(50)]
        public string Token { get; set; } = null!;

        [MaxLength(100)]
        public string Username { get; set; } = null!;

        public DateTime ExpireDate { get; set; }
    }
}

