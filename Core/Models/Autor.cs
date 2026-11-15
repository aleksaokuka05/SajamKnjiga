using System;
using System.Collections.Generic;
using System.Text;
using Core.Interfaces;

namespace Core.Models
{
    public class Autor : ISerializable
    {
        public string Ime { get; set; } 

        public string Prezime { get; set; } 

        public DateTime DatumRodjenja { get; set; }

        public Adresa Adresa { get; set; } 

        public string Telefon { get; set; } 

        public string Imejl { get; set; }

        public string BrojLicneKarte { get; set; } 

        public int GodineIskustva { get; set; }

        public List<Knjiga> Knjige { get; set; } 

        public Autor()
        {
            Adresa = new Adresa();
            Knjige = new List<Knjiga>();
        }

        public Autor(string ime, string prezime, DateTime datumRodjenja, Adresa adresa, string telefon, string imejl, string brojLicneKarte, int godineIskustva)
        {
            Ime = ime;
            Prezime = prezime;
            DatumRodjenja = datumRodjenja;
            Adresa = adresa;
            Telefon = telefon;
            Imejl = imejl;
            BrojLicneKarte = brojLicneKarte;
            GodineIskustva = godineIskustva;
            Knjige = new List<Knjiga>();
        }

        public string[] ToCSV()
        {
            string[] niz_polja =
            {
                Ime,
                Prezime,
                DatumRodjenja.ToString("dd.MM.yyyy"),
                Adresa.Ulica,
                Adresa.Broj,
                Adresa.Grad,
                Adresa.Drzava,
                Telefon,
                Imejl,
                BrojLicneKarte,
                GodineIskustva.ToString()
            };

            return niz_polja;
        }

        public void FromCSV(string[] values)
        {
            Ime = values[0];
            Prezime = values[1];
            DatumRodjenja = DateTime.ParseExact(
                values[2],
                "dd.MM.yyyy",
                System.Globalization.CultureInfo.InvariantCulture
            );

            Adresa = new Adresa(
                values[3],
                values[4],
                values[5],
                values[6]
            );

            Telefon = values[7];
            Imejl = values[8];
            BrojLicneKarte = values[9];
            GodineIskustva = int.Parse(values[10]);

            Knjige = new List<Knjiga>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("IME: " + Ime + ", ");
            sb.Append("PREZIME: " + Prezime + ", ");
            sb.Append("DATUM RODJENJA: " + DatumRodjenja.ToString("dd.MM.yyyy") + ", ");
            sb.Append("ADRESA: " + Adresa + ", ");
            sb.Append("TELEFON: " + Telefon + ", ");
            sb.Append("IMEJL: " + Imejl + ", ");
            sb.Append("BROJ LICNE KARTE: " + BrojLicneKarte + ", ");
            sb.Append("GODINE ISKUSTVA: " + GodineIskustva + ", ");
            sb.Append("KNJIGE: ");

            sb.AppendJoin(", ", Knjige.Select(knjiga => knjiga.Naziv));

            return sb.ToString();
        }


    }
}
