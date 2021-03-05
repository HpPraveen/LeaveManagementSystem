using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveManagementSystem.Models
{
    public class EmployeeMaster
    {
        [Key]
        public long EmployeeId { get; set; }

        [DisplayName("Employee Code")]
        public string EmployeeCode { get; set; }

        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }

        [DisplayName("Supervisor Code")]
        public string EmployeeSupervisorCode { get; set; }

        [DisplayName("Leave Package")]
        public string EmployeeLeavePacakge { get; set; }
    }
}