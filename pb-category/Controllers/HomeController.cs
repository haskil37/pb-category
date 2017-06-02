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
        [ChildActionOnly]
        public ActionResult Menu()
        {
            return PartialView();
        }
        public ActionResult Index()
        {
            Session.Clear();
            Session["Count"] = 0;

            Session["Organization.Name"] = "";
            Session["Organization.Address"] = "";
            Session["Organization.Annotations"] = new List<string>();
            Session["Organization.Length"] = new List<string>();
            Session["Organization.Width"] = new List<string>();
            Session["Organization.Height"] = new List<string>();
            Session["CategoryText"] = new List<string>();
            Session["CategoryEnd"] = new List<string>();
            
            return View();
        }
        public ActionResult SelectCategory()
        {
            Session["SelectMenu"] = 1;
            string Name = "", Address = "";
            if (Session["Organization.Name"] != null)
                Name = Session["Organization.Name"].ToString();
            if (Session["Organization.Address"] != null)
                Address = Session["Organization.Address"].ToString();
            return View(new OrganizationViewModel() { Name = Name, Address = Address });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectCategory(OrganizationViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            Session["Organization.Name"] = model.Name;
            Session["Organization.Address"] = model.Address;
            (Session["Organization.Annotations"] as List<string>).Add(model.Annotation);
            (Session["Organization.Length"] as List<string>).Add(model.Length);
            (Session["Organization.Width"] as List<string>).Add(model.Width);
            (Session["Organization.Height"] as List<string>).Add(model.Height);

            Session["Length"] = model.Length;
            Session["Width"] = model.Width;
            Session["Height"] = model.Height;

            Session["Count"] = (int)Session["Count"] + 1;

            Session["SelectMenu"] = 2;
            switch (model.Category)
            {
                case "A1":
                    (Session["CategoryText"] as List<string>).Add("Горючие газы");
                    return RedirectToAction("Gas", "Calculate");
                case "A2":
                    (Session["CategoryText"] as List<string>).Add("ЛВЖ с температурой вспышки не более 28 °С");

                    Session["Category"] = "A";
                    return RedirectToAction("HFL", "Calculate");
                case "B2":
                    if (model.Category == "B3")
                        (Session["CategoryText"] as List<string>).Add("Горючие жидкости");
                    else
                        (Session["CategoryText"] as List<string>).Add("ЛВЖ с температурой вспышки более 28 °С");

                    Session["Category"] = "B";
                    return RedirectToAction("HFL", "Calculate");
                case "B3":
                    goto case "B2";
                case "C1":
                    if (model.Category == "C2")
                        (Session["CategoryText"] as List<string>).Add("Трудногорючие жидкости");
                    else
                        (Session["CategoryText"] as List<string>).Add("Твердые горючие и трудногорючие вещества и материалы");

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
            var t = Session["Organization.Name"];
            var tt =  Session["Organization.Address"];
            var yy = Session["Organization.Annotations"];
            var yy2 = Session["Organization.Length"];
            var yy3 = Session["Organization.Width"];
            var yy4 = Session["Organization.Height"];
            var u = Session["CategoryText"];
            var i = Session["CategoryEnd"];

            Session["SelectMenu"] = 3;
            ViewBag.Count = ((List<string>)Session["CategoryEnd"]).Count;
            return View();
        }
    }
}