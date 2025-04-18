using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace SafeNetAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Column("Login")]
        public string Login { get; set; }
        [Column("Token")]
        public string Token { get; set; }
    }
}
