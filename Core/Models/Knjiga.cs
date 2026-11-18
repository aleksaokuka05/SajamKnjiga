using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Core.Interfaces;

namespace Core.Models
{
    public class Knjiga : ISerializable
    {
        public int Id { get; set; }
        public string ISBN { get; set; }
        public string Naziv { get; set; }
        public string Zanr { get; set; }
        public int GodinaIzdanja { get; set; }
        public decimal Cena { get; set; }
        public int BrojStrana { get; set; }

        public int IzdavacId { get; set; }
        public Izdavac? Izdavac { get; set; }

        public List<Autor> Autori { get; set; }
        public List<Posetilac> Kupci { get; set; }
        public List<Posetilac> ListaZeljaPosetilaca { get; set; }

        public Knjiga()
        {
            ISBN = string.Empty;
            Naziv = string.Empty;
            Zanr = string.Empty;
            Autori = new List<Autor>();
            Kupci = new List<Posetilac>();
            ListaZeljaPosetilaca = new List<Posetilac>();
        }

        public Knjiga(string isbn, string naziv, string zanr,
            int godinaIzdanja, decimal cena, int brojStrana,
            Izdavac izdavac)
        {
            ISBN = isbn;
            Naziv = naziv;
            Zanr = zanr;
            GodinaIzdanja = godinaIzdanja;
            Cena = cena;
            BrojStrana = brojStrana;
            Izdavac = izdavac;
            IzdavacId = izdavac.Id;

            Autori = new List<Autor>();
            Kupci = new List<Posetilac>();
            ListaZeljaPosetilaca = new List<Posetilac>();
        }

        public string[] ToCSV()
        {
            string[] niz_polja =
            {
                Id.ToString(),
                ISBN,
                Naziv,
                Zanr,
                GodinaIzdanja.ToString(),
                Cena.ToString(CultureInfo.InvariantCulture),
                BrojStrana.ToString(),
                IzdavacId.ToString()
            };

            return niz_polja;
        }

        public void FromCSV(string[] values)
        {
            Id = int.Parse(values[0]);
            ISBN = values[1];
            Naziv = values[2];
            Zanr = values[3];
            GodinaIzdanja = int.Parse(values[4]);
            Cena = decimal.Parse(values[5], CultureInfo.InvariantCulture);
            BrojStrana = int.Parse(values[6]);
            IzdavacId = int.Parse(values[7]);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("ISBN: " + ISBN + ", ");
            sb.Append("NAZIV: " + Naziv + ", ");
            sb.Append("ZANR: " + Zanr + ", ");
            sb.Append("GODINA IZDANJA: " + GodinaIzdanja + ", ");
            sb.Append("CENA: " + Cena.ToString("F2") + ", ");
            sb.Append("BROJ STRANA: " + BrojStrana + ", ");
            sb.Append("IZDAVAC: " + Izdavac?.Naziv + ", ");
            sb.Append("AUTORI: ");
            sb.AppendJoin(", ", Autori.Select(autor => autor.Ime + " " + autor.Prezime));

            return sb.ToString();
        }
    }
}