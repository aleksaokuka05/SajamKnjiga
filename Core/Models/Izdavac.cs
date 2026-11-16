using System;
using System.Collections.Generic;
using System.Text;
using Core.Interfaces;

namespace Core.Models
{
    public class Izdavac : ISerializable
    {
        public string Sifra { get; set; } = string.Empty;
        public string Naziv { get; set; } = string.Empty;
        public Autor SefIzdavaca { get; set; }
        public List<Autor> Autori { get; set; } = new();
        public List<Knjiga> Knjige { get; set; } = new();



        public Izdavac()
        {
            SefIzdavaca = new Autor();
        }

        public Izdavac(string sifra, string naziv, Autor sefIzdavaca)
        {
            Sifra = sifra;
            Naziv = naziv;
            SefIzdavaca = sefIzdavaca;
        }

        public string[] ToCSV()
        {
            string[] niz_polja =
            {
                Sifra,
                Naziv,
                SefIzdavaca.BrojLicneKarte
             };

            return niz_polja;
        }

        public void FromCSV(string[] values)
        {
            Sifra = values[0];
            Naziv = values[1];

            SefIzdavaca = new Autor
            {
                BrojLicneKarte = values[2]
            };

            Autori = new List<Autor>();
            Knjige = new List<Knjiga>();
        }

    }
}
