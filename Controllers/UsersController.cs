using LeaveManagementSystem.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace LeaveManagementSystem.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Login(string username, string password)
        {
            if (username != null && username != "" && password != null && password != "")
            {
                var userDetails = db.User.ToList().Where(e => e.Username == username && e.Password == password).ToList();
                var empName = db.EmployeeMaster.ToList().Where(e => e.EmployeeCode == username).ToList().FirstOrDefault().EmployeeName;

                if (userDetails.Count > 0)
                {
                    if (userDetails.FirstOrDefault().UserId != 0)
                    {
                        var userRole = db.UserRole.ToList().Where(e => e.UserId == userDetails.FirstOrDefault().UserId).FirstOrDefault().Role;
                        if (userRole == "Employee")
                        {
                            TempData["LoggedEmployeeCode"] = username;
                            TempData["LoggedEmployee"] = empName;
                            return RedirectToAction("Create", "LeaveRequest");
                        }
                        else if (userRole == "Supervisor")
                        {
                            TempData["LoggedSupervisorCode"] = username;
                            TempData["LoggedSupervisorName"] = empName;
                            return RedirectToAction("Create", "LeaveRequest");
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