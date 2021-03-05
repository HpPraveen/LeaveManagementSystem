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
            return RedirectToAction("Create", "LeaveRequest");
        }
    }
}