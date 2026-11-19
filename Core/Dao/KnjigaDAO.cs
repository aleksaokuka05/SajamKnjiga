using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;
using Core.Utils;

namespace Core.DAO
{
    // Izvor: OISISI, Vezbe 3, CRUDExample (DAO pattern)
    public class KnjigaDAO
    {
        private readonly List<Knjiga> _knjige;
        private readonly Storage<Knjiga> _storage;

        private int _sledeciId;

        public KnjigaDAO()
        {
            _storage = new Storage<Knjiga>("knjige.csv");
            _knjige = _storage.Load();
            _sledeciId = _knjige.Count == 0 ? 1 : _knjige.Max(knjiga => knjiga.Id) + 1;
        }


        private int GenerateId()
        {
            return _sledeciId++;
        }

        public Knjiga? GetKnjigaById(int id)
        {
            foreach (Knjiga knjiga in _knjige)
            {
                if (knjiga.Id == id)
                {
                    return knjiga;
                }
            }

            return null;
        }


        public Knjiga AddKnjiga(Knjiga knjiga)
        {
            knjiga.Id = GenerateId();
            _knjige.Add(knjiga);
            _storage.Save(_knjige);
            return knjiga;
        }

        public Knjiga? UpdateKnjiga(Knjiga knjiga)
        {
            Knjiga? staraKnjiga = GetKnjigaById(knjiga.Id);

            if (staraKnjiga is null)
            {
                return null;
            }

            staraKnjiga.ISBN = knjiga.ISBN;
            staraKnjiga.Naziv = knjiga.Naziv;
            staraKnjiga.Zanr = knjiga.Zanr;
            staraKnjiga.GodinaIzdanja = knjiga.GodinaIzdanja;
            staraKnjiga.Cena = knjiga.Cena;
            staraKnjiga.BrojStrana = knjiga.BrojStrana;
            staraKnjiga.Autori = knjiga.Autori;
            staraKnjiga.IzdavacId = knjiga.IzdavacId;
            staraKnjiga.Izdavac = knjiga.Izdavac;
            staraKnjiga.Kupci = knjiga.Kupci;
            staraKnjiga.ListaZeljaPosetilaca = knjiga.ListaZeljaPosetilaca;

            _storage.Save(_knjige);
            return staraKnjiga;
        }

     

        public Knjiga? RemoveKnjiga(int id)
        {
            Knjiga? knjiga = GetKnjigaById(id);

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
