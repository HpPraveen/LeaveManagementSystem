using LeaveManagementSystem.Models;
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
            if (employeeMaster.EmployeeCode != null && employeeMaster.EmployeeLeavePacakge != null && employeeMaster.EmployeeName != null && employeeMaster.EmployeeSupervisorCode != null &&
                employeeMaster.EmployeeCode != "" && employeeMaster.EmployeeLeavePacakge != "" && employeeMaster.EmployeeName != "" && employeeMaster.EmployeeSupervisorCode != "")
            {
                var existingEmployee = db.EmployeeMaster.ToList().Where(e => e.EmployeeCode == employeeMaster.EmployeeCode).ToList();

                if (existingEmployee.Count == 0)
                {
                    if (ModelState.IsValid)
                    {
                        db.EmployeeMaster.Add(employeeMaster);
                        db.SaveChanges();
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