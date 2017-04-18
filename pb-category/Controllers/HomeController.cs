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
        public static CalculateViewModels GlobalModel;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Calculate()
        {
            GlobalModel = new CalculateViewModels();
            GlobalStep = 1;
            ViewBag.Step = GlobalStep;
            return View(new CalculateViewModels());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult CalculateSteps(CalculateViewModels model)
        {
            int CurrentStep = 1;
            float Pmax, P0;
            if (GlobalStep >= 1)
            {
                Pmax = GetValue(model.Pmax, ref CurrentStep);
                if (Pmax == 0)
                {
                    Pmax = 900;
                    CurrentStep++;
                }
                GlobalModel.Pmax = Pmax.ToString();
            }
            if (GlobalStep >= 2)
            {
                P0 = GetValue(model.P0, ref CurrentStep);
                if (P0 == 0)
                {
                    P0 = 101;
                    CurrentStep++;
                }
                GlobalModel.P0 = P0.ToString();
            }

            if (CurrentStep > GlobalStep)
                GlobalStep = CurrentStep;

            ViewBag.Step = GlobalStep;

            return PartialView(GlobalModel);
        }
        #region Вспомогательные функции
        private float GetValue(string value, ref int CurrentStep)
        {
            float parse = 0;
            if (!string.IsNullOrEmpty(value) && float.TryParse(value, out parse))
                CurrentStep++;
            return parse;
        }
        #endregion
    }
}