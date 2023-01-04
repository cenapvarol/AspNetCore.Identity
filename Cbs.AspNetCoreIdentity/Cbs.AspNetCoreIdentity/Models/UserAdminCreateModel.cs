using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cbs.AspNetCoreIdentity.Models
{
    public class UserAdminCreateModel
    {
        [Required(ErrorMessage ="Kullanıcı adı gereklidir")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email adı gereklidir")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Cinsiyet adı gereklidir")]
        public string Gender { get; set; }
    }
}
