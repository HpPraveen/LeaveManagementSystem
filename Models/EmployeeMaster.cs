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
        [RegularExpression("(^E[0-9]{4})", ErrorMessage = "Invalid Employee Code. (Start with capital E and use 4 digits. example employee code = E000)")]
        public string EmployeeCode { get; set; }

        [DisplayName("Employee Name")]
        public string EmployeeName { get; set; }

        [DisplayName("Supervisor Code")]
        [RegularExpression("(^S[0-9]{4})", ErrorMessage = "Invalid Supervisor Code. (Start with capital S and use 4 digits. example supervisor code = S000)")]
        public string EmployeeSupervisorCode { get; set; }

        [DisplayName("Leave Package")]
        public LeavePacakge EmployeeLeavePacakge { get; set; }
    }

    public enum LeavePacakge
    {
        Wages,
        Office
    }
}