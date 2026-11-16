using System;
using System.Collections.Generic;
using System.Text;
using Core.Interfaces;

namespace Core.Models 
{
    public class Kupovina : ISerializable
    {
        public Posetilac Posetilac { get; set; }
        public Knjiga Knjiga { get; set; }
        public DateTime DatumKupovine { get; set; }
        public int Ocena { get; set; }
        public string Komentar { get; set; } = string.Empty;

        public Kupovina()
        {

        }

        public Kupovina(Posetilac posetilac, Knjiga knjiga, DateTime datumKupovine, int ocena, string komentar)
        {
            Posetilac = posetilac;
            Knjiga = knjiga;
            DatumKupovine = datumKupovine;
            Ocena = ocena;
            Komentar = komentar;
        }

        public string[] ToCSV()
        {
            string[] niz_polja =
            {
                Posetilac.BrojClanskeKarte,
                Knjiga.ISBN,
                DatumKupovine.ToString("dd.MM.yyyy"),
                Ocena.ToString(),
                Komentar
            };

            return niz_polja;
        }

        public void FromCSV(string[] values)
        {
            Posetilac = new Posetilac
            {
                BrojClanskeKarte = values[0]
            };

            Knjiga = new Knjiga
            {
                ISBN = values[1]
            };

            DatumKupovine = DateTime.ParseExact(
                values[2],
                "dd.MM.yyyy",
                System.Globalization.CultureInfo.InvariantCulture
            );

            Ocena = int.Parse(values[3]);
            Komentar = values[4];
        }
    }
}
