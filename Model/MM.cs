using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenadżerPaczek.Model
{
    public class MM
    {
        public bool IsCheckd { get; set; }
        public string Trn_DokumentObcy { get; set; }
        public string Mag_Kod { get; set; }
        public string Data { get; set; }
        public string Opis { get; internal set; }
        public string Trn_Numer { get; internal set; }
    }
}
