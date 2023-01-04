using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cbs.AspNetCoreIdentity.Models
{
    public class RoleModel
    {
        [Required(ErrorMessage ="Role  adı gereklidir")]
        public string Name { get; set; }
    }
}
