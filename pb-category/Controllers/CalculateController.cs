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
        public static int DCSCount = 0;

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
        public PartialViewResult RLAdd()
        {
            GlobalModelGas.R.Add(null);
            GlobalModelGas.L.Add(null);
            return PartialView("Gas/RLAdd", GlobalModelGas);
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
        public ActionResult DCS()
        {
            var GlobalModelDCS = new CalculateDCSViewModel()
            {
                Name = new List<string>() { null },
                Qn = new List<string>() { null },
                G = new List<string>() { null },

            };
            ViewBag.GategoryEnd = "noend";

            Session["SelectMenu"] = 2;//Удалить потом

            return View("DCS/Index", GlobalModelDCS);
        }
        public PartialViewResult MaterialAdd()
        {
            ViewBag.MaterialCount = ++DCSCount;

            return PartialView("DCS/MaterialAdd");
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
                if (Pmax < 0)
                {
                    Pmax = 900;
                    CurrentStep++;
                }
                GlobalModelGas.Pmax = Pmax.ToString();
            }
            if (GlobalModelGas.GlobalStep >= 2)
            {
                P0 = GetValue(model.P0, ref CurrentStep);
                if (P0 < 0)
                {
                    P0 = 101;
                    CurrentStep++;
                }
                GlobalModelGas.P0 = P0.ToString();
            }
            if (GlobalModelGas.GlobalStep >= 3)
            {
                Vcb = GetValue(model.Vcb, ref CurrentStep);
                if (Vcb > 0)
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
                if (Kh < 0)
                {
                    Kh = 3;
                    CurrentStep++;
                }
                GlobalModelGas.Kh = Kh.ToString();
            }
            if (GlobalModelGas.GlobalStep >= 5)
            {
                Z = GetValue(model.Z, ref CurrentStep);
                if (Z > 0)
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
                    if (M > 0)
                        GlobalModelGas.M = M.ToString();
                    else
                        GlobalModelGas.M = "none";
                    V0 = GetValue(model.V0, ref FullData);
                    if (V0 > 0)
                        GlobalModelGas.V0 = V0.ToString();
                    else
                        GlobalModelGas.V0 = "none";
                    if (V0 == 0)
                        FullData--;
                    Tp = GetValue(model.Tp, ref FullData);
                    if (Tp > 0)
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
                    if (Rogp > 0)
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
                    if (Nc > 0)
                        GlobalModelGas.Nc = Nc.ToString();
                    else
                        GlobalModelGas.Nc = "none";
                    Nh = GetValue(model.Nh, ref FullData);
                    if (Nh > 0)
                        GlobalModelGas.Nh = Nh.ToString();
                    else
                        GlobalModelGas.Nh = "none";
                    Nx = GetValue(model.Nx, ref FullData);
                    if (Nx > 0)
                        GlobalModelGas.Nx = Nx.ToString();
                    else
                        GlobalModelGas.Nx = "none";
                    No = GetValue(model.No, ref FullData);
                    if (No > 0)
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
                    if (Cct > 0)
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
                    if (Rog > 0)
                        GlobalModelGas.Rog = Rog.ToString();
                    else
                        GlobalModelGas.Rog = "none";
                    P1 = GetValue(model.P1, ref FullData);
                    if (P1 > 0)
                        GlobalModelGas.P1 = P1.ToString();
                    else
                        GlobalModelGas.P1 = "none";
                    P2 = GetValue(model.P2, ref FullData);
                    if (P2 > 0)
                        GlobalModelGas.P2 = P2.ToString();
                    else
                        GlobalModelGas.P2 = "none";
                    V = GetValue(model.V, ref FullData);
                    if (V > 0)
                        GlobalModelGas.V = V.ToString();
                    else
                        GlobalModelGas.V = "none";
                    Q = GetValue(model.Q, ref FullData);
                    if (Q > 0)
                        GlobalModelGas.Q = Q.ToString();
                    else
                        GlobalModelGas.Q = "none";
                    T = GetValue(model.T, ref FullData);
                    if (T > 0)
                        GlobalModelGas.T = T.ToString();
                    else
                        GlobalModelGas.T = "none";


                    int countR = 0;
                    int temp = 0;
                    foreach (var item in model.R)
                    {
                        R = GetValue(item, ref countR);
                        if (R > 0)
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
                        if (L > 0)
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
                    if (Mkg > 0)
                        GlobalModelGas.Mkg = Mkg.ToString();
                    else
                        GlobalModelGas.Mkg = "none";
                }
            }
            if (CurrentStep == 9) //Если все нужные 8 шагов есть, то считаем категорию
            {
                double DeltaP = (Pmax - P0) * (Mkg * Z / (Vcb * Rogp)) * (100 / Cct) * (1 / Kh);
                GlobalModelGas.DeltaP = DeltaP.ToString();
                if (DeltaP > 5)
                    (Session["CategoryEnd"] as List<string>).Add("А");
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
        public ActionResult StepsHFL(CalculateHFLViewModel model, string submitButton)
        {
            int CurrentStep = 1;
            double Pmax = 0, P0 = 0, Vcb = 0, Kh = 0, Z = 0, Rogp = 0, Cct = 0, Mkg = 0;
            if (GlobalModelHFL.GlobalStep >= 1)
            {
                Pmax = GetValue(model.Pmax, ref CurrentStep);
                if (Pmax < 0)
                {
                    Pmax = 900;
                    CurrentStep++;
                }
                GlobalModelHFL.Pmax = Pmax.ToString();
            }
            if (GlobalModelHFL.GlobalStep >= 2)
            {
                P0 = GetValue(model.P0, ref CurrentStep);
                if (P0 < 0)
                {
                    P0 = 101;
                    CurrentStep++;
                }
                GlobalModelHFL.P0 = P0.ToString();
            }
            if (GlobalModelHFL.GlobalStep >= 3)
            {
                Vcb = GetValue(model.Vcb, ref CurrentStep);
                if (Vcb > 0)
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
                if (Kh < 0)
                {
                    Kh = 3;
                    CurrentStep++;
                }
                GlobalModelHFL.Kh = Kh.ToString();
            }
            if (GlobalModelHFL.GlobalStep >= 5)
            {
                Z = GetValue(model.Z, ref CurrentStep);
                if (Z >= 0)
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
                    if (M > 0)
                        GlobalModelHFL.M = M.ToString();
                    else
                        GlobalModelHFL.M = "none";
                    V0 = GetValue(model.V0, ref FullData);
                    if (V0 > 0)
                        GlobalModelHFL.V0 = V0.ToString();
                    else
                        GlobalModelHFL.V0 = "none";
                    if (V0 == 0)
                        FullData--;
                    Tp = GetValue(model.Tp, ref FullData);
                    if (Tp > 0)
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
                    if (Rogp > 0)
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
                    if (Nc > 0)
                        GlobalModelHFL.Nc = Nc.ToString();
                    else
                        GlobalModelHFL.Nc = "none";
                    Nh = GetValue(model.Nh, ref FullData);
                    if (Nh > 0)
                        GlobalModelHFL.Nh = Nh.ToString();
                    else
                        GlobalModelHFL.Nh = "none";
                    Nx = GetValue(model.Nx, ref FullData);
                    if (Nx > 0)
                        GlobalModelHFL.Nx = Nx.ToString();
                    else
                        GlobalModelHFL.Nx = "none";
                    No = GetValue(model.No, ref FullData);
                    if (No > 0)
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
                    if (Cct > 0)
                        GlobalModelHFL.Cct = Cct.ToString();
                    else
                        GlobalModelHFL.Cct = "none";
                }
            }
            if (GlobalModelHFL.GlobalStep >= 8)
            {
                GlobalModelHFL.Mkg = "none";
                double Mr, Memk, Msvokr;
                if (submitButton == "WButton")
                {
                    double Tp2, Ca, A, B, MW, Nu;
                    int fullW = 0;
                    if (model.Tp == null)
                    {
                        Tp2 = GetValue(model.Tp2, ref fullW);
                        if (Tp2 > 0)
                            GlobalModelHFL.Tp2 = Tp2.ToString();
                        else
                            GlobalModelHFL.Tp2 = "none";
                    }
                    else
                    {
                        Tp2 = GetValue(model.Tp, ref fullW);
                        GlobalModelHFL.Tp2 = Tp2.ToString();
                    }
                    A = GetValue(model.A, ref fullW);
                    if (A > 0)
                        GlobalModelHFL.A = A.ToString();
                    else
                        GlobalModelHFL.A = "none";
                    B = GetValue(model.B, ref fullW);
                    if (B > 0)
                        GlobalModelHFL.B = B.ToString();
                    else
                        GlobalModelHFL.B = "none";
                    Ca = GetValue(model.Ca, ref fullW);
                    if (Ca > 0)
                        GlobalModelHFL.Ca = Ca.ToString();
                    else
                        GlobalModelHFL.Ca = "none";
                    MW = GetValue(model.MW, ref fullW);
                    if (MW > 0)
                        GlobalModelHFL.MW = MW.ToString();
                    else
                        GlobalModelHFL.MW = "none";
                    Nu = GetValue(model.Nu, ref fullW);
                    if (Nu > 0)
                        GlobalModelHFL.Nu = Nu.ToString();
                    else
                        GlobalModelHFL.Nu = "none";

                    if (fullW == 6)
                    {
                        double W, Pn;
                        Pn = Math.Pow(10, A - B / (Tp2 + Ca));
                        W = Math.Pow(10, -6) * Nu * Pn * Math.Pow(MW, 0.5);
                        GlobalModelHFL.W = W.ToString();
                    }
                    else
                        GlobalModelHFL.W = "none-calculate";
                }
                if (submitButton == "MrButton")
                {
                    double W, V, koef, T;
                    int fullMr = 0;
                    W = GetValue(model.W, ref fullMr);
                    if (W > 0)
                        GlobalModelHFL.W = W.ToString();
                    else
                        GlobalModelHFL.W = "none";
                    V = GetValue(model.V, ref fullMr);
                    if (V > 0)
                        GlobalModelHFL.V = V.ToString();
                    else
                        GlobalModelHFL.V = "none";
                    T = GetValue(model.T, ref fullMr);
                    if (T > 0)
                        GlobalModelHFL.T = T.ToString();
                    else
                        GlobalModelHFL.T = "none";
                    koef = GetValue(model.Vkoef, ref fullMr);
                    if (koef > 0)
                        GlobalModelHFL.Vkoef = koef.ToString();
                    else
                        GlobalModelHFL.Vkoef = "none";
                    if (fullMr == 4)
                    {
                        double Fi, S;
                        Fi = V * koef;
                        S = Convert.ToDouble(Session["Length"]) * Convert.ToDouble(Session["Width"]);

                        if (Fi > S)
                            Fi = S;

                        Mr = W * T * Fi;
                        GlobalModelHFL.Mr = Mr.ToString();
                    }
                    else
                        GlobalModelHFL.Mr = "none-calculate";
                }
                if (submitButton == "mButton")
                {
                    int FullData = 0;
                    Mr = GetValue(model.Mr, ref FullData);
                    if (Mr > 0)
                        GlobalModelHFL.Mr = Mr.ToString();
                    else
                        GlobalModelHFL.Mr = "none";
                    Memk = GetValue(model.Memk, ref FullData);
                    if (Memk > 0)
                        GlobalModelHFL.Memk = Memk.ToString();
                    else
                        GlobalModelHFL.Memk = "none";
                    Msvokr = GetValue(model.Msvokr, ref FullData);
                    if (Msvokr > 0)
                        GlobalModelHFL.Msvokr = Msvokr.ToString();
                    else
                        GlobalModelHFL.Msvokr = "none";

                    if (FullData == 3)
                    {
                        Mkg = Mr + Memk + Msvokr;
                        GlobalModelHFL.Mkg = Mkg.ToString();
                        CurrentStep++;
                    }
                    else
                        GlobalModelHFL.Mkg = "none-calculate";
                }
                if (submitButton != "MrButton" && submitButton != "WButton" && submitButton != "mButton")
                {
                    Mkg = GetValue(model.Mkg, ref CurrentStep);
                    if (Mkg > 0)
                        GlobalModelHFL.Mkg = Mkg.ToString();
                    else
                        GlobalModelHFL.Mkg = "none";
                }
            }
            if (CurrentStep == 9) //Если все нужные 8 шагов есть, то считаем категорию
            {
                double DeltaP = (Pmax - P0) * (Mkg * Z / (Vcb * Rogp)) * (100 / Cct) * (1 / Kh);
                if (DeltaP > 5)
                    (Session["CategoryEnd"] as List<string>).Add("Б");

                GlobalModelHFL.DeltaP = DeltaP.ToString();
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StepsDCS(CalculateDCSViewModel model, string submitButton, int? q)
        {
            if (!ModelState.IsValid)
                return PartialView("DCS/Area", model);
            int count = model.Name.Count;
            double Q = 0;
            for (int i = 0; i < count; i++)
                Q += Convert.ToDouble(model.G[i]) * Convert.ToDouble(model.Qn[i]);

            double G, S = Convert.ToDouble(model.S);

            if (S < 10)
                G = Q / 10;
            else
                G = Q / S;

            ViewBag.GategoryEnd = "noend";
            ViewBag.Value = G.ToString();
            if (G > 2200)
            {
                (Session["CategoryEnd"] as List<string>).Add("В1");
                ViewBag.GategoryEnd = "end";
                ViewBag.GategoryStep = "C1";
            }
            if (G <= 2200 && G >= 1401)
            {
                (Session["CategoryEnd"] as List<string>).Add("В2");
                ViewBag.GategoryStep = "C2";
                if (!string.IsNullOrEmpty(model.H))
                {
                    bool result = double.TryParse(model.H, out double H);
                    if (result == true)
                    {
                        ViewBag.GategoryEnd = "end";
                        double value = 0.64 * 2200 * H * H;
                        if (Q >= value)
                        {
                            (Session["CategoryEnd"] as List<string>).Add("В1");
                            ViewBag.GategoryStep = "C1";
                        }
                    }
                }
            }
            if (G <= 1400 && G >= 181)
            {
                (Session["CategoryEnd"] as List<string>).Add("В3");
                ViewBag.GategoryStep = "C3";
                if (!string.IsNullOrEmpty(model.H))
                {
                    bool result = double.TryParse(model.H, out double H);
                    if (result == true)
                    {
                        ViewBag.GategoryEnd = "end";
                        double value = 0.64 * 1400 * H * H;
                        if (Q >= value)
                        {
                            ViewBag.GategoryStep = "C2";
                            (Session["CategoryEnd"] as List<string>).Add("В2");
                        }
                    }
                }
            }
            if (G <= 180 && G > 0)
            {
                ViewBag.GategoryStep = "C4";
                (Session["CategoryEnd"] as List<string>).Add("В4");

                if (Convert.ToDouble(model.S) < 10)
                    ViewBag.GategoryEnd = "end";

                double l = 0;
                if (q != null)
                {
                    switch (q.Value)
                    {
                        case 5:
                            l = 12;
                            break;
                        case 10:
                            l = 8;
                            break;
                        case 15:
                            l = 6;
                            break;
                        case 20:
                            l = 5;
                            break;
                        case 25:
                            l = 4;
                            break;
                        case 30:
                            l = 3.8;
                            break;
                        case 40:
                            l = 3.2;
                            break;
                        case 50:
                            l = 2.8;
                            break;
                        default:
                            l = 12;
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(submitButton))
                {
                    if (submitButton == "No")
                    {
                        (Session["CategoryEnd"] as List<string>).Add("В3");
                        ViewBag.GategoryStep = "C3";
                    }
                    ViewBag.GategoryEnd = "end";
                }
                else
                {
                    if (Convert.ToDouble(model.H) > 11)
                        ViewBag.L = l;
                    else
                        ViewBag.L = l + (11 - Convert.ToDouble(model.H));
                }

                if (l == 0 || string.IsNullOrEmpty(model.H))
                    return PartialView("DCS/Area", model);

                ViewBag.Lpr = true;
            }
            return PartialView("DCS/Area", model);
        }
        #region Вспомогательные функции
        private double GetValue(string value, ref int CurrentStep)
        {
            if (!string.IsNullOrEmpty(value))
                value = value.Replace(".", ",").Trim();
            double parse = -1;
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
            #region DeltaP
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
            #region Mkg 
            if (GlobalModelHFL.Mkg != "none" && GlobalModelHFL.Mkg != "none-calculate")
            {
                Step step = new Step()
                {
                    Number = 8,
                    FinishValue = GlobalModelHFL.Mkg,
                    AllInputs = new List<Input> {

                        new Input()
                        {
                            Name = "Mkg",
                            Value = GlobalModelHFL.Mkg
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
                //Если была нажата кнопка рассчета всей массы
                if (GlobalModelHFL.Mkg == "none-calculate")
                {
                    if (GlobalModelHFL.Mr == "none")
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Mr",
                                Error = "Не указано",
                            }
                        );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Mr",
                                Value = GlobalModelHFL.Mr
                            }
                        );
                    if (GlobalModelHFL.Memk == "none")
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Memk",
                                Error = "Не указано",
                            }
                        );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Memk",
                                Value = GlobalModelHFL.Memk
                            }
                        );
                    if (GlobalModelHFL.Msvokr == "none")
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Msvokr",
                                Error = "Не указано",
                            }
                        );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Msvokr",
                                Value = GlobalModelHFL.Msvokr
                            }
                        );
                }

                //Если была нажата кнопка рассчета Mr
                if (GlobalModelHFL.Mr != "none" && GlobalModelHFL.Mr != "none-calculate")
                {
                    step.AllInputs.Add(
                        new Input()
                        {
                            Name = "Mr",
                            Value = GlobalModelHFL.Mr
                        });

                }
                else
                { 
                    if (GlobalModelHFL.W == "none")
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "W",
                                Error = "Не указано",
                            }
                        );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "W",
                                Value = GlobalModelHFL.W
                            }
                        );
                    if (GlobalModelHFL.V == "none")
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
                                Value = GlobalModelHFL.V
                            }
                        );
                    if (GlobalModelHFL.T == "none")
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
                                Value = GlobalModelHFL.T
                            }
                        );
                }
                //Если была нажата кнопка рассчета W
                if (GlobalModelHFL.W != "none" && GlobalModelHFL.W != "none-calculate")
                {
                    step.AllInputs.Add(
                        new Input()
                        {
                            Name = "W",
                            Value = GlobalModelHFL.W
                        });
                }
                else
                { 
                    if (GlobalModelHFL.Nu == "none")
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Nu",
                                Error = "Не указано",
                            }
                        );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Nu",
                                Value = GlobalModelHFL.Nu
                            }
                        );
                    if (GlobalModelHFL.MW == "none")
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "MW",
                                Error = "Не указано",
                            }
                        );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "MW",
                                Value = GlobalModelHFL.MW
                            }
                        );
                    if (GlobalModelHFL.A == "none")
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "A",
                                Error = "Не указано",
                            }
                        );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "A",
                                Value = GlobalModelHFL.A
                            }
                        );
                    if (GlobalModelHFL.B == "none")
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "B",
                                Error = "Не указано",
                            }
                        );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "B",
                                Value = GlobalModelHFL.B
                            }
                        );
                    if (GlobalModelHFL.Ca == "none")
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Ca",
                                Error = "Не указано",
                            }
                        );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Ca",
                                Value = GlobalModelHFL.Ca
                            }
                        );
                    if (GlobalModelHFL.Tp2 == "none")
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Tp2",
                                Error = "Не указано",
                            }
                        );
                    else
                        step.AllInputs.Add(
                            new Input()
                            {
                                Name = "Tp2",
                                Value = GlobalModelHFL.Tp2
                            }
                        );
                }
                values.Add(step);
            }
            #endregion
            #region DeltaP
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