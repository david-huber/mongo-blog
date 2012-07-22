using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mongo.Blog.Models
{
    public class SignIn
    {
        [Required, DisplayName("User name:")]
        public string UserName { get; set; }
        
        [Required, DisplayName("Password:")]
        public string Password { get; set; }
    }
}