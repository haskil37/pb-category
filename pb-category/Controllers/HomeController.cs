using pb_category.Models;
using System.Web.Mvc;

namespace pb_category.Controllers
{
    public class HomeController : Controller
    {
        [ChildActionOnly]
        public ActionResult Menu()
        {
            return PartialView();
        }
        public ActionResult Index()
        {
            Session.Clear();
            return View();
        }
        public ActionResult SelectCategory()
        {
            Session["SelectMenu"] = 1;
            return View(new OrganizationViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectCategory(OrganizationViewModel model)
        {
            return View();
        }
    }
}