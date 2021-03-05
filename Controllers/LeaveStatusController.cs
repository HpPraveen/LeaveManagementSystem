using LeaveManagementSystem.DAL;
using LeaveManagementSystem.Models;
using LeaveManagementSystem.Services;
using System;
using System.Web.Mvc;

namespace LeaveManagementSystem.Controllers
{
    public class LeaveStatusController : Controller
    {
        private readonly LeaveStatusService _leaveStatusService = new LeaveStatusService(new UnitOfWork(new ApplicationDbContext()));
        private string supervisorCode = "s1";

        public ActionResult Index()
        {
            ViewData["PendingLeave"] = _leaveStatusService.GetPendingLeaveConfirmationsBySupervisor(supervisorCode);
            ViewData["ApprovedRejectedLeave"] = _leaveStatusService.GetLeaveConfirmationsBySupervisor(supervisorCode);

            return View();
        }

        public ActionResult LeaveConfirmation(string employeeCode, string leaveRequestedFromDate, string leaveTypeCode, int noOfLeave, string status, string approverComment)
        {
            var leaveConfirmationViewModel = new LeaveConfirmationViewModel
            {
                EmployeeCode = employeeCode,
                LeaveTypeCode = leaveTypeCode,
                LeaveRequestedFromDate = Convert.ToDateTime(leaveRequestedFromDate).ToString("dd/MM/yyyy"),
                LeaveStatus = status,
                NumberOfLeaves = noOfLeave,
                ApproverComment = approverComment,
                LeaveStatusUpdatedDate = DateTime.Now.Date.ToString(),
                LeaveStatusUpdatedBy = supervisorCode
            };

            var result = _leaveStatusService.UpdateEmployeeLeaveEntry(leaveConfirmationViewModel);

            if (result == true)
            {
                if (status == "Approved")
                {
                    TempData["SuccessMessage"] = "Employee Successfully Approved !";
                }
                else if (status == "Rejected")
                {
                    TempData["SuccessMessage"] = "Employee Successfully Rejected !";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Error !";
            }
            return Redirect(this.Request.UrlReferrer.AbsolutePath);
        }
    }
}