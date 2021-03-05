using LeaveManagementSystem.DAL;
using LeaveManagementSystem.Models;
using LeaveManagementSystem.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace LeaveManagementSystem.Controllers
{
    public class LeaveRequestController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly LeaveRequestService _leaveRequestService = new LeaveRequestService(new UnitOfWork(new ApplicationDbContext()));

        private List<SelectListItem> ddlYears = new List<SelectListItem>();
        private string employeeCode = "e1";

        public ActionResult Create(int? Year, string LeaveTypeCode)
        {
            int? year = Year;
            string leaveTypeCode = LeaveTypeCode;
            if (year == null)
            {
                year = DateTime.Now.Year;
            }
            if (leaveTypeCode == null)
            {
                leaveTypeCode = "A001";
            }

            ViewData["LeaveHistory"] = _leaveRequestService.GetAllLeaveByEmployee(employeeCode, year);
            ViewData["LeavePackage"] = _leaveRequestService.GetLeavePackageByEmployee(employeeCode);
            ViewData["LeaveTypes"] = _leaveRequestService.GetAllLeaveTypes();
            ViewData["Supervisor"] = _leaveRequestService.GetSupervisorByEmployee(employeeCode);
            ViewBag.RemainingLeave = _leaveRequestService.GetRemainingLeaveAmountByEmployee(employeeCode, year, leaveTypeCode);
            ViewBag.linktoYearId = GetYears(year);

            return View();
        }

        private SelectList GetYears(int? iSelectedYear)
        {
            int CurrentYear = DateTime.Now.Year;

            for (int i = 2010; i <= CurrentYear; i++)
            {
                ddlYears.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }
            return new SelectList(ddlYears, "Value", "Text", iSelectedYear);
        }

        public ActionResult GetLeaveTypeCode(int year, string leaveTypeCode)
        {
            var remainingLeave = _leaveRequestService.GetRemainingLeaveAmountByEmployee(employeeCode, year, leaveTypeCode);

            return Json(remainingLeave, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LeaveApply(string leaveType, string requestedDate, int? leaveDays, string reason)
        {
            if (leaveType != null && requestedDate != null && leaveDays != null && reason != null)
            {
            }
            else
            {
                TempData["ErrorMessage"] = "Please fill all required fields !";
            }
            //var leaveConfirmationViewModel = new LeaveConfirmationViewModel
            //{
            //    EmployeeCode = employeeCode,
            //    LeaveTypeCode = leaveTypeCode,
            //    LeaveRequestedFromDate = leaveRequestedFromDate,
            //    LeaveStatus = status,
            //    NumberOfLeaves = noOfLeave,
            //    ApproverComment = approverComment,
            //    LeaveStatusUpdatedDate = DateTime.Now.Date.ToString(),
            //    LeaveStatusUpdatedBy = supervisorCode
            //};

            //var result = _leaveStatusService.UpdateEmployeeLeaveEntry(leaveConfirmationViewModel);

            //if (result == true)
            //{
            //    if (status == "Approved")
            //    {
            //        TempData["SuccessMessage"] = "Employee Successfully Approved !";
            //    }
            //    else if (status == "Rejected")
            //    {
            //        TempData["SuccessMessage"] = "Employee Successfully Rejected !";
            //    }
            //}
            //else
            //{
            //    TempData["ErrorMessage"] = "Error !";
            //}
            return Redirect(this.Request.UrlReferrer.AbsolutePath);
        }
    }
}