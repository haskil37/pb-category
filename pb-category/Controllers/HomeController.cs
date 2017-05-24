using System.Web.Mvc;

namespace pb_category.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Session.Clear();
            return View();
        }
        public ActionResult SelectCategory()
        {
            Session["SelectMenu"] = 1;
            return View();
        }
        [ChildActionOnly]
        public ActionResult Menu()
        {
            return PartialView();
        }
    }
}