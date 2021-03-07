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
            ViewData["EmployeeType"] = Enum.GetValues(typeof(EmployeeType));
            ViewData["Supervisor"] = db.EmployeeMaster.ToList().Where(e => e.EmployeeType == EmployeeType.Supervisor);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeId,EmployeeCode,EmployeeName,EmployeeSupervisorCode,EmployeeLeavePacakge")] EmployeeMaster employeeMaster, string employeeType, string supervisorCode)
        {
            var supervisor_Code = supervisorCode;
            if (supervisor_Code == null || supervisor_Code == "")
            {
                supervisor_Code = employeeMaster.EmployeeCode;
            }

            if (employeeMaster.EmployeeCode != null && employeeMaster.EmployeeName != null && supervisor_Code != null &&
                employeeMaster.EmployeeCode != "" && employeeMaster.EmployeeLeavePacakge.ToString() != "" && employeeMaster.EmployeeName != "" && supervisor_Code != "")
            {
                var existingEmployee = db.EmployeeMaster.ToList().Where(e => e.EmployeeCode == employeeMaster.EmployeeCode || e.EmployeeName == employeeMaster.EmployeeName).ToList();

                if (existingEmployee.Count == 0)
                {
                    if (ModelState.IsValid)
                    {
                        EmployeeType empType = (EmployeeType)Enum.Parse(typeof(EmployeeType), employeeType);
                        var empMaster = new EmployeeMaster
                        {
                            EmployeeCode = employeeMaster.EmployeeCode,
                            EmployeeName = employeeMaster.EmployeeName,
                            EmployeeLeavePacakge = employeeMaster.EmployeeLeavePacakge,
                            EmployeeType = empType,
                            EmployeeSupervisorCode = supervisor_Code
                        };
                        db.EmployeeMaster.Add(empMaster);
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

                        var newUser = new User
                        {
                            Username = employeeMaster.EmployeeCode,
                            Password = employeeMaster.EmployeeCode
                        };
                        db.User.Add(newUser);
                        db.SaveChanges();

                        var userId = db.User.ToList().Where(e => e.Username == employeeMaster.EmployeeCode).FirstOrDefault().UserId;

                        if (employeeType == "Normal")
                        {
                            var newUserRole = new UserRole
                            {
                                UserId = userId,
                                Role = employeeType
                            };
                            db.UserRole.Add(newUserRole);
                            db.SaveChanges();
                        }
                        else
                        {
                            var newUserRole = new UserRole
                            {
                                UserId = userId,
                                Role = employeeType
                            };
                            db.UserRole.Add(newUserRole);
                            db.SaveChanges();
                        }
                        return RedirectToAction("Index");
                    }
                    ViewData["EmployeeType"] = Enum.GetValues(typeof(EmployeeType));
                    ViewData["Supervisor"] = db.EmployeeMaster.ToList();
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
            ViewData["EmployeeType"] = Enum.GetValues(typeof(EmployeeType));
            ViewData["Supervisor"] = db.EmployeeMaster.ToList();
            return View();
        }
    }
}