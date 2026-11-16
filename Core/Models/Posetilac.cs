using System;
using System.Collections.Generic;
using System.Text;
using Core.Interfaces;

namespace Core.Models
{
    public class Posetilac : ISerializable
    {
        public string Ime { get; set; } = string.Empty;
        public string Prezime { get; set; } = string.Empty;
        public DateTime DatumRodjenja { get; set; }
        public Adresa Adresa { get; set; }
        public string KontaktTelefon { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string BrojClanskeKarte { get; set; } = string.Empty;
        public int TrenutnaGodinaClanstva {  get; set; }
        public StatusPosetioca Status {  get; set; }
        public double ProsecnaOcena { get; set; }
        public List<Kupovina> Kupovine { get; set; } = new();
        public List<Knjiga> ListaZelja { get; set; } = new();

        public Posetilac()
        {
            Adresa = new Adresa();
        }

        public Posetilac(string ime, string prezime, DateTime datumRodjenja,
            Adresa adresa, string kontaktTelefon, string email,
            string brojClanskeKarte, int trenutnaGodinaClanstva,
            StatusPosetioca status, double prosecnaOcena)
        {
            Ime = ime;
            Prezime = prezime;
            DatumRodjenja = datumRodjenja;
            Adresa = adresa;
            KontaktTelefon = kontaktTelefon;
            Email = email;
            BrojClanskeKarte = brojClanskeKarte;
            TrenutnaGodinaClanstva = trenutnaGodinaClanstva;
            Status = status;
            ProsecnaOcena = prosecnaOcena;
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
                KontaktTelefon,
                Email,
                BrojClanskeKarte,
                TrenutnaGodinaClanstva.ToString(),
                Status.ToString(),
                ProsecnaOcena.ToString(System.Globalization.CultureInfo.InvariantCulture)
            };

            return niz_polja;
        }

        public void FromCSV(string[] values)
        {
            Ime = values[0];
            Prezime = values[1];
            DatumRodjenja = DateTime.ParseExact(values[2], "dd.MM.yyyy",
                System.Globalization.CultureInfo.InvariantCulture);

            Adresa = new Adresa
            {
                Ulica = values[3],
                Broj = values[4],
                Grad = values[5],
                Drzava = values[6]
            };

            KontaktTelefon = values[7];
            Email = values[8];
            BrojClanskeKarte = values[9];
            TrenutnaGodinaClanstva = int.Parse(values[10]);
            Status = Enum.Parse<StatusPosetioca>(values[11]);
            ProsecnaOcena = double.Parse(values[12],
            System.Globalization.CultureInfo.InvariantCulture);

            Kupovine = new List<Kupovina>();
            ListaZelja = new List<Knjiga>();
        }

    }
}
