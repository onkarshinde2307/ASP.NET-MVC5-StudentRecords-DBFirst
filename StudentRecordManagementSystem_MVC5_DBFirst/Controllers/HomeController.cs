using System.Web.Mvc;

namespace StudentRecordManagementSystem_MVC5_DBFirst.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Our mission is to empower and educate.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Reach out to us for any queries.";
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }
    }
}
