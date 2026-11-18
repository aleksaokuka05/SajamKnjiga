using System;
using System.Collections.Generic;
using Core.Models;
using Core.Utils;

namespace Core.DAO
{
    public class KnjigaDAO
    {
        private readonly List<Knjiga> _knjige;
        private readonly Storage<Knjiga> _storage;

        public KnjigaDAO()
        {
            _storage = new Storage<Knjiga>("knjige.csv");
            _knjige = _storage.Load();
        }

        public Knjiga AddKnjiga(Knjiga knjiga)
        {
            _knjige.Add(knjiga);
            _storage.Save(_knjige);
            return knjiga;
        }

        public Knjiga? UpdateKnjiga(Knjiga knjiga)
        {
            Knjiga? staraKnjiga = GetKnjigaByISBN(knjiga.ISBN);

            if (staraKnjiga is null)
            {
                return null;
            }

            staraKnjiga.Naziv = knjiga.Naziv;
            staraKnjiga.Zanr = knjiga.Zanr;
            staraKnjiga.GodinaIzdanja = knjiga.GodinaIzdanja;
            staraKnjiga.Cena = knjiga.Cena;
            staraKnjiga.BrojStrana = knjiga.BrojStrana;
            staraKnjiga.Autori = knjiga.Autori;
            staraKnjiga.IzdavacSifra = knjiga.IzdavacSifra;
            staraKnjiga.Izdavac = knjiga.Izdavac;
            staraKnjiga.Kupci = knjiga.Kupci;
            staraKnjiga.ListaZeljaPosetilaca = knjiga.ListaZeljaPosetilaca;

            _storage.Save(_knjige);
            return staraKnjiga;
        }

        public Knjiga? GetKnjigaByISBN(string isbn)
        {
            foreach (Knjiga knjiga in _knjige)
            {
                if (knjiga.ISBN == isbn)
                {
                    return knjiga;
                }
            }

            return null;
        }

        public Knjiga? RemoveKnjiga(string isbn)
        {
            Knjiga? knjiga = GetKnjigaByISBN(isbn);

            if (knjiga is null)
            {
                return null;
            }

            _knjige.Remove(knjiga);
            _storage.Save(_knjige);

            return knjiga;
        }

        public List<Knjiga> GetAllKnjige()
        {
            return _knjige;
        }
    }
}
