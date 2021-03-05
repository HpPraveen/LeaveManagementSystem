using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveManagementSystem.Models
{
    public class LeaveEntry
    {
        [Key]
        public long LeaveEntryId { get; set; }

        public string EmployeeCode { get; set; }
        public string EmployeesSupervisorCode { get; set; }
        public string LeaveTypeCode { get; set; }
        public DateTime LeaveRequestedFromDate { get; set; }
        public DateTime LeaveRequestedToDate { get; set; }
        public int NumberOfLeaves { get; set; }
        public string LeaveStatus { get; set; }
        public string LeaveStatusUpdatedBy { get; set; }
        public string LeaveStatusUpdatedDate { get; set; }
        public string EmployeeComment { get; set; }
        public string ApproverComment { get; set; }
    }
}