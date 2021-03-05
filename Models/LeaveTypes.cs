using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveManagementSystem.Models
{
    public class LeaveTypes
    {
        [Key]
        public long LeaveTypeId { get; set; }

        public string LeaveTypeCode { get; set; }
        public string LeaveTypeDescription { get; set; }
    }
}