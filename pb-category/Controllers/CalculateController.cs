using pb_category.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace pb_category.Controllers
{
    public class CalculateController : Controller
    {
        public class Input
        {
            public string Name;
            public string Value;
            public string Error;
        }
        public class Step
        {
            public int Number { get; set; }
            public string FinishValue { get; set; }
            public List<Input> AllInputs { get; set; }
        }
        public static CalculateViewModels GlobalModel;

        public JsonResult GetValueA(string id)
        {
            List<Step> values = new List<Step>();

            if (GlobalModel.Pmax != "none")
            {
                Step step = new Step()
                {
                    Number = 1,
                    FinishValue = GlobalModel.Pmax,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "Pmax",
                            Value = GlobalModel.Pmax
                        },

                    }
                };
                values.Add(step);
            }
            if (GlobalModel.P0 != "none")
            {
                Step step = new Step()
                {
                    Number = 2,
                    FinishValue = GlobalModel.P0,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "P0",
                            Value = GlobalModel.P0
                        },

                    }
                };
                values.Add(step);
            }
            if (GlobalModel.Vcb != "none")
            {
                Step step = new Step()
                {
                    Number = 3,
                    FinishValue = GlobalModel.Vcb,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "Vcb",
                            Value = GlobalModel.Vcb
                        },

                    }
                };
                values.Add(step);
            }
            else
            {
                Step step = new Step()
                {
                    Number = 3,
                    AllInputs = new List<Input> {
                    new Input()
                    {
                        Name = "Vcb",
                        Error = "Не указан свободный объем помещения",
                    },
                }
                };
                values.Add(step);
            }
            if (GlobalModel.Kh != "none")
            {
                Step step = new Step()
                {
                    Number = 4,
                    FinishValue = GlobalModel.Kh,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "Kh",
                            Value = GlobalModel.Kh
                        },

                    }
                };
                values.Add(step);
            }
            if (GlobalModel.Z != "none")
            {
                Step step = new Step()
                {
                    Number = 5,
                    FinishValue = GlobalModel.Z,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "Z",
                            Value = GlobalModel.Z
                        },

                    }
                };
                values.Add(step);
            }
            else
            {
                Step step = new Step()
                {
                    Number = 5,
                    AllInputs = new List<Input> {

                    new Input()
                    {
                        Name = "Z",
                        Error = "Выберите коэффициент",
                    },

                }
                };
                values.Add(step);
            }
            if (GlobalModel.Rogp != "none" && GlobalModel.Rogp != "none-calculate")
            {
                Step step = new Step()
                {
                    Number = 6,
                    FinishValue = GlobalModel.Rogp,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "Rogp",
                            Value = GlobalModel.Rogp
                        },
                    }
                };
                values.Add(step);
            }
            else
            {
                Step step = new Step()
                {
                    Number = 6,
                    AllInputs = new List<Input> {
                        new Input()
                        {
                            Name = "Rogp",
                            Error = "Не указана плотность газа или пара",
                        },
                    }
                };

                if (GlobalModel.Rogp == "none-calculate")
                {
                    if (GlobalModel.M == "none")
                        step.AllInputs.Add( 
                            new Input()
                            {
                                Name = "M",
                                Error = "Не указана молярная масса",
                            }
                        );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "M",
                                Value = GlobalModel.M
                            }
                        );
                    if (GlobalModel.V0 == "none")
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "V0",
                                Error = "Не указана расчетная температура",
                            }
                        );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "V0",
                                Value = GlobalModel.V0
                            }
                        );
                    if (GlobalModel.Tp == "none")
                            step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Tp",
                                Error = "Не указан мольный объем",
                            }
                        );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Tp",
                                Value = GlobalModel.Tp
                            }
                        );
                }
                values.Add(step);
            }
            return Json(values, JsonRequestBehavior.AllowGet);
        }
        public ActionResult A()
        {
            GlobalModel = new CalculateViewModels()
            {
                GlobalStep = 1
            };
            ViewBag.Step = GlobalModel.GlobalStep;
            return View(new CalculateViewModels());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Steps(CalculateViewModels model, string submitButton)
        {
            int CurrentStep = 1;
            double Pmax, P0, Vcb, Kh, Z, Rogp, Cct;
            if (GlobalModel.GlobalStep >= 1)
            {
                Pmax = GetValue(model.Pmax, ref CurrentStep);
                if (Pmax == 0)
                {
                    Pmax = 900;
                    CurrentStep++;
                }
                GlobalModel.Pmax = Pmax.ToString();
            }
            if (GlobalModel.GlobalStep >= 2)
            {
                P0 = GetValue(model.P0, ref CurrentStep);
                if (P0 == 0)
                {
                    P0 = 101;
                    CurrentStep++;
                }
                GlobalModel.P0 = P0.ToString();
            }
            if (GlobalModel.GlobalStep >= 3)
            {
                Vcb = GetValue(model.Vcb, ref CurrentStep);
                if (Vcb != 0)
                    GlobalModel.Vcb = Vcb.ToString();
                else
                    GlobalModel.Vcb = "none";
            }
            if (GlobalModel.GlobalStep >= 4)
            {
                Kh = GetValue(model.Kh, ref CurrentStep);
                if (Kh == 0)
                {
                    Kh = 3;
                    CurrentStep++;
                }
                GlobalModel.Kh = Kh.ToString();
            }
            if (GlobalModel.GlobalStep >= 5)
            {
                Z = GetValue(model.Z, ref CurrentStep);
                if (Z != 0)
                    GlobalModel.Z = Z.ToString();
                else
                    GlobalModel.Z = "none";
            }
            if (GlobalModel.GlobalStep >= 6)
            {
                if (submitButton == "RogpButton")
                {
                    double M, V0, Tp;
                    int FullData = 0;
                    M = GetValue(model.M, ref FullData);
                    if (M != 0)
                        GlobalModel.M = M.ToString();
                    else
                        GlobalModel.M = "none";
                    V0 = GetValue(model.V0, ref FullData);
                    if (V0 > 0)
                        GlobalModel.V0 = V0.ToString();
                    else if (V0 == 0)
                    {
                        FullData--;
                        GlobalModel.V0 = "none";
                    }
                    else
                        GlobalModel.V0 = "none";
                    Tp = GetValue(model.Tp, ref FullData);
                    if (Tp != 0)
                        GlobalModel.Tp = Tp.ToString();
                    else
                        GlobalModel.Tp = "none";
                    if (FullData == 3)
                    {
                        Rogp = M / (V0 * (1 + 0.00367 * Tp));
                        GlobalModel.Rogp = Rogp.ToString();
                        CurrentStep++;
                    }
                    else
                        GlobalModel.Rogp = "none-calculate";
                }
                else
                {
                    GlobalModel.M = model.M;
                    GlobalModel.V0 = model.V0;
                    GlobalModel.Tp = model.Tp;
                    Rogp = GetValue(model.Rogp, ref CurrentStep);
                    if (Rogp != 0)
                        GlobalModel.Rogp = Rogp.ToString();
                    else
                        GlobalModel.Rogp = "none";
                }
            }
            if (GlobalModel.GlobalStep >= 7)
            {
                if (submitButton == "CctButton")
                {
                    double Beta, Nc, Nh, Nx, No;
                    int FullData = 0;
                    Nc = GetValue(model.Nc, ref FullData);
                    GlobalModel.Nc = Nc.ToString();
                    Nh = GetValue(model.Nh, ref FullData);
                    GlobalModel.Nh = Nh.ToString();
                    Nx = GetValue(model.Nx, ref FullData);
                    GlobalModel.Nx = Nx.ToString();
                    No = GetValue(model.No, ref FullData);
                    GlobalModel.No = No.ToString();
                    if (FullData == 4)
                    {
                        Beta = Nc + (Nh - Nx) / 4 - No / 2;
                        GlobalModel.Beta = Beta.ToString();
                        Cct = 100 / (1 + 4.84 * Beta);
                        GlobalModel.Cct = Cct.ToString();
                        CurrentStep++;
                    }
                }
                else
                {
                    GlobalModel.Nc = model.Nc;
                    GlobalModel.Nh = model.Nh;
                    GlobalModel.Nx = model.Nx;
                    GlobalModel.No = model.No;
                    Cct = GetValue(model.Cct, ref CurrentStep);
                    if (Cct != 0)
                        GlobalModel.Cct = Cct.ToString();
                }
            }
            if (GlobalModel.GlobalStep >= 8)
            {
                if (submitButton == "CctButton")
                {
                    double Beta, Nc, Nh, Nx, No;
                    int FullData = 0;
                    Nc = GetValue(model.Nc, ref FullData);
                    GlobalModel.Nc = Nc.ToString();
                    Nh = GetValue(model.Nh, ref FullData);
                    GlobalModel.Nh = Nh.ToString();
                    Nx = GetValue(model.Nx, ref FullData);
                    GlobalModel.Nx = Nx.ToString();
                    No = GetValue(model.No, ref FullData);
                    GlobalModel.No = No.ToString();
                    if (FullData == 4)
                    {
                        Beta = Nc + (Nh - Nx) / 4 - No / 2;
                        GlobalModel.Beta = Beta.ToString();
                        Cct = 100 / (1 + 4.84 * Beta);
                        GlobalModel.Cct = Cct.ToString();
                        CurrentStep++;
                    }
                }
                else
                {
                    GlobalModel.Nc = model.Nc;
                    GlobalModel.Nh = model.Nh;
                    GlobalModel.Nx = model.Nx;
                    GlobalModel.No = model.No;
                    Cct = GetValue(model.Cct, ref CurrentStep);
                    if (Cct != 0)
                        GlobalModel.Cct = Cct.ToString();
                }
            }

            ViewBag.Z = new List<SelectListItem>
            {
                new SelectListItem { Text = "Водород - 1", Value = "1" },
                new SelectListItem { Text = "Горючие газы (кроме водорода) - 0,5", Value = "0,5" }
            };

            if (CurrentStep > GlobalModel.GlobalStep)
            {
                GlobalModel.GlobalStep = CurrentStep;
                ViewBag.Step = GlobalModel.GlobalStep;
                return PartialView("Steps", GlobalModel);
            }
            else
            {
                return HttpNotFound();
            }
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