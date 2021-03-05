using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaveManagementSystem.Models
{
    public class LeaveConfirmationViewModel
    {
        public string Year { get; set; }
        public string EmployeeName { get; set; }
        public string LeaveType { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeesSupervisorCode { get; set; }
        public string LeaveTypeCode { get; set; }
        public string LeaveRequestedFromDate { get; set; }
        public string LeaveRequestedToDate { get; set; }
        public int NumberOfLeaves { get; set; }
        public string LeaveStatus { get; set; }
        public string LeaveStatusUpdatedBy { get; set; }
        public string LeaveStatusUpdatedDate { get; set; }
        public string EmployeeComment { get; set; }
        public string ApproverComment { get; set; }
    }
}