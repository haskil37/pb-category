using pb_category.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace pb_category.Controllers
{
    public class CalculateController : Controller
    {
        public static CalculateGasViewModel GlobalModelGas;
        public static CalculateHFLViewModel GlobalModelHFL;

        public ActionResult Gas()
        {
            GlobalModelGas = new CalculateGasViewModel()
            {
                GlobalStep = 1,
                DeltaP = "none",
                R = new List<string>(),
                L = new List<string>(),
            };
            GlobalModelGas.R.Add(null);
            GlobalModelGas.L.Add(null);
            ViewBag.Step = 1;

            Session["SelectMenu"] = 2;//Удалить потом

            return View("Gas/Index", new CalculateGasViewModel());
        }
        public ActionResult HFL()
        {
            GlobalModelHFL = new CalculateHFLViewModel()
            {
                GlobalStep = 1,
                DeltaP = "none",
            };
            ViewBag.Step = 1;

            Session["SelectMenu"] = 2;//Удалить потом

            return View("HFL/Index", new CalculateHFLViewModel());
        }

        public PartialViewResult RLAdd()
        {
            GlobalModelGas.R.Add(null);
            GlobalModelGas.L.Add(null);
            return PartialView("RLAdd", GlobalModelGas);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StepsGas(CalculateGasViewModel model, string submitButton)
        {
            int CurrentStep = 1;
            double Pmax = 0, P0 = 0, Vcb = 0, Kh = 0, Z = 0, Rogp = 0, Cct = 0, Mkg = 0;
            if (GlobalModelGas.GlobalStep >= 1)
            {
                Pmax = GetValue(model.Pmax, ref CurrentStep);
                if (Pmax == 0)
                {
                    Pmax = 900;
                    CurrentStep++;
                }
                GlobalModelGas.Pmax = Pmax.ToString();
            }
            if (GlobalModelGas.GlobalStep >= 2)
            {
                P0 = GetValue(model.P0, ref CurrentStep);
                if (P0 == 0)
                {
                    P0 = 101;
                    CurrentStep++;
                }
                GlobalModelGas.P0 = P0.ToString();
            }
            if (GlobalModelGas.GlobalStep >= 3)
            {
                Vcb = GetValue(model.Vcb, ref CurrentStep);
                if (Vcb != 0)
                    GlobalModelGas.Vcb = Vcb.ToString();
                else
                {
                    if (Session["Length"] != null && Session["Width"] != null && Session["Height"] != null)
                    {
                        GlobalModelGas.Vcb = (Convert.ToDouble(Session["Length"]) *
                                          Convert.ToDouble(Session["Width"]) *
                                          Convert.ToDouble(Session["Height"]) * 0.8).ToString();
                        CurrentStep++;
                    }
                    else
                        GlobalModelGas.Vcb = "none";
                }
            }
            if (GlobalModelGas.GlobalStep >= 4)
            {
                Kh = GetValue(model.Kh, ref CurrentStep);
                if (Kh == 0)
                {
                    Kh = 3;
                    CurrentStep++;
                }
                GlobalModelGas.Kh = Kh.ToString();
            }
            if (GlobalModelGas.GlobalStep >= 5)
            {
                Z = GetValue(model.Z, ref CurrentStep);
                if (Z != 0)
                    GlobalModelGas.Z = Z.ToString();
                else
                    GlobalModelGas.Z = "none";
            }
            if (GlobalModelGas.GlobalStep >= 6)
            {
                if (submitButton == "RogpButton")
                {
                    double M, V0, Tp;
                    int FullData = 0;
                    M = GetValue(model.M, ref FullData);
                    if (M != 0)
                        GlobalModelGas.M = M.ToString();
                    else
                        GlobalModelGas.M = "none";
                    V0 = GetValue(model.V0, ref FullData);
                    if (V0 > 0)
                        GlobalModelGas.V0 = V0.ToString();
                    else if (V0 == 0)
                    {
                        FullData--;
                        GlobalModelGas.V0 = "none";
                    }
                    else
                        GlobalModelGas.V0 = "none";
                    Tp = GetValue(model.Tp, ref FullData);
                    if (Tp != 0)
                        GlobalModelGas.Tp = Tp.ToString();
                    else
                        GlobalModelGas.Tp = "none";
                    if (FullData == 3)
                    {
                        Rogp = M / (V0 * (1 + 0.00367 * Tp));
                        GlobalModelGas.Rogp = Rogp.ToString();
                        CurrentStep++;
                    }
                    else
                        GlobalModelGas.Rogp = "none-calculate";
                }
                else
                {
                    GlobalModelGas.M = model.M;
                    GlobalModelGas.V0 = model.V0;
                    GlobalModelGas.Tp = model.Tp;
                    Rogp = GetValue(model.Rogp, ref CurrentStep);
                    if (Rogp != 0)
                        GlobalModelGas.Rogp = Rogp.ToString();
                    else
                        GlobalModelGas.Rogp = "none";
                }
            }
            if (GlobalModelGas.GlobalStep >= 7)
            {
                if (submitButton == "CctButton")
                {
                    double Beta, Nc, Nh, Nx, No;
                    int FullData = 0;
                    Nc = GetValue(model.Nc, ref FullData);
                    if (Nc != 0)
                        GlobalModelGas.Nc = Nc.ToString();
                    else
                        GlobalModelGas.Nc = "none";
                    Nh = GetValue(model.Nh, ref FullData);
                    if (Nh != 0)
                        GlobalModelGas.Nh = Nh.ToString();
                    else
                        GlobalModelGas.Nh = "none";
                    Nx = GetValue(model.Nx, ref FullData);
                    if (Nx != 0)
                        GlobalModelGas.Nx = Nx.ToString();
                    else
                        GlobalModelGas.Nx = "none";
                    No = GetValue(model.No, ref FullData);
                    if (No != 0)
                        GlobalModelGas.No = No.ToString();
                    else
                        GlobalModelGas.No = "none";
                    if (FullData == 4)
                    {
                        Beta = Nc + (Nh - Nx) / 4 - No / 2;
                        GlobalModelGas.Beta = Beta.ToString();
                        Cct = 100 / (1 + 4.84 * Beta);
                        GlobalModelGas.Cct = Cct.ToString();
                        CurrentStep++;
                    }
                    else
                        GlobalModelGas.Cct = "none-calculate";
                }
                else
                {
                    GlobalModelGas.Nc = model.Nc;
                    GlobalModelGas.Nh = model.Nh;
                    GlobalModelGas.Nx = model.Nx;
                    GlobalModelGas.No = model.No;
                    Cct = GetValue(model.Cct, ref CurrentStep);
                    if (Cct != 0)
                        GlobalModelGas.Cct = Cct.ToString();
                    else
                        GlobalModelGas.Cct = "none";
                }
            }
            if (GlobalModelGas.GlobalStep >= 8)
            {
                if (submitButton == "mButton")
                {
                    double Rog, P1, P2, V, R, Q, T, L;
                    int FullData = 0;
                    Rog = GetValue(model.Rog, ref FullData);
                    if (Rog != 0)
                        GlobalModelGas.Rog = Rog.ToString();
                    else
                        GlobalModelGas.Rog = "none";
                    P1 = GetValue(model.P1, ref FullData);
                    if (P1 != 0)
                        GlobalModelGas.P1 = P1.ToString();
                    else
                        GlobalModelGas.P1 = "none";
                    P2 = GetValue(model.P2, ref FullData);
                    if (P2 != 0)
                        GlobalModelGas.P2 = P2.ToString();
                    else
                        GlobalModelGas.P2 = "none";
                    V = GetValue(model.V, ref FullData);
                    if (V != 0)
                        GlobalModelGas.V = V.ToString();
                    else
                        GlobalModelGas.V = "none";
                    Q = GetValue(model.Q, ref FullData);
                    if (Q != 0)
                        GlobalModelGas.Q = Q.ToString();
                    else
                        GlobalModelGas.Q = "none";
                    T = GetValue(model.T, ref FullData);
                    if (T != 0)
                        GlobalModelGas.T = T.ToString();
                    else
                        GlobalModelGas.T = "none";


                    int countR = 0;
                    int temp = 0;
                    foreach (var item in model.R)
                    {
                        R = GetValue(item, ref countR);
                        if (R != 0)
                            GlobalModelGas.R[temp] = R.ToString();
                        else
                            GlobalModelGas.R[temp] = "none";
                        temp++;
                    }
                    int countL = 0;
                    temp = 0;
                    foreach (var item in model.L)
                    {
                        L = GetValue(item, ref countL);
                        if (L != 0)
                            GlobalModelGas.L[temp] = L.ToString();
                        else
                            GlobalModelGas.L[temp] = "none";
                        temp++;
                    }




                    if (FullData == 6 && countR == countL)
                    {
                        double Va, Vt, V2 = 0;
                        Va = 0.001 * P1 * V;
                        for (int i = 0; i < countR; i++)
                            V2 += Convert.ToDouble(GlobalModelGas.R[i]) * Convert.ToDouble(GlobalModelGas.R[i]) * Convert.ToDouble(GlobalModelGas.L[i]);
                        Vt = Q * T + Math.PI * P2 * V2;
                        Mkg = Rog * (Va + Vt);
                        GlobalModelGas.Mkg = Mkg.ToString();
                        CurrentStep++;
                    }
                    else
                        GlobalModelGas.Mkg = "none-calculate";
                }
                else
                {
                    GlobalModelGas.Rog = model.Rog;
                    GlobalModelGas.P1 = model.P1;
                    GlobalModelGas.P2 = model.P2;
                    GlobalModelGas.V = model.V;
                    GlobalModelGas.R = model.R;
                    GlobalModelGas.Q = model.Q;
                    GlobalModelGas.T = model.T;
                    GlobalModelGas.L = model.L;
                    Mkg = GetValue(model.Mkg, ref CurrentStep);
                    if (Mkg != 0)
                        GlobalModelGas.Mkg = Mkg.ToString();
                    else
                        GlobalModelGas.Mkg = "none";
                }
            }
            if (CurrentStep == 9) //Если все нужные 8 шагов есть, то считаем категорию
            {
                double DeplaP = (Pmax - P0) * (Mkg * Z / (Vcb * Rogp)) * (100 / Cct) * (1 / Kh);
                GlobalModelGas.DeltaP = DeplaP.ToString();
            }

            ViewBag.Z = new List<SelectListItem>
            {
                new SelectListItem { Text = "Водород - 1", Value = "1" },
                new SelectListItem { Text = "Горючие газы (кроме водорода) - 0,5", Value = "0,5" }
            };

            if (CurrentStep <= GlobalModelGas.GlobalStep)
                return HttpNotFound();
            GlobalModelGas.GlobalStep = CurrentStep;
            ViewBag.Step = GlobalModelGas.GlobalStep;

            return PartialView("Gas/Steps", GlobalModelGas);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StepsHFL(CalculateGasViewModel model, string submitButton)
        {
            int CurrentStep = 1;
            double Pmax = 0, P0 = 0, Vcb = 0, Kh = 0, Z = 0, Rogp = 0, Cct = 0, Mkg = 0;
            if (GlobalModelHFL.GlobalStep >= 1)
            {
                Pmax = GetValue(model.Pmax, ref CurrentStep);
                if (Pmax == 0)
                {
                    Pmax = 900;
                    CurrentStep++;
                }
                GlobalModelHFL.Pmax = Pmax.ToString();
            }
            if (GlobalModelHFL.GlobalStep >= 2)
            {
                P0 = GetValue(model.P0, ref CurrentStep);
                if (P0 == 0)
                {
                    P0 = 101;
                    CurrentStep++;
                }
                GlobalModelHFL.P0 = P0.ToString();
            }
            if (GlobalModelHFL.GlobalStep >= 3)
            {
                Vcb = GetValue(model.Vcb, ref CurrentStep);
                if (Vcb != 0)
                    GlobalModelHFL.Vcb = Vcb.ToString();
                else
                {
                    if (Session["Length"] != null && Session["Width"] != null && Session["Height"] != null)
                    {
                        GlobalModelHFL.Vcb = (Convert.ToDouble(Session["Length"]) *
                                          Convert.ToDouble(Session["Width"]) *
                                          Convert.ToDouble(Session["Height"]) * 0.8).ToString();
                        CurrentStep++;
                    }
                    else
                        GlobalModelHFL.Vcb = "none";
                }
            }
            if (GlobalModelHFL.GlobalStep >= 4)
            {
                Kh = GetValue(model.Kh, ref CurrentStep);
                if (Kh == 0)
                {
                    Kh = 3;
                    CurrentStep++;
                }
                GlobalModelHFL.Kh = Kh.ToString();
            }
            if (GlobalModelHFL.GlobalStep >= 5)
            {
                Z = GetValue(model.Z, ref CurrentStep);
                if (Z != 0)
                    GlobalModelHFL.Z = Z.ToString();
                else
                    GlobalModelHFL.Z = "none";
            }
            if (GlobalModelHFL.GlobalStep >= 6)
            {
                if (submitButton == "RogpButton")
                {
                    double M, V0, Tp;
                    int FullData = 0;
                    M = GetValue(model.M, ref FullData);
                    if (M != 0)
                        GlobalModelHFL.M = M.ToString();
                    else
                        GlobalModelHFL.M = "none";
                    V0 = GetValue(model.V0, ref FullData);
                    if (V0 > 0)
                        GlobalModelHFL.V0 = V0.ToString();
                    else if (V0 == 0)
                    {
                        FullData--;
                        GlobalModelHFL.V0 = "none";
                    }
                    else
                        GlobalModelHFL.V0 = "none";
                    Tp = GetValue(model.Tp, ref FullData);
                    if (Tp != 0)
                        GlobalModelHFL.Tp = Tp.ToString();
                    else
                        GlobalModelHFL.Tp = "none";
                    if (FullData == 3)
                    {
                        Rogp = M / (V0 * (1 + 0.00367 * Tp));
                        GlobalModelHFL.Rogp = Rogp.ToString();
                        CurrentStep++;
                    }
                    else
                        GlobalModelHFL.Rogp = "none-calculate";
                }
                else
                {
                    GlobalModelHFL.M = model.M;
                    GlobalModelHFL.V0 = model.V0;
                    GlobalModelHFL.Tp = model.Tp;
                    Rogp = GetValue(model.Rogp, ref CurrentStep);
                    if (Rogp != 0)
                        GlobalModelHFL.Rogp = Rogp.ToString();
                    else
                        GlobalModelHFL.Rogp = "none";
                }
            }
            if (GlobalModelHFL.GlobalStep >= 7)
            {
                if (submitButton == "CctButton")
                {
                    double Beta, Nc, Nh, Nx, No;
                    int FullData = 0;
                    Nc = GetValue(model.Nc, ref FullData);
                    if (Nc != 0)
                        GlobalModelHFL.Nc = Nc.ToString();
                    else
                        GlobalModelHFL.Nc = "none";
                    Nh = GetValue(model.Nh, ref FullData);
                    if (Nh != 0)
                        GlobalModelHFL.Nh = Nh.ToString();
                    else
                        GlobalModelHFL.Nh = "none";
                    Nx = GetValue(model.Nx, ref FullData);
                    if (Nx != 0)
                        GlobalModelHFL.Nx = Nx.ToString();
                    else
                        GlobalModelHFL.Nx = "none";
                    No = GetValue(model.No, ref FullData);
                    if (No != 0)
                        GlobalModelHFL.No = No.ToString();
                    else
                        GlobalModelHFL.No = "none";
                    if (FullData == 4)
                    {
                        Beta = Nc + (Nh - Nx) / 4 - No / 2;
                        GlobalModelHFL.Beta = Beta.ToString();
                        Cct = 100 / (1 + 4.84 * Beta);
                        GlobalModelHFL.Cct = Cct.ToString();
                        CurrentStep++;
                    }
                    else
                        GlobalModelHFL.Cct = "none-calculate";
                }
                else
                {
                    GlobalModelHFL.Nc = model.Nc;
                    GlobalModelHFL.Nh = model.Nh;
                    GlobalModelHFL.Nx = model.Nx;
                    GlobalModelHFL.No = model.No;
                    Cct = GetValue(model.Cct, ref CurrentStep);
                    if (Cct != 0)
                        GlobalModelHFL.Cct = Cct.ToString();
                    else
                        GlobalModelHFL.Cct = "none";
                }
            }
            if (GlobalModelHFL.GlobalStep >= 8)
            {
                //if (submitButton == "mButton")
                //{
                //    double Rog, P1, P2, V, R, Q, T, L;
                //    int FullData = 0;
                //    Rog = GetValue(model.Rog, ref FullData);
                //    if (Rog != 0)
                //        GlobalModelB.Rog = Rog.ToString();
                //    else
                //        GlobalModelB.Rog = "none";
                //    P1 = GetValue(model.P1, ref FullData);
                //    if (P1 != 0)
                //        GlobalModelB.P1 = P1.ToString();
                //    else
                //        GlobalModelB.P1 = "none";
                //    P2 = GetValue(model.P2, ref FullData);
                //    if (P2 != 0)
                //        GlobalModelB.P2 = P2.ToString();
                //    else
                //        GlobalModelB.P2 = "none";
                //    V = GetValue(model.V, ref FullData);
                //    if (V != 0)
                //        GlobalModelB.V = V.ToString();
                //    else
                //        GlobalModelB.V = "none";
                //    Q = GetValue(model.Q, ref FullData);
                //    if (Q != 0)
                //        GlobalModelB.Q = Q.ToString();
                //    else
                //        GlobalModelB.Q = "none";
                //    T = GetValue(model.T, ref FullData);
                //    if (T != 0)
                //        GlobalModelB.T = T.ToString();
                //    else
                //        GlobalModelB.T = "none";






                //    if (FullData == 6)
                //    {
                //        double Va, Vt, V2 = 0;
                //        Va = 0.001 * P1 * V;
                //        Vt = Q * T + Math.PI * P2 * V2;
                //        Mkg = Rog * (Va + Vt);
                //        GlobalModelB.Mkg = Mkg.ToString();
                //        CurrentStep++;
                //    }
                //    else
                //        GlobalModelB.Mkg = "none-calculate";
                //}
                //else
                //{
                //    GlobalModelB.Rog = model.Rog;
                //    GlobalModelB.P1 = model.P1;
                //    GlobalModelB.P2 = model.P2;
                //    GlobalModelB.V = model.V;
                //    GlobalModelB.R = model.R;
                //    GlobalModelB.Q = model.Q;
                //    GlobalModelB.T = model.T;
                //    GlobalModelB.L = model.L;
                //    Mkg = GetValue(model.Mkg, ref CurrentStep);
                //    if (Mkg != 0)
                //        GlobalModelB.Mkg = Mkg.ToString();
                //    else
                //        GlobalModelB.Mkg = "none";
                //}
            }
            if (CurrentStep == 9) //Если все нужные 8 шагов есть, то считаем категорию
            {
                double DeplaP = (Pmax - P0) * (Mkg * Z / (Vcb * Rogp)) * (100 / Cct) * (1 / Kh);
                GlobalModelHFL.DeltaP = DeplaP.ToString();
            }

            ViewBag.Z = new List<SelectListItem>
            {
                new SelectListItem { Text = "Легковоспламеняющиеся и горючие жидкости, нагретые до температуры вспышки и выше - 0,3", Value = "0,3" },
                new SelectListItem { Text = "Легковоспламеняющиеся и горючие жидкости, нагретые ниже температуры вспышки, при наличии возможности образования аэрозоля - 0,3", Value = "0,3" },
                new SelectListItem { Text = "Легковоспламеняющиеся и горючие жидкости, нагретые ниже температуры вспышки, при отсутствии возможности образования аэрозоля - 0", Value = "0" }
            };

            if (CurrentStep <= GlobalModelHFL.GlobalStep)
                return HttpNotFound();
            GlobalModelHFL.GlobalStep = CurrentStep;
            ViewBag.Step = GlobalModelHFL.GlobalStep;

            return PartialView("HFL/Steps", GlobalModelHFL);
        }

        #region Вспомогательные функции
        private double GetValue(string value, ref int CurrentStep)
        {
            if (!string.IsNullOrEmpty(value))
                value = value.Replace(".", ",").Trim();
            double parse = 0;
            if (!string.IsNullOrEmpty(value) && double.TryParse(value, out parse))
                CurrentStep++;
            return parse;
        }
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

        public JsonResult GetValueGas(string id)
        {
            List<Step> values = new List<Step>();
            #region Pmax
            if (GlobalModelGas.Pmax != "none")
            {
                Step step = new Step()
                {
                    Number = 1,
                    FinishValue = GlobalModelGas.Pmax,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "Pmax",
                            Value = GlobalModelGas.Pmax
                        },

                    }
                };
                values.Add(step);
            }
            #endregion
            #region P0
            if (GlobalModelGas.P0 != "none")
            {
                Step step = new Step()
                {
                    Number = 2,
                    FinishValue = GlobalModelGas.P0,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "P0",
                            Value = GlobalModelGas.P0
                        },

                    }
                };
                values.Add(step);
            }
            #endregion
            #region Vcb
            if (GlobalModelGas.Vcb != "none")
            {
                Step step = new Step()
                {
                    Number = 3,
                    FinishValue = GlobalModelGas.Vcb,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "Vcb",
                            Value = GlobalModelGas.Vcb
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
            #endregion
            #region Kh
            if (GlobalModelGas.Kh != "none")
            {
                Step step = new Step()
                {
                    Number = 4,
                    FinishValue = GlobalModelGas.Kh,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "Kh",
                            Value = GlobalModelGas.Kh
                        },

                    }
                };
                values.Add(step);
            }
            #endregion
            #region Z
            if (GlobalModelGas.Z != "none")
            {
                Step step = new Step()
                {
                    Number = 5,
                    FinishValue = GlobalModelGas.Z,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "Z",
                            Value = GlobalModelGas.Z
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
            #endregion
            #region Rogp
            if (GlobalModelGas.Rogp != "none" && GlobalModelGas.Rogp != "none-calculate")
            {
                Step step = new Step()
                {
                    Number = 6,
                    FinishValue = GlobalModelGas.Rogp,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "Rogp",
                            Value = GlobalModelGas.Rogp
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

                if (GlobalModelGas.Rogp == "none-calculate")
                {
                    if (GlobalModelGas.M == "none")
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
                                Value = GlobalModelGas.M
                            }
                        );
                    if (GlobalModelGas.V0 == "none")
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
                                Value = GlobalModelGas.V0
                            }
                        );
                    if (GlobalModelGas.Tp == "none")
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
                                Value = GlobalModelGas.Tp
                            }
                        );
                }
                values.Add(step);
            }
            #endregion
            #region Cct
            if (GlobalModelGas.Cct != "none" && GlobalModelGas.Cct != "none-calculate")
            {
                Step step = new Step()
                {
                    Number = 7,
                    FinishValue = GlobalModelGas.Cct,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "Cct",
                            Value = GlobalModelGas.Cct
                        },
                    }
                };
                values.Add(step);
            }
            else
            {
                Step step = new Step()
                {
                    Number = 7,
                    AllInputs = new List<Input> {
                        new Input()
                        {
                            Name = "Cct",
                            Error = "Не указана стехиометрическая концентрация горючего газа",
                        },
                    }
                };

                if (GlobalModelGas.Cct == "none-calculate")
                {
                    if (GlobalModelGas.Nc == "none")
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Nc",
                                Error = "Не указано",
                            }
                        );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Nc",
                                Value = GlobalModelGas.Nc
                            }
                        );
                    if (GlobalModelGas.Nh == "none")
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Nh",
                                Error = "Не указано",
                            }
                        );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Nh",
                                Value = GlobalModelGas.Nh
                            }
                        );
                    if (GlobalModelGas.No == "none")
                        step.AllInputs.Add(
                        new Input()
                        {
                            Name = "No",
                            Error = "Не указано",
                        }
                    );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "No",
                                Value = GlobalModelGas.No
                            }
                        );
                    if (GlobalModelGas.Nx == "none")
                        step.AllInputs.Add(
                        new Input()
                        {
                            Name = "Nx",
                            Error = "Не указано",
                        }
                    );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Nx",
                                Value = GlobalModelGas.Nx
                            }
                        );
                }
                values.Add(step);
            }
            #endregion
            #region Mkg
            if (GlobalModelGas.Mkg != "none" && GlobalModelGas.Mkg != "none-calculate")
            {
                Step step = new Step()
                {
                    Number = 8,
                    FinishValue = GlobalModelGas.Mkg,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "Mkg",
                            Value = GlobalModelGas.Mkg
                        },
                    }
                };
                values.Add(step);
            }
            else
            {
                Step step = new Step()
                {
                    Number = 8,
                    AllInputs = new List<Input> {
                        new Input()
                        {
                            Name = "Mkg",
                            Error = "Не указана масса",
                        },
                    }
                };

                if (GlobalModelGas.Mkg == "none-calculate")
                {
                    if (GlobalModelGas.Rog == "none")
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Rog",
                                Error = "Не указано",
                            }
                        );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Rog",
                                Value = GlobalModelGas.Rog
                            }
                        );
                    if (GlobalModelGas.P1 == "none")
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "P1",
                                Error = "Не указано",
                            }
                        );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "P1",
                                Value = GlobalModelGas.P1
                            }
                        );
                    if (GlobalModelGas.P2 == "none")
                        step.AllInputs.Add(
                        new Input()
                        {
                            Name = "P2",
                            Error = "Не указано",
                        }
                    );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "P2",
                                Value = GlobalModelGas.P2
                            }
                        );
                    if (GlobalModelGas.V == "none")
                        step.AllInputs.Add(
                        new Input()
                        {
                            Name = "V",
                            Error = "Не указано",
                        }
                    );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "V",
                                Value = GlobalModelGas.V
                            }
                        );
                    if (GlobalModelGas.T == "none")
                        step.AllInputs.Add(
                        new Input()
                        {
                            Name = "T",
                            Error = "Не указано",
                        }
                    );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "T",
                                Value = GlobalModelGas.T
                            }
                        );
                    if (GlobalModelGas.Q == "none")
                        step.AllInputs.Add(
                        new Input()
                        {
                            Name = "Q",
                            Error = "Не указано",
                        }
                    );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Q",
                                Value = GlobalModelGas.Q
                            }
                        );
                    for (int i = 0; i < GlobalModelGas.R.Count(); i++)
                    {
                        if (GlobalModelGas.R[i] == "none")
                            step.AllInputs.Add(
                            new Input()
                            {
                                Name = "R" + (i + 1),
                                Error = "Не указано",
                            }
                        );
                        else
                            step.AllInputs.Add(
                                new Input()
                                {
                                    Name = "R" + (i + 1),
                                    Value = GlobalModelGas.R[i]
                                }
                            );
                    }
                    for (int i = 0; i < GlobalModelGas.L.Count(); i++)
                    {
                        if (GlobalModelGas.L[i] == "none")
                            step.AllInputs.Add(
                            new Input()
                            {
                                Name = "L" + (i + 1),
                                Error = "Не указано",
                            }
                        );
                        else
                            step.AllInputs.Add(
                                new Input()
                                {
                                    Name = "L" + (i + 1),
                                    Value = GlobalModelGas.L[i]
                                }
                            );
                    }

                }
                values.Add(step);
            }
            #endregion
            #region DeplaP
            if (GlobalModelGas.DeltaP != "none")
            {
                Step step = new Step()
                {
                    Number = 9,
                    FinishValue = GlobalModelGas.DeltaP,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "DeltaP",
                            Value = GlobalModelGas.DeltaP
                        },

                    }
                };
                values.Add(step);
            }
            #endregion
            return Json(values, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetValueHFL(string id)
        {
            List<Step> values = new List<Step>();
            #region Pmax
            if (GlobalModelHFL.Pmax != "none")
            {
                Step step = new Step()
                {
                    Number = 1,
                    FinishValue = GlobalModelHFL.Pmax,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "Pmax",
                            Value = GlobalModelHFL.Pmax
                        },

                    }
                };
                values.Add(step);
            }
            #endregion
            #region P0
            if (GlobalModelHFL.P0 != "none")
            {
                Step step = new Step()
                {
                    Number = 2,
                    FinishValue = GlobalModelHFL.P0,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "P0",
                            Value = GlobalModelHFL.P0
                        },

                    }
                };
                values.Add(step);
            }
            #endregion
            #region Vcb
            if (GlobalModelHFL.Vcb != "none")
            {
                Step step = new Step()
                {
                    Number = 3,
                    FinishValue = GlobalModelHFL.Vcb,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "Vcb",
                            Value = GlobalModelHFL.Vcb
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
            #endregion
            #region Kh
            if (GlobalModelHFL.Kh != "none")
            {
                Step step = new Step()
                {
                    Number = 4,
                    FinishValue = GlobalModelHFL.Kh,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "Kh",
                            Value = GlobalModelHFL.Kh
                        },

                    }
                };
                values.Add(step);
            }
            #endregion
            #region Z
            if (GlobalModelHFL.Z != "none")
            {
                Step step = new Step()
                {
                    Number = 5,
                    FinishValue = GlobalModelHFL.Z,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "Z",
                            Value = GlobalModelHFL.Z
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
            #endregion
            #region Rogp
            if (GlobalModelHFL.Rogp != "none" && GlobalModelHFL.Rogp != "none-calculate")
            {
                Step step = new Step()
                {
                    Number = 6,
                    FinishValue = GlobalModelHFL.Rogp,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "Rogp",
                            Value = GlobalModelHFL.Rogp
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

                if (GlobalModelHFL.Rogp == "none-calculate")
                {
                    if (GlobalModelHFL.M == "none")
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
                                Value = GlobalModelHFL.M
                            }
                        );
                    if (GlobalModelHFL.V0 == "none")
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
                                Value = GlobalModelHFL.V0
                            }
                        );
                    if (GlobalModelHFL.Tp == "none")
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
                                Value = GlobalModelHFL.Tp
                            }
                        );
                }
                values.Add(step);
            }
            #endregion
            #region Cct
            if (GlobalModelHFL.Cct != "none" && GlobalModelHFL.Cct != "none-calculate")
            {
                Step step = new Step()
                {
                    Number = 7,
                    FinishValue = GlobalModelHFL.Cct,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "Cct",
                            Value = GlobalModelHFL.Cct
                        },
                    }
                };
                values.Add(step);
            }
            else
            {
                Step step = new Step()
                {
                    Number = 7,
                    AllInputs = new List<Input> {
                        new Input()
                        {
                            Name = "Cct",
                            Error = "Не указана стехиометрическая концентрация горючего газа",
                        },
                    }
                };

                if (GlobalModelHFL.Cct == "none-calculate")
                {
                    if (GlobalModelHFL.Nc == "none")
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Nc",
                                Error = "Не указано",
                            }
                        );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Nc",
                                Value = GlobalModelHFL.Nc
                            }
                        );
                    if (GlobalModelHFL.Nh == "none")
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Nh",
                                Error = "Не указано",
                            }
                        );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Nh",
                                Value = GlobalModelHFL.Nh
                            }
                        );
                    if (GlobalModelHFL.No == "none")
                        step.AllInputs.Add(
                        new Input()
                        {
                            Name = "No",
                            Error = "Не указано",
                        }
                    );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "No",
                                Value = GlobalModelHFL.No
                            }
                        );
                    if (GlobalModelHFL.Nx == "none")
                        step.AllInputs.Add(
                        new Input()
                        {
                            Name = "Nx",
                            Error = "Не указано",
                        }
                    );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Nx",
                                Value = GlobalModelHFL.Nx
                            }
                        );
                }
                values.Add(step);
            }
            #endregion
            #region 

            #endregion
            #region DeplaP
            if (GlobalModelHFL.DeltaP != "none")
            {
                Step step = new Step()
                {
                    Number = 9,
                    FinishValue = GlobalModelHFL.DeltaP,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "DeltaP",
                            Value = GlobalModelHFL.DeltaP
                        },

                    }
                };
                values.Add(step);
            }
            #endregion
            return Json(values, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}