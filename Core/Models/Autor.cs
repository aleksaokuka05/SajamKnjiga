
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interfaces;

namespace Core.Models
{
    public class Autor : ISerializable
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public int AdresaId { get; set; }
        public Adresa? Adresa { get; set; }
        public string Telefon { get; set; }
        public string Imejl { get; set; }
        public string BrojLicneKarte { get; set; }
        public int GodineIskustva { get; set; }
        public List<Knjiga> Knjige { get; set; }

        public Autor()
        {
            Ime = string.Empty;
            Prezime = string.Empty;
            Telefon = string.Empty;
            Imejl = string.Empty;
            BrojLicneKarte = string.Empty;
            Knjige = new List<Knjiga>();
        }

        public Autor(string ime, string prezime, DateTime datumRodjenja,
            Adresa adresa, string telefon, string imejl,
            string brojLicneKarte, int godineIskustva)
        {
            Ime = ime;
            Prezime = prezime;
            DatumRodjenja = datumRodjenja;
            Adresa = adresa;
            AdresaId = adresa.Id;
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
                Id.ToString(),
                Ime,
                Prezime,
                DatumRodjenja.ToString("dd.MM.yyyy"),
                AdresaId.ToString(),
                Telefon,
                Imejl,
                BrojLicneKarte,
                GodineIskustva.ToString()
            };

            return niz_polja;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Ime = values[1];
            Prezime = values[2];
            DatumRodjenja = DateTime.ParseExact(values[3], "dd.MM.yyyy",
                System.Globalization.CultureInfo.InvariantCulture);
            AdresaId = int.Parse(values[4]);
            Telefon = values[5];
            Imejl = values[6];
            BrojLicneKarte = values[7];
            GodineIskustva = int.Parse(values[8]);
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
