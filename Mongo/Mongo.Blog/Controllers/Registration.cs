using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mongo.Blog.Controllers
{
    public class Registration
    {
        [Required]
        [DisplayName("User name:")]
        public string UserName { get; set; }
        [Required]
        [DisplayName("Password:")]
        public string Password { get; set; }
        [Required]
        [DisplayName("Confirm password:")]
        public string ConfirmPassword { get; set; }
        [Required]
        [DisplayName("Email address:")]
        public string Email { get; set; }
    }
}