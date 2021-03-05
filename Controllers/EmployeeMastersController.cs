using LeaveManagementSystem.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace LeaveManagementSystem.Controllers
{
    public class EmployeeMastersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(db.EmployeeMaster.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeId,EmployeeCode,EmployeeName,EmployeeSupervisorCode,EmployeeLeavePacakge")] EmployeeMaster employeeMaster)
        {
            if (employeeMaster.EmployeeCode != null && employeeMaster.EmployeeName != null && employeeMaster.EmployeeSupervisorCode != null &&
                employeeMaster.EmployeeCode != "" && employeeMaster.EmployeeLeavePacakge.ToString() != "" && employeeMaster.EmployeeName != "" && employeeMaster.EmployeeSupervisorCode != "")
            {
                var existingEmployee = db.EmployeeMaster.ToList().Where(e => e.EmployeeCode == employeeMaster.EmployeeCode).ToList();

                if (existingEmployee.Count == 0)
                {
                    if (ModelState.IsValid)
                    {
                        db.EmployeeMaster.Add(employeeMaster);
                        db.SaveChanges();

                        if (employeeMaster.EmployeeLeavePacakge.ToString() == "Office")
                        {
                            var annualLeaveForOffice = new LeaveAllocation
                            {
                                EmployeeCode = employeeMaster.EmployeeCode,
                                LeaveTypeCode = "A0001",
                                Year = DateTime.Now.Year.ToString(),
                                EntitledLeaveAmount = 14,
                                UtilizedLeaveAmount = 0,
                                RemainingLeaveAmount = 14
                            };
                            db.LeaveAllocation.Add(annualLeaveForOffice);
                            db.SaveChanges();

                            var casualLeaveForOffice = new LeaveAllocation
                            {
                                EmployeeCode = employeeMaster.EmployeeCode,
                                LeaveTypeCode = "C0001",
                                Year = DateTime.Now.Year.ToString(),
                                EntitledLeaveAmount = 7,
                                UtilizedLeaveAmount = 0,
                                RemainingLeaveAmount = 7
                            };
                            db.LeaveAllocation.Add(casualLeaveForOffice);
                            db.SaveChanges();

                            var sickLeaveForOffice = new LeaveAllocation
                            {
                                EmployeeCode = employeeMaster.EmployeeCode,
                                LeaveTypeCode = "S0001",
                                Year = DateTime.Now.Year.ToString(),
                                EntitledLeaveAmount = 21,
                                UtilizedLeaveAmount = 0,
                                RemainingLeaveAmount = 21
                            };
                            db.LeaveAllocation.Add(sickLeaveForOffice);
                            db.SaveChanges();
                        }
                        else if (employeeMaster.EmployeeLeavePacakge.ToString() == "Wages")
                        {
                            var annualLeaveForWages = new LeaveAllocation
                            {
                                EmployeeCode = employeeMaster.EmployeeCode,
                                LeaveTypeCode = "A0001",
                                Year = DateTime.Now.Year.ToString(),
                                EntitledLeaveAmount = 10,
                                UtilizedLeaveAmount = 0,
                                RemainingLeaveAmount = 10
                            };
                            db.LeaveAllocation.Add(annualLeaveForWages);
                            db.SaveChanges();

                            var casualLeaveForWages = new LeaveAllocation
                            {
                                EmployeeCode = employeeMaster.EmployeeCode,
                                LeaveTypeCode = "C0001",
                                Year = DateTime.Now.Year.ToString(),
                                EntitledLeaveAmount = 10,
                                UtilizedLeaveAmount = 0,
                                RemainingLeaveAmount = 10
                            };
                            db.LeaveAllocation.Add(casualLeaveForWages);
                            db.SaveChanges();

                            var sickLeaveForWages = new LeaveAllocation
                            {
                                EmployeeCode = employeeMaster.EmployeeCode,
                                LeaveTypeCode = "S0001",
                                Year = DateTime.Now.Year.ToString(),
                                EntitledLeaveAmount = 10,
                                UtilizedLeaveAmount = 0,
                                RemainingLeaveAmount = 10
                            };
                            db.LeaveAllocation.Add(sickLeaveForWages);
                            db.SaveChanges();
                        }

                        return RedirectToAction("Index");
                    }
                    return View(employeeMaster);
                }
                else
                {
                    TempData["ErrorMessage"] = "Employee Exist !!! ";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Please fill all the fields !";
            }
            return View();
        }
    }
}