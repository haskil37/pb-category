using pb_category.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace pb_category.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Calculate()
        {
            //Горючие газы
            Session.Clear();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult CalculateSteps(CalculateModels model)
        {
            switch (Session["Step"])
            {
                default:
                    if (!string.IsNullOrEmpty(model.Pmax))
                        Session["Step"] = 1;
                    break;
                case 1:
                    if (!string.IsNullOrEmpty(model.P0))
                        Session["Step"] = 2;
                    else
                        Session["Step"] = 0;
                    break;
            }
            return PartialView();
        }
    }
}