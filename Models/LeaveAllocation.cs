using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveManagementSystem.Models
{
    public class LeaveAllocation
    {
        [Key]
        public long LeaveAllocationId { get; set; }

        public string Year { get; set; }
        public string EmployeeCode { get; set; }
        public string LeaveTypeCode { get; set; }
        public int EntitledLeaveAmount { get; set; }
        public int UtilizedLeaveAmount { get; set; }
        public int RemainingLeaveAmount { get; set; }
    }
}