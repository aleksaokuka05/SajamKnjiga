using Core.Interfaces;
using System;
using System.Text;

namespace Core.Models 
{
    public class Kupovina : ISerializable
    {
        public int Id { get; set; }

        public int PosetilacId { get; set; }
        public Posetilac? Posetilac { get; set; }

        public int KnjigaId { get; set; }
        public Knjiga? Knjiga { get; set; }

        public DateTime DatumKupovine { get; set; }

        public int Ocena { get; set; }

        public string Komentar { get; set; }

        public Kupovina()
        {
            Komentar = string.Empty;
        }

        public Kupovina(Posetilac posetilac, Knjiga knjiga, DateTime datumKupovine, int ocena, string komentar)
        {
            Posetilac = posetilac;
            PosetilacId = posetilac.Id;
            Knjiga = knjiga;
            KnjigaId = knjiga.Id;
            DatumKupovine = datumKupovine;
            Ocena = ocena;
            Komentar = komentar;
        }

        public string[] ToCSV()
        {
            string[] niz_polja =
            {
                Id.ToString(),
                PosetilacId.ToString(),
                KnjigaId.ToString(),
                DatumKupovine.ToString("dd.MM.yyyy"),
                Ocena.ToString(),
                Komentar
            };

            return niz_polja;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            PosetilacId = int.Parse(values[1]);
            KnjigaId = int.Parse(values[2]);
            DatumKupovine = DateTime.ParseExact(
                values[3],
                "dd.MM.yyyy",
                System.Globalization.CultureInfo.InvariantCulture);
            Ocena = int.Parse(values[4]);
            Komentar = values[5];
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("POSETILAC: " + Posetilac?.Ime + " " + Posetilac?.Prezime + ", ");
            sb.Append("KNJIGA: " + Knjiga?.Naziv + ", ");
            sb.Append("DATUM KUPOVINE: " + DatumKupovine.ToString("dd.MM.yyyy") + ", ");
            sb.Append("OCENA: " + Ocena + ", ");
            sb.Append("KOMENTAR: " + Komentar);

            return sb.ToString();
        }
    }
}
