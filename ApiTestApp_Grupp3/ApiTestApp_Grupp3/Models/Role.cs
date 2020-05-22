using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTestApp_Grupp3.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        [StringLength(50)]
        public string RoleName { get; set; }
        public EmployeeRole EmployeeRole { get; set; }
    }
}
