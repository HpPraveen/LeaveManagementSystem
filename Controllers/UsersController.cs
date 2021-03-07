using LeaveManagementSystem.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace LeaveManagementSystem.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public ActionResult Create()
        {
            #region
            if (!_context.Roles.Any(x => x.Name == "Supervisor"))
            {
                _context.Roles.Add(new IdentityRole("Supervisor"));
                _context.SaveChanges();
            }
            if (!_context.Roles.Any(x => x.Name == "Employee"))
            {
                _context.Roles.Add(new IdentityRole("Employee"));
                _context.SaveChanges();
            }
            if (!_context.Roles.Any(x => x.Name == "Manager"))
            {
                _context.Roles.Add(new IdentityRole("Manager"));
                _context.SaveChanges();
            }

            if (!_context.LeaveTypes.Any(x => x.LeaveTypeCode == "A0001"))
            {
                var annualLeaveType = new LeaveTypes
                {
                    LeaveTypeCode = "A0001",
                    LeaveTypeDescription = "Annual"
                };
                _context.LeaveTypes.Add(annualLeaveType);
                _context.SaveChanges();
            }
            if (!_context.LeaveTypes.Any(x => x.LeaveTypeCode == "C0001"))
            {
                var casualLeaveType = new LeaveTypes
                {
                    LeaveTypeCode = "C0001",
                    LeaveTypeDescription = "Casual"
                };
                _context.LeaveTypes.Add(casualLeaveType);
                _context.SaveChanges();
            }
            if (!_context.LeaveTypes.Any(x => x.LeaveTypeCode == "S0001"))
            {
                var sickLeaveType = new LeaveTypes
                {
                    LeaveTypeCode = "S0001",
                    LeaveTypeDescription = "Sick"
                };
                _context.LeaveTypes.Add(sickLeaveType);
                _context.SaveChanges();
            }

            if (!_context.EmployeeMaster.Any(x => x.EmployeeCode == "E0001"))
            {
                var employeeMaster = new EmployeeMaster
                {
                    EmployeeCode = "E0001",
                    EmployeeName = "Test Supervisor",
                    EmployeeLeavePacakge = LeavePacakge.Wages,
                    EmployeeType = EmployeeType.Supervisor,
                    EmployeeSupervisorCode = "E0001"
                };
                _context.EmployeeMaster.Add(employeeMaster);
                _context.SaveChanges();

                var annualLeaveForOffice = new LeaveAllocation
                {
                    EmployeeCode = employeeMaster.EmployeeCode,
                    LeaveTypeCode = "A0001",
                    Year = DateTime.Now.Year.ToString(),
                    EntitledLeaveAmount = 10,
                    UtilizedLeaveAmount = 0,
                    RemainingLeaveAmount = 10
                };
                _context.LeaveAllocation.Add(annualLeaveForOffice);
                _context.SaveChanges();

                var casualLeaveForOffice = new LeaveAllocation
                {
                    EmployeeCode = employeeMaster.EmployeeCode,
                    LeaveTypeCode = "C0001",
                    Year = DateTime.Now.Year.ToString(),
                    EntitledLeaveAmount = 10,
                    UtilizedLeaveAmount = 0,
                    RemainingLeaveAmount = 10
                };
                _context.LeaveAllocation.Add(casualLeaveForOffice);
                _context.SaveChanges();

                var sickLeaveForOffice = new LeaveAllocation
                {
                    EmployeeCode = employeeMaster.EmployeeCode,
                    LeaveTypeCode = "S0001",
                    Year = DateTime.Now.Year.ToString(),
                    EntitledLeaveAmount = 10,
                    UtilizedLeaveAmount = 0,
                    RemainingLeaveAmount = 10
                };
                _context.LeaveAllocation.Add(sickLeaveForOffice);
                _context.SaveChanges();

                var newUser = new User
                {
                    Username = employeeMaster.EmployeeCode,
                    Password = employeeMaster.EmployeeCode
                };
                _context.User.Add(newUser);
                _context.SaveChanges();

                var userId = _context.User.ToList().Where(e => e.Username == employeeMaster.EmployeeCode).FirstOrDefault().UserId;

                var newUserRole = new UserRole
                {
                    UserId = userId,
                    Role = EmployeeType.Supervisor.ToString()
                };
                _context.UserRole.Add(newUserRole);
                _context.SaveChanges();
            }

            if (!_context.EmployeeMaster.Any(x => x.EmployeeCode == "E0002"))
            {
                var employeeMaster = new EmployeeMaster
                {
                    EmployeeCode = "E0002",
                    EmployeeName = "Test Employee",
                    EmployeeLeavePacakge = LeavePacakge.Office,
                    EmployeeType = EmployeeType.Employee,
                    EmployeeSupervisorCode = "E0001"
                };
                _context.EmployeeMaster.Add(employeeMaster);
                _context.SaveChanges();

                var annualLeaveForOffice = new LeaveAllocation
                {
                    EmployeeCode = employeeMaster.EmployeeCode,
                    LeaveTypeCode = "A0001",
                    Year = DateTime.Now.Year.ToString(),
                    EntitledLeaveAmount = 14,
                    UtilizedLeaveAmount = 0,
                    RemainingLeaveAmount = 14
                };
                _context.LeaveAllocation.Add(annualLeaveForOffice);
                _context.SaveChanges();

                var casualLeaveForOffice = new LeaveAllocation
                {
                    EmployeeCode = employeeMaster.EmployeeCode,
                    LeaveTypeCode = "C0001",
                    Year = DateTime.Now.Year.ToString(),
                    EntitledLeaveAmount = 7,
                    UtilizedLeaveAmount = 0,
                    RemainingLeaveAmount = 7
                };
                _context.LeaveAllocation.Add(casualLeaveForOffice);
                _context.SaveChanges();

                var sickLeaveForOffice = new LeaveAllocation
                {
                    EmployeeCode = employeeMaster.EmployeeCode,
                    LeaveTypeCode = "S0001",
                    Year = DateTime.Now.Year.ToString(),
                    EntitledLeaveAmount = 21,
                    UtilizedLeaveAmount = 0,
                    RemainingLeaveAmount = 21
                };
                _context.LeaveAllocation.Add(sickLeaveForOffice);
                _context.SaveChanges();

                var newUser = new User
                {
                    Username = employeeMaster.EmployeeCode,
                    Password = employeeMaster.EmployeeCode
                };
                _context.User.Add(newUser);
                _context.SaveChanges();

                var userId = _context.User.ToList().Where(e => e.Username == employeeMaster.EmployeeCode).FirstOrDefault().UserId;

                var newUserRole = new UserRole
                {
                    UserId = userId,
                    Role = EmployeeType.Employee.ToString()
                };
                _context.UserRole.Add(newUserRole);
                _context.SaveChanges();
            }
            #endregion
            return View();
        }

        public ActionResult Login(string username, string password)
        {
            if (username != null && username != "" && password != null && password != "")
            {
                var userDetails = _context.User.ToList().Where(e => e.Username == username && e.Password == password).ToList();

                if (userDetails.Count > 0)
                {
                    if (userDetails.FirstOrDefault().UserId != 0)
                    {
                        var empName = _context.EmployeeMaster.ToList().Where(e => e.EmployeeCode == username).ToList().FirstOrDefault().EmployeeName;
                        var userRole = _context.UserRole.ToList().Where(e => e.UserId == userDetails.FirstOrDefault().UserId).FirstOrDefault().Role;
                        if (userRole == "Employee")
                        {
                            Session["LoggedEmployeeCode"] = username;
                            Session["LoggedEmployee"] = empName;
                            return RedirectToAction("Create", "LeaveRequest");
                        }
                        else if (userRole == "Supervisor")
                        {
                            Session["LoggedSupervisorCode"] = username;
                            Session["LoggedSupervisorName"] = empName;
                            return RedirectToAction("Index", "LeaveStatus");
                        }
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Please check your login credentials !";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Please check your login credentials !";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Please fill all the fields !";
            }
            return Redirect(this.Request.UrlReferrer.AbsolutePath);
        }
    }
}