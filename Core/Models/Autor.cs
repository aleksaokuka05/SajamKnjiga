using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Autor
    {
        public string Ime { get; set; } = string.Empty;

        public string Prezime { get; set; } = string.Empty;

        public DateTime DatumRodjenja { get; set; }

        public Adresa Adresa { get; set; } = new();

        public string Telefon { get; set; } = string.Empty;

        public string Imejl { get; set; } = string.Empty;

        public string BrojLicneKarte { get; set; } = string.Empty;

        public int GodineIskustva { get; set; }

        public List<Knjiga> Knjige { get; set; } = new();

        public Autor()
        {

        }

        public Autor(string ime, string prezime, DateTime datumRodjenja, Adresa adresa, string telefon, string imejl, string brojLicneKarte, int godineIskustva, List<Knjiga> knjige)
        {
            Ime = ime;
            Prezime = prezime;
            DatumRodjenja = datumRodjenja;
            Adresa = adresa;
            Telefon = telefon;
            Imejl = imejl;
            BrojLicneKarte = brojLicneKarte;
            GodineIskustva = godineIskustva;
            Knjige = knjige;
        }


    }
}
