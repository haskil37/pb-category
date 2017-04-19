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

            //Горючие газы
            GlobalModel.ListZ = new List<ListZ>
                {
                    new ListZ{ Value="1", Name="Водород - 1" },
                    new ListZ{ Value="0,5", Name="Горючие газы (кроме водорода) - 0,5" }
                };
            return View(new CalculateViewModels());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult CalculateSteps(CalculateViewModels model, string submitButton)
        {
            int CurrentStep = 1;
            double Pmax, P0, Vcb, Kh, Z, Rogp;
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
            if (GlobalStep >= 3)
            {
                Vcb = GetValue(model.Vcb, ref CurrentStep);
                if (Vcb != 0)
                    GlobalModel.Vcb = Vcb.ToString();
            }
            if (GlobalStep >= 4)
            {
                Kh = GetValue(model.Kh, ref CurrentStep);
                if (Kh == 0)
                {
                    Kh = 3;
                    CurrentStep++;
                }
                GlobalModel.Kh = Kh.ToString();
            }
            if (GlobalStep >= 5)
            {
                Z = GetValue(model.Z, ref CurrentStep);
                if (Z != 0)
                    GlobalModel.Z = Z.ToString();
            }
            if(GlobalStep >= 6)
            {
                if(submitButton == "RogpButton")
                {
                    double M, V0, Tp;
                    int FullData = 0;
                    M = GetValue(model.M, ref FullData);
                    GlobalModel.M = M.ToString();
                    V0 = GetValue(model.V0, ref FullData);
                    GlobalModel.V0 = V0.ToString();
                    Tp = GetValue(model.Tp, ref FullData);
                    GlobalModel.Tp = Tp.ToString();
                    if (FullData == 3)
                    {
                        Rogp = M / (V0 * (1 + 0.00367 * Tp));
                        GlobalModel.Rogp = Rogp.ToString();
                        CurrentStep++;
                    }
                }
                else
                {
                    Rogp = GetValue(model.Rogp, ref CurrentStep);
                    if (Rogp != 0)
                        GlobalModel.Rogp = Rogp.ToString();
                }
            }






            if (CurrentStep > GlobalStep)
                GlobalStep = CurrentStep;
            ViewBag.Step = GlobalStep;
            return PartialView(GlobalModel);
        }
        #region Вспомогательные функции
        private double GetValue(string value, ref int CurrentStep)
        {
            double parse = 0;
            if (!string.IsNullOrEmpty(value) && double.TryParse(value, out parse))
                CurrentStep++;
            return parse;
        }
        #endregion
    }
}