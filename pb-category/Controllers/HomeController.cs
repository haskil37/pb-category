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
            if (!ModelState.IsValid)
                return View(model);
            Session["Length"] = model.Length;
            Session["Width"] = model.Width;
            Session["Height"] = model.Height;
            Session["SelectMenu"] = 2;
            switch (model.Category)
            {
                case "A1":
                    return RedirectToAction("Gas", "Calculate");
                case "A2":
                    Session["Category"] = "A";
                    return RedirectToAction("HFL", "Calculate");
                case "B2":
                    Session["Category"] = "B";
                    return RedirectToAction("HFL", "Calculate");
                case "B3":
                    goto case "B2";
                default:
                    return View(model);
            }
        }
    }
}