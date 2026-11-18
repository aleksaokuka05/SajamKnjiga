using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Core.Interfaces;

namespace Core.Models
{
    public enum StatusPosetioca
    {
        Regular,
        VIP
    }
    public class Posetilac : ISerializable
    {
        public int Id { get; set; }

        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateTime DatumRodjenja { get; set; }

        public int AdresaId { get; set; }
        public Adresa? Adresa { get; set; }

        public string KontaktTelefon { get; set; }
        public string Email { get; set; }
        public string BrojClanskeKarte { get; set; }

        public int TrenutnaGodinaClanstva { get; set; }
        public StatusPosetioca Status { get; set; }
       

        public List<Kupovina> Kupovine { get; set; }
        public List<Knjiga> ListaZelja { get; set; }

        public double ProsecnaOcena
        {
            get
            {
                if (Kupovine.Count == 0)
                {
                    return 0;
                }

                return Kupovine.Average(kupovina => kupovina.Ocena);
            }
        }

        public Posetilac()
        {
            Ime = string.Empty;
            Prezime = string.Empty;
            KontaktTelefon = string.Empty;
            Email = string.Empty;
            BrojClanskeKarte = string.Empty;
            Status = StatusPosetioca.Regular;

            Kupovine = new List<Kupovina>();
            ListaZelja = new List<Knjiga>();
        }

        public Posetilac(string ime, string prezime, DateTime datumRodjenja,
            Adresa adresa, string kontaktTelefon, string email,
            string brojClanskeKarte, int trenutnaGodinaClanstva,
            StatusPosetioca status)
        {
            Ime = ime;
            Prezime = prezime;
            DatumRodjenja = datumRodjenja;
            Adresa = adresa;
            AdresaId = adresa.Id;
            KontaktTelefon = kontaktTelefon;
            Email = email;
            BrojClanskeKarte = brojClanskeKarte;
            TrenutnaGodinaClanstva = trenutnaGodinaClanstva;
            Status = status;
           

            Kupovine = new List<Kupovina>();
            ListaZelja = new List<Knjiga>();
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
                KontaktTelefon,
                Email,
                BrojClanskeKarte,
                TrenutnaGodinaClanstva.ToString(),
                Status.ToString()
        
            };

            return niz_polja;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Ime = values[1];
            Prezime = values[2];

            DatumRodjenja = DateTime.ParseExact(
                values[3],
                "dd.MM.yyyy",
                System.Globalization.CultureInfo.InvariantCulture);

            AdresaId = int.Parse(values[4]);
            KontaktTelefon = values[5];
            Email = values[6];
            BrojClanskeKarte = values[7];
            TrenutnaGodinaClanstva = int.Parse(values[8]);
            Status = Enum.Parse<StatusPosetioca>(values[9]);

            Kupovine = new List<Kupovina>();
            ListaZelja = new List<Knjiga>();
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("IME: " + Ime + ", ");
            sb.Append("PREZIME: " + Prezime + ", ");
            sb.Append("DATUM RODJENJA: " + DatumRodjenja.ToString("dd.MM.yyyy") + ", ");
            sb.Append("ADRESA: " + Adresa + ", ");
            sb.Append("TELEFON: " + KontaktTelefon + ", ");
            sb.Append("EMAIL: " + Email + ", ");
            sb.Append("BROJ CLANSKE KARTE: " + BrojClanskeKarte + ", ");
            sb.Append("GODINA CLANSTVA: " + TrenutnaGodinaClanstva + ", ");
            sb.Append("STATUS: " + Status + ", ");
            sb.Append("PROSECNA OCENA: " + ProsecnaOcena + ", ");

            sb.Append("KUPOVINE: ");
            sb.AppendJoin(", ", Kupovine.Select(kupovina => kupovina.Knjiga?.Naziv));

            sb.Append(", LISTA ZELJA: ");
            sb.AppendJoin(", ", ListaZelja.Select(knjiga => knjiga.Naziv));

            return sb.ToString();
        }

    }
}
