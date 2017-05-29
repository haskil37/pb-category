using pb_category.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace pb_category.Controllers
{
    public class OrganizationModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public List<string> Annotations { get; set; }
        public List<string> Length { get; set; }
        public List<string> Width { get; set; }
        public List<string> Height { get; set; }
    }
    public class HomeController : Controller
    {
        public static OrganizationModel Organization;
        [ChildActionOnly]
        public ActionResult Menu()
        {
            return PartialView();
        }
        public ActionResult Index()
        {
            Organization = new OrganizationModel()
            {
                Annotations = new List<string>(),
                Length = new List<string>(),
                Width = new List<string>(),
                Height = new List<string>()
            };
            Session.Clear();
            return View();
        }
        public ActionResult SelectCategory()
        {
            Session["SelectMenu"] = 1;

            OrganizationViewModel model = new OrganizationViewModel()
            {
                Address = Organization.Address,
                Name = Organization.Name
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectCategory(OrganizationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            Organization.Name = model.Name;
            Organization.Address = model.Address;
            Organization.Annotations.Add(model.Annotation);
            Organization.Length.Add(model.Length);
            Organization.Width.Add(model.Width);
            Organization.Height.Add(model.Height);

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
                case "C1":
                    Session["Category"] = "C";
                    return RedirectToAction("DCS", "Calculate");
                case "C2":
                    goto case "C1";
                default:
                    return View(model);
            }
        }
        public ActionResult ViewOrganization()
        {
            return View();
        }
    }
}