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
        public static CalculateAViewModels GlobalModel;

        public JsonResult GetValueA(string id)
        {
            List<Step> values = new List<Step>();
            #region Pmax
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
            #endregion
            #region P0
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
            #endregion
            #region Vcb
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
            #endregion
            #region Kh
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
            #endregion
            #region Z
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
            #endregion
            #region Rogp
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
            #endregion
            #region Cct
            if (GlobalModel.Cct != "none" && GlobalModel.Cct != "none-calculate")
            {
                Step step = new Step()
                {
                    Number = 7,
                    FinishValue = GlobalModel.Cct,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "Cct",
                            Value = GlobalModel.Cct
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

                if (GlobalModel.Cct == "none-calculate")
                {
                    if (GlobalModel.Nc == "none")
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
                                Value = GlobalModel.Nc
                            }
                        );
                    if (GlobalModel.Nh == "none")
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
                                Value = GlobalModel.Nh
                            }
                        );
                    if (GlobalModel.No == "none")
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
                                Value = GlobalModel.No
                            }
                        );
                    if (GlobalModel.Nx == "none")
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
                                Value = GlobalModel.Nx
                            }
                        );
                }
                values.Add(step);
            }
            #endregion
            #region Mkg
            if (GlobalModel.Mkg != "none" && GlobalModel.Mkg != "none-calculate")
            {
                Step step = new Step()
                {
                    Number = 8,
                    FinishValue = GlobalModel.Mkg,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "Mkg",
                            Value = GlobalModel.Mkg
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

                if (GlobalModel.Mkg == "none-calculate")
                {
                    if (GlobalModel.Rog == "none")
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
                                Value = GlobalModel.Rog
                            }
                        );
                    if (GlobalModel.P1 == "none")
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
                                Value = GlobalModel.P1
                            }
                        );
                    if (GlobalModel.P2 == "none")
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
                                Value = GlobalModel.P2
                            }
                        );
                    if (GlobalModel.V == "none")
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
                                Value = GlobalModel.V
                            }
                        );
                    if (GlobalModel.T == "none")
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
                                Value = GlobalModel.T
                            }
                        );
                    if (GlobalModel.R[0] == "none")
                        step.AllInputs.Add(
                        new Input()
                        {
                            Name = "R",
                            Error = "Не указано",
                        }
                    );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "R",
                                Value = GlobalModel.R[0]
                            }
                        );
                    if (GlobalModel.Q == "none")
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
                                Value = GlobalModel.Q
                            }
                        );
                    if (GlobalModel.L[0] == "none")
                        step.AllInputs.Add(
                        new Input()
                        {
                            Name = "L",
                            Error = "Не указано",
                        }
                    );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "L",
                                Value = GlobalModel.L[0]
                            }
                        );
                }
                values.Add(step);
            }
            #endregion
            #region DeplaP
            if (GlobalModel.DeltaP != "none")
            {
                Step step = new Step()
                {
                    Number = 9,
                    FinishValue = GlobalModel.DeltaP,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "DeltaP",
                            Value = GlobalModel.DeltaP
                        },

                    }
                };
                values.Add(step);
            }
            #endregion
            return Json(values, JsonRequestBehavior.AllowGet);
        }
        public ActionResult A()
        {
            GlobalModel = new CalculateAViewModels()
            {
                GlobalStep = 1,
                DeltaP = "none",
                R = new List<string>(),
                L = new List<string>(),
            };
            GlobalModel.R.Add(null);
            GlobalModel.L.Add(null);
            ViewBag.Step = GlobalModel.GlobalStep;
            return View(new CalculateAViewModels());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Steps(CalculateAViewModels model, string submitButton)
        {
            int CurrentStep = 1;
            double Pmax = 0, P0 = 0, Vcb = 0, Kh = 0, Z = 0, Rogp = 0, Cct = 0, Mkg = 0;
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
                    if (Nc != 0)
                        GlobalModel.Nc = Nc.ToString();
                    else
                        GlobalModel.Nc = "none";
                    Nh = GetValue(model.Nh, ref FullData);
                    if (Nh != 0)
                        GlobalModel.Nh = Nh.ToString();
                    else
                        GlobalModel.Nh = "none";
                    Nx = GetValue(model.Nx, ref FullData);
                    if (Nx != 0)
                        GlobalModel.Nx = Nx.ToString();
                    else
                        GlobalModel.Nx = "none";
                    No = GetValue(model.No, ref FullData);
                    if (No != 0)
                        GlobalModel.No = No.ToString();
                    else
                        GlobalModel.No = "none";
                    if (FullData == 4)
                    {
                        Beta = Nc + (Nh - Nx) / 4 - No / 2;
                        GlobalModel.Beta = Beta.ToString();
                        Cct = 100 / (1 + 4.84 * Beta);
                        GlobalModel.Cct = Cct.ToString();
                        CurrentStep++;
                    }
                    else
                        GlobalModel.Cct = "none-calculate";
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
                    else
                        GlobalModel.Cct = "none";
                }
            }
            if (GlobalModel.GlobalStep >= 8)
            {
                if (submitButton == "mButton")
                {
                    double Rog, P1, P2, V, R, Q, T, L;
                    int FullData = 0;
                    Rog = GetValue(model.Rog, ref FullData);
                    if (Rog != 0)
                        GlobalModel.Rog = Rog.ToString();
                    else
                        GlobalModel.Rog = "none";
                    P1 = GetValue(model.P1, ref FullData);
                    if (P1 != 0)
                        GlobalModel.P1 = P1.ToString();
                    else
                        GlobalModel.P1 = "none";
                    P2 = GetValue(model.P2, ref FullData);
                    if (P2 != 0)
                        GlobalModel.P2 = P2.ToString();
                    else
                        GlobalModel.P2 = "none";
                    V = GetValue(model.V, ref FullData);
                    if (V != 0)
                        GlobalModel.V = V.ToString();
                    else
                        GlobalModel.V = "none";
                    R = GetValue(model.R[0], ref FullData);
                    if (R != 0)
                        GlobalModel.R[0] = R.ToString();
                    else
                        GlobalModel.R[0] = "none";
                    Q = GetValue(model.Q, ref FullData);
                    if (Q != 0)
                        GlobalModel.Q = Q.ToString();
                    else
                        GlobalModel.Q = "none";
                    T = GetValue(model.T, ref FullData);
                    if (T != 0)
                        GlobalModel.T = T.ToString();
                    else
                        GlobalModel.T = "none";
                    L = GetValue(model.L[0], ref FullData);
                    if (L != 0)
                        GlobalModel.L[0] = L.ToString();
                    else
                        GlobalModel.L[0] = "none";
                    if (FullData == 8)
                    {
                        double Va, Vt;
                        Va = 0.001 * P1 * V;
                        Vt = Q * T + Math.PI * P2 * (R * R * L);
                        Mkg = Rog * (Va + Vt);
                        GlobalModel.Mkg = Mkg.ToString();
                        CurrentStep++;
                    }
                    else
                        GlobalModel.Mkg = "none-calculate";
                }
                else
                {
                    GlobalModel.Rog = model.Rog;
                    GlobalModel.P1 = model.P1;
                    GlobalModel.P2 = model.P2;
                    GlobalModel.V = model.V;
                    GlobalModel.R = model.R;
                    GlobalModel.Q = model.Q;
                    GlobalModel.T = model.T;
                    GlobalModel.L = model.L;
                    Mkg = GetValue(model.Mkg, ref CurrentStep);
                    if (Mkg != 0)
                        GlobalModel.Mkg = Mkg.ToString();
                    else
                        GlobalModel.Mkg = "none";
                }
            }
            if (CurrentStep == 9) //Если все нужные 8 шагов есть, то считаем категорию
            {
                double DeplaP = (Pmax - P0) * (Mkg * Z / (Vcb * Rogp)) * (100 / Cct) * (1 / Kh);
                GlobalModel.DeltaP = DeplaP.ToString();
            }

            ViewBag.Z = new List<SelectListItem>
            {
                new SelectListItem { Text = "Водород - 1", Value = "1" },
                new SelectListItem { Text = "Горючие газы (кроме водорода) - 0,5", Value = "0,5" }
            };

            if (CurrentStep <= GlobalModel.GlobalStep)
                return HttpNotFound();
            GlobalModel.GlobalStep = CurrentStep;
            ViewBag.Step = GlobalModel.GlobalStep;

            return PartialView("Steps", GlobalModel);
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