using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpManageApp.Domain.Entities
{
    public class LoginModel
    {

        [Required]
        public string Id { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
