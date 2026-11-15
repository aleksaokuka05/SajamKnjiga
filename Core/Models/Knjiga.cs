
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Core.Interfaces;

namespace Core.Models
{
    public class Knjiga : ISerializable
    {
        public string ISBN { get; set; }

        public string Naziv { get; set; }

        public string Zanr { get; set; }

        public int GodinaIzdanja { get; set; }

        public decimal Cena { get; set; }

        public int BrojStrana { get; set; }

        public List<Autor> Autori { get; set; }

        public string IzdavacSifra { get; set; }

        public Izdavac Izdavac { get; set; }

        public List<Posetilac> Kupci { get; set; }

        public List<Posetilac> ListaZeljaPosetilaca { get; set; }


        public Knjiga()
        {
            Autori = new List<Autor>();
            Kupci = new List<Posetilac>();
            ListaZeljaPosetilaca = new List<Posetilac>();
        }

        public Knjiga(string isbn, string naziv, string zanr, int godinaIzdanja,
                      decimal cena, int brojStrana, Izdavac izdavac)
        {
            ISBN = isbn;
            Naziv = naziv;
            Zanr = zanr;
            GodinaIzdanja = godinaIzdanja;
            Cena = cena;
            BrojStrana = brojStrana;
            Izdavac = izdavac;
            IzdavacSifra = izdavac.Sifra;

            Autori = new List<Autor>();
            Kupci = new List<Posetilac>();
            ListaZeljaPosetilaca = new List<Posetilac>();
        }


        public string[] ToCSV()
        {
            string[] niz_polja =
            {
                ISBN,
                Naziv,
                Zanr,
                GodinaIzdanja.ToString(),
                Cena.ToString(System.Globalization.CultureInfo.InvariantCulture),
                BrojStrana.ToString(),
                IzdavacSifra
            };

            return niz_polja;
        }

        public void FromCSV(string[] values)
        {
            ISBN = values[0];
            Naziv = values[1];
            Zanr = values[2];
            GodinaIzdanja = int.Parse(values[3]);
            Cena = decimal.Parse(values[4], System.Globalization.CultureInfo.InvariantCulture);
            BrojStrana = int.Parse(values[5]);
            IzdavacSifra = values[6];

            Autori = new List<Autor>();
            Kupci = new List<Posetilac>();
            ListaZeljaPosetilaca = new List<Posetilac>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("ISBN: " + ISBN + ", ");
            sb.Append("NAZIV: " + Naziv + ", ");
            sb.Append("ZANR: " + Zanr + ", ");
            sb.Append("GODINA IZDANJA: " + GodinaIzdanja + ", ");
            sb.Append("CENA: " + Cena + ", ");
            sb.Append("BROJ STRANA: " + BrojStrana + ", ");
            sb.Append("IZDAVAC: " + (Izdavac != null ? Izdavac.Naziv : IzdavacSifra) + ", ");

            sb.Append("AUTORI: ");
            sb.AppendJoin(", ", Autori.Select(autor => autor.Ime + " " + autor.Prezime));

            sb.Append(", KUPCI: ");
            sb.AppendJoin(", ", Kupci.Select(kupac => kupac.Ime + " " + kupac.Prezime));

            sb.Append(", LISTA ZELJA: ");
            sb.AppendJoin(", ", ListaZeljaPosetilaca.Select(posetilac => posetilac.Ime + " " + posetilac.Prezime));

            return sb.ToString();
        }
    }
}
