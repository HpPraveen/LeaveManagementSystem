using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveManagementSystem.Models
{
    public class EmployeeMaster
    {
        [Key]
        public long EmployeeId { get; set; }

        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeSupervisorCode { get; set; }
        public string EmployeeLeavePacakge { get; set; }
    }
}