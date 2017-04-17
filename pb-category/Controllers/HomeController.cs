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
        public static int GlobalStep;
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Calculate()
        {
            //Горючие газы
            GlobalStep = 1;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult CalculateSteps(CalculateModels model)
        {
            int CurrentStep = 1;
            string PartialStep = "Step";
            float Pmax, P0;
            if (float.TryParse(model.Pmax, out Pmax))
                CurrentStep++;
            if (float.TryParse(model.P0, out P0))
                CurrentStep++;

            if (Pmax == 0)
                Pmax = 900;
            if (P0 == 0)
                P0 = 101;


            if (CurrentStep > GlobalStep)
            {
                GlobalStep = CurrentStep;
                PartialStep += CurrentStep;
            }
            return PartialView(PartialStep);
        }
    }
}