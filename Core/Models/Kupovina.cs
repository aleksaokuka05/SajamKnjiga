using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Kupovina
    {
        public Posetilac Posetilac { get; set; }
        public Knjiga Knjiga { get; set; }
        public DateTime DatumKupovine { get; set; }
        public int Ocena { get; set; }
        public string Komentar { get; set; } = string.Empty;


    }
}
