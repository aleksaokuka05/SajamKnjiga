using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Core.Interfaces;

namespace Core.Models
{
    public class Izdavac : ISerializable
    {

        public int Id { get; set; }
        public string Sifra { get; set; }
        public string Naziv { get; set; }
        public int SefIzdavacaId { get; set; }
        public Autor? SefIzdavaca { get; set; }
        public List<Autor> Autori { get; set; } 
        public List<Knjiga> Knjige { get; set; } 



        public Izdavac()
        {
            Sifra = string.Empty;
            Naziv = string.Empty;
            Autori = new List<Autor>();
            Knjige = new List<Knjiga>();
        
        }

        public Izdavac(string sifra, string naziv, Autor sefIzdavaca)
        {
            Sifra = sifra;
            Naziv = naziv;
            SefIzdavaca = sefIzdavaca;
            SefIzdavacaId = sefIzdavaca.Id;

            Autori = new List<Autor>();
            Knjige = new List<Knjiga>();
        }

        public string[] ToCSV()
        {
            string[] niz_polja =
            {
                Id.ToString(),
                Sifra,
                Naziv,
                SefIzdavacaId.ToString()
             };

            return niz_polja;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            Sifra = values[1];
            Naziv = values[2];
            SefIzdavacaId = int.Parse(values[3]);

            Autori = new List<Autor>();
            Knjige = new List<Knjiga>();
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("SIFRA: " + Sifra + ", ");
            sb.Append("NAZIV: " + Naziv + ", ");
            sb.Append("SEF IZDAVACA: " + (SefIzdavaca != null
                ? SefIzdavaca.Ime + " " + SefIzdavaca.Prezime
                : SefIzdavacaId.ToString()) + ", ");

            sb.Append("AUTORI: ");
            sb.AppendJoin(", ", Autori.Select(autor => autor.Ime + " " + autor.Prezime));

            sb.Append(", KNJIGE: ");
            sb.AppendJoin(", ", Knjige.Select(knjiga => knjiga.Naziv));

            return sb.ToString();
        }

    }
}
