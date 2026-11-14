using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Knjiga
    {
        public string ISBN { get; set; } = string.Empty;

        public string Naziv { get; set; } = string.Empty;

        public string Zanr { get; set; } = string.Empty;

        public int GodinaIzdanja { get; set; }

        public decimal Cena { get; set; }

        public int BrojStrana { get; set; }

        public List<Autor> Autori { get; set; } = new();

        public Izdavac Izdavac { get; set; }

        public List<Posetilac> Kupci { get; set; } = new();

        public List<Posetilac> ListaZeljaPosetilaca { get; set; } = new();


        public Knjiga()
        {
        }

        /*
            public Knjiga(string isbn, string naziv, string zanr, int godinaIzdanja, decimal cena, int brojStrana, List<Autor> autori, Izdavac izdavac)
                {
                    ISBN = isbn;
                    Naziv = naziv;
                    Zanr = zanr;
                    GodinaIzdanja = godinaIzdanja;
                    Cena = cena;
                    BrojStrana = brojStrana;
                    Autori = autori;
                    Izdavac = izdavac;
                }
        */







    }
}
