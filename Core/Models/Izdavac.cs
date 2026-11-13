using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Izdavac
    {
        public string Sifra { get; set; } = string.Empty;
        public string Naziv {  get; set; } = string.Empty;
        public Autor SefIzdavaca { get; set; }
        public List<Autor> Autori { get; set; } = new();
        public List<Knjiga> Knjige { get; set; } = new();

    }
}
