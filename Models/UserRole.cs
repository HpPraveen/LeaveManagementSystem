using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveManagementSystem.Models
{
    public class UserRole
    {
        [Key]
        public long RoleId { get; set; }

        public long UserId { get; set; }

        public string Role { get; set; }
    }
}