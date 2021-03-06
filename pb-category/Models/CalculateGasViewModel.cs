﻿using System.Collections.Generic;

namespace pb_category.Models
{
    public class CalculateGasViewModel
    {
        public int GlobalStep { get; set; }
        public string Pmax { get; set; }
        public string P0 { get; set; }
        public string Vcb { get; set; }
        public string Kh { get; set; }
        public string Z { set; get; }
        public string Rogp { get; set; }
        public string M { get; set; }
        public string Tp { get; set; }
        public string V0 { get; set; }
        public string Cct { get; set; }
        public string Beta { get; set; }
        public string Nc { get; set; }
        public string Nh { get; set; }
        public string Nx { get; set; }
        public string No { get; set; }
        public string Mkg { get; set; }
        public string Rog { get; set; }
        public string Va { get; set; }
        public string P1 { get; set; }
        public string V { get; set; }
        public string V1t { get; set; }
        public string Q { get; set; }
        public string T { get; set; }
        public string V2t { get; set; }
        public string P2 { get; set; }
        public List<string> R { get; set; }
        public List<string> L { get; set; }
        public string DeltaP { get; set; }
    }
}